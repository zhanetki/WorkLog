using System.Collections.ObjectModel;
using WorkLog.Data;
using WorkLog.Models;

namespace WorkLog
{
    public partial class EmployeesPage : ContentPage
    {
        private Databaza _database = new Databaza();
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public EmployeesPage()
        {
            InitializeComponent();
            LoadEmployees();
            EmployeesCollectionView.ItemsSource = Employees;
        }

        private void LoadEmployeesFromDb()
        {
            Employees.Clear();
            var allEmployees = _database.GetAllEmployees();
            foreach (var emp in allEmployees)
            {
                Employees.Add(emp);
            }
        }
        private async void OnDeleteEmployeeClicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.BindingContext is Employee employee)
            {
                bool confirm = await DisplayAlert("Potvrdit", $"Opravdu chcete smazat {employee.Name}?", "Ano", "Ne");
                if (confirm)
                {
                    var db = new Databaza();
                    db.DeleteEmployee(employee.Id);
                    LoadEmployees(); // Обновляем список
                }
            }
        }
        private void LoadEmployees()
        {
            try
            {
                Employees.Clear();
                var db = new Databaza();
                var allEmployees = db.GetAllEmployees();
                var allRecords = db.GetAllWorkRecords();

                foreach (var emp in allEmployees)
                {
                    int totalHours = allRecords
                        .Where(r => r.EmployeeId == emp.Id)
                        .Sum(r => r.HoursWorked);
                    emp.TotalWorkedHours = totalHours;
                    Employees.Add(emp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке сотрудников: " + ex.Message);
            }
        }

        private async void OnCalculateSalaryClicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.BindingContext is Employee employee)
            {
                var db = new Databaza();
                double salary = db.CalculateEmployeeSalary(employee.Id);
                await DisplayAlert("Mzda", $"Výpočet mzdy: {salary} Kč", "OK");
            }
        }

        private async void OnBackToHomeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadEmployees();
            MessagingCenter.Subscribe<AddWorkRecordPage>(this, "WorkRecordAdded", (sender) =>
            {
                LoadEmployees();
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AddWorkRecordPage>(this, "WorkRecordAdded");
        }


    }
}
