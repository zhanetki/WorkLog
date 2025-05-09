using WorkLog.Data;

namespace WorkLog;

public partial class AddProjectPage : ContentPage
{
    public AddProjectPage()
	{  
		InitializeComponent();  
	}
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string name = ProjectNameEntry.Text?.Trim() ?? "";
        string description = ProjectDescriptionEditor.Text?.Trim() ?? "";

        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Chyba", "Název projektu je povinný.", "OK");
            return;
        }
        var db = new Databaza();
        db.AddProject(name, description);

        await DisplayAlert("Úspìch", "Projekt byl pøidán.", "OK");
        await Shell.Current.GoToAsync("..");
    }
    private async void OnBackToHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }


}