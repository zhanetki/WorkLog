using WorkLog.Data;
using System;
using System.Linq;

namespace WorkLog;

public partial class MainPage : ContentPage
{
    private readonly Databaza db = new(); 

    public MainPage()
    {
        InitializeComponent();
        UpdateStatistics();
    }
    private async void UpdateStatistics()
    {
        try
        {
            int totalHours = await GetTotalWorkHours();
            int totalEmployees = await GetTotalEmployees();
            int totalProjects = await GetTotalProjects();

            TotalHoursLabel.Text = totalHours.ToString();
            TotalEmployeesLabel.Text = totalEmployees.ToString();
            TotalProjectsLabel.Text = totalProjects.ToString();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Chyba", $"Nepodařilo se načíst statistiky.\n{ex.Message}", "OK");
        }
    }
    private Task<int> GetTotalWorkHours()
    {
        var records = db.GetAllWorkRecords();
        int totalHours = records.Sum(r => r.HoursWorked);
        return Task.FromResult(totalHours);
    }

    private Task<int> GetTotalEmployees()
    {
        return Task.FromResult(db.GetAllEmployees().Count);
    }

    private Task<int> GetTotalProjects()
    {
        return Task.FromResult(db.GetAllProjects().Count);
    }

    private async void OnEmployeesPageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EmployeesPage));
    }

    private async void OnProjectsPageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProjectsPage));
    }

    private async void OnAddEmployeePageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddEmployeePage));
    }

    private async void OnWorkRecordsPageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(WorkRecordsPage));
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateStatistics(); 
    }
}
