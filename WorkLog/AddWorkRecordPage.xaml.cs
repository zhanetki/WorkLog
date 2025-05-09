using WorkLog.Data;
using WorkLog.Models;

namespace WorkLog;

public partial class AddWorkRecordPage : ContentPage
{
    private List<Employee> employees = new();
    private List<Project> projects = new();

    public AddWorkRecordPage()
    {
        InitializeComponent();
        LoadData();
    }
    private void LoadData()
    {
        try
        {
            var db = new Databaza();

            employees = db.GetAllEmployees() ?? new List<Employee>();
            projects = db.GetAllProjects() ?? new List<Project>();

            Console.WriteLine($"Zaměstnanců: {employees.Count}, Projektů: {projects.Count}");

            EmployeePicker.ItemsSource = employees;
            ProjectPicker.ItemsSource = projects;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba načítání dat: {ex.Message}");
            DisplayAlert("Chyba", "Nepodařilo se načíst zaměstnance nebo projekty.", "OK");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (EmployeePicker.SelectedItem is not Employee selectedEmployee ||
            ProjectPicker.SelectedItem is not Project selectedProject ||
            !int.TryParse(HoursEntry.Text, out int hours))
        {
            await DisplayAlert("Chyba", "Zkontrolujte zadané hodnoty.", "OK");
            return;
        }

        string date = WorkDatePicker.Date.ToString("yyyy-MM-dd");

        try
        {
            var db = new Databaza();
            db.AddWorkRecord(selectedEmployee.Id, selectedProject.Id, date, hours);
            await DisplayAlert("Hotovo", "Záznam byl uložen.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Chyba při ukládání", ex.Message, "OK");
        }
    }

    private async void OnBackToHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}
