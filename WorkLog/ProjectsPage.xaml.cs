using System.Collections.ObjectModel;
using WorkLog.Models;
using WorkLog.Data;

namespace WorkLog
{
    public partial class ProjectsPage : ContentPage
    {
        private Databaza _database = new Databaza();
        public ObservableCollection<Project> Projects { get; set; } = new ObservableCollection<Project>();

        public ProjectsPage()
        {
            InitializeComponent();
            LoadProjectsFromDb();
            ProjectsCollectionView.ItemsSource = Projects;
        }

        private void LoadProjectsFromDb()
        {
            Projects.Clear();
            var allProjects = _database.GetAllProjects();
            foreach (var proj in allProjects)
            {
                Projects.Add(proj);
            }
        }

        private async void OnAddProjectClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddProjectPage));
        }
        private async void OnBackToHomeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        private async void OnDeleteProjectClicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.BindingContext is Project project)
            {
                bool confirm = await DisplayAlert("Potvrdit", $"Opravdu chcete smazat projekt \"{project.Name}\"?", "Ano", "Ne");
                if (confirm)
                {
                    var db = new Databaza();
                    db.DeleteProject(project.Id);
                    LoadProjects();
                }
            }
        }
        private void LoadProjects()
        {
            var db = new Databaza();
            var projects = db.GetAllProjects();
            ProjectsCollectionView.ItemsSource = projects;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadProjects();
        }


    }
}
