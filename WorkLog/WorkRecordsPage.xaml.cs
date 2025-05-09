using WorkLog.Data;
using WorkLog.Models;

namespace WorkLog;

public partial class WorkRecordsPage : ContentPage
{
    public WorkRecordsPage()
    {
        InitializeComponent();
        LoadRecords();
    }

    private async void OnAddRecordClicked(object sender, EventArgs e)
    {
        
        await Shell.Current.GoToAsync(nameof(AddWorkRecordPage));
    }
    


private async void OnDeleteRecordClicked(object sender, EventArgs e)
{
    if ((sender as Button)?.BindingContext is WorkRecord record)
    {
        bool confirm = await DisplayAlert("Potvrdit", $"Opravdu chcete smazat záznam z projektu {record.ProjectName}?", "Ano", "Ne");
        if (confirm)
        {
            var db = new Databaza();
            db.DeleteWorkRecord(record.Id);
            LoadRecords(); 
        }
    }
}



private void LoadRecords()
    {
        var db = new Databaza();
        var records = db.GetAllWorkRecords();
        RecordsCollectionView.ItemsSource = records;
    }

    private async void OnBackToHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadRecords();
    }
}
