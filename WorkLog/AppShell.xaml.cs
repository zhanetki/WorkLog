namespace WorkLog
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(EmployeesPage), typeof(EmployeesPage));
            Routing.RegisterRoute(nameof(AddEmployeePage), typeof(AddEmployeePage));
            Routing.RegisterRoute(nameof(ProjectsPage), typeof(ProjectsPage));
            Routing.RegisterRoute(nameof(WorkRecordsPage), typeof(WorkRecordsPage));
            Routing.RegisterRoute(nameof(AddProjectPage), typeof(AddProjectPage));
            Routing.RegisterRoute(nameof(AddWorkRecordPage), typeof(AddWorkRecordPage));

        }
    }
}
