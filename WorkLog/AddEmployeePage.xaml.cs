using Microsoft.Maui.Controls;
using WorkLog.Data;
using WorkLog.Models; 

namespace WorkLog
{
    public partial class AddEmployeePage : ContentPage

    {
        private Databaza _database = new Databaza();
        public AddEmployeePage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string firstName = FirstNameEntry.Text?.Trim() ?? "";
            string lastName = LastNameEntry.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                await DisplayAlert("Chyba", "Prosím, vyplňte všechna pole správně.", "OK");
                return;
            }
            string position = PositionEntry.Text?.Trim() ?? "";
            string department = DepartmentEntry.Text?.Trim() ?? "";
            string rateText = RateEntry.Text?.Trim() ?? "";

            if (!double.TryParse(rateText, out double hourlyRate))
            {
                await DisplayAlert("Chyba", "Neplatná hodinová mzda.", "OK");
                return;
            }
            try
            {
                _database.AddEmployee(firstName, lastName, position, department, hourlyRate);

                await DisplayAlert("Úspěch", "Zaměstnanec byl přidán.", "OK");
                await Shell.Current.GoToAsync("//EmployeesPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Chyba", $"Nepodařilo se uložit zaměstnance.\n{ex.Message}", "OK");
            }
        }
        private async void OnBackToHomeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

    }

}
