using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using WorkLog.Models;

namespace WorkLog.Data
{
    public class Databaza
    {
        private string dbPath;


    public Databaza()
        {
            dbPath = Path.Combine(FileSystem.AppDataDirectory, "worklog.db");
            InitializeDatabase();
            System.Diagnostics.Debug.WriteLine($"DB path: {dbPath}");

        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string createEmployeesTable = @"
              CREATE TABLE IF NOT EXISTS Employees (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Position TEXT,
    Department TEXT,
    HourlyRate REAL NOT NULL
);
;";

                string createProjectsTable = @"
                CREATE TABLE IF NOT EXISTS Projects (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT
                );";

                string createWorkRecordsTable = @"
                 CREATE TABLE IF NOT EXISTS WorkRecords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EmployeeId INTEGER,
                ProjectId INTEGER,
                Date TEXT NOT NULL,
                HoursWorked INTEGER NOT NULL,
                 FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
                FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
                    );";

                using (var command = new SqliteCommand(createEmployeesTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqliteCommand(createProjectsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqliteCommand(createWorkRecordsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddEmployee(string firstName, string lastName, string position, string department, double hourlyRate)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Employees (FirstName, LastName, Position, Department, HourlyRate) VALUES (@firstName, @lastName, @position, @department, @hourlyRate)";
                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@position", position);
                    command.Parameters.AddWithValue("@department", department);
                    command.Parameters.AddWithValue("@hourlyRate", hourlyRate);
                  
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddProject(string name, string description)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Projects (Name, Description) VALUES (@name, @description)";
                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@description", description);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddWorkRecord(int employeeId, int projectId, string date, int hoursWorked)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string insertQuery = "INSERT INTO WorkRecords (EmployeeId, ProjectId, Date, HoursWorked) VALUES (@employeeId, @projectId, @date, @hoursWorked)";
                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    command.Parameters.AddWithValue("@projectId", projectId);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@hoursWorked", hoursWorked);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProject(int projectId)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Projects WHERE Id = @id";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", projectId);
                    command.ExecuteNonQuery();
                }
            }
        }

   
        public void DeleteWorkRecord(int workRecordId)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM WorkRecords WHERE Id = @id";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", workRecordId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateEmployee(int employeeId, string name, string position, string department, double hourlyRate)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string updateQuery = "UPDATE Employees SET Name = @name, Position = @position, Department = @department, HourlyRate = @hourlyRate WHERE Id = @id";
                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", employeeId);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@position", position);
                    command.Parameters.AddWithValue("@department", department);
                    command.Parameters.AddWithValue("@hourlyRate", hourlyRate);
                   
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProject(int projectId, string name, string description)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string updateQuery = "UPDATE Projects SET Name = @name, Description = @description WHERE Id = @id";
                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", projectId);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@description", description);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateWorkRecord(int workRecordId, int employeeId, int projectId, string date)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string updateQuery = "UPDATE WorkRecords SET EmployeeId = @employeeId, ProjectId = @projectId, Date = @date WHERE Id = @id";
                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", workRecordId);
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    command.Parameters.AddWithValue("@projectId", projectId);
                    command.Parameters.AddWithValue("@date", date);
                   
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string selectQuery = "SELECT Id, FirstName, LastName, Position, Department, HourlyRate FROM Employees";

                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var employee = new Employee
                        {
                            Id = id,
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Position = reader.GetString(3),
                            Department = reader.GetString(4),
                            HourlyRate = reader.GetDouble(5),
                        };

                        // Подсчет отработанных часов
                        employee.TotalWorkedHours = CalculateTotalHours(id);
                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }
        private int CalculateTotalHours(int employeeId)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
            string query = "SELECT SUM(HoursWorked) FROM WorkRecords WHERE EmployeeId = @id";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", employeeId);

            var result = command.ExecuteScalar();
            if (result == DBNull.Value || result == null)
                return 0;

            return Convert.ToInt32(result);
        }
        public List<Project> GetAllProjects()
        {
            var projects = new List<Project>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string selectQuery = "SELECT Id, Name, Description FROM Projects";

                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var project = new Project
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2)
                        };
                        projects.Add(project);
                    }
                }
            }

            return projects;
        }
        public List<WorkRecord> GetAllWorkRecords()
        {
            var records = new List<WorkRecord>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string selectQuery = @"
            SELECT wr.Id, wr.EmployeeId, wr.ProjectId, wr.Date, wr.HoursWorked,
       e.FirstName || ' ' || e.LastName AS EmployeeName,
       p.Name AS ProjectName

            FROM WorkRecords wr
            LEFT JOIN Employees e ON wr.EmployeeId = e.Id
            LEFT JOIN Projects p ON wr.ProjectId = p.Id";

                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new WorkRecord
                        {
                            Id = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            ProjectId = reader.GetInt32(2),
                            Date = reader.GetString(3),
                            HoursWorked = reader.GetInt32(4),
                            EmployeeName = reader.GetString(5),
                            ProjectName = reader.GetString(6)
                        };
                        records.Add(record);
                    }
                }
            }

            return records;
        }
        public void DeleteEmployee(int id)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Employees WHERE Id = @id";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public double CalculateEmployeeSalary(int employeeId)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string query = "SELECT SUM(HoursWorked) FROM WorkRecords WHERE EmployeeId = @id";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", employeeId);

            var result = command.ExecuteScalar();
            long totalHours = result == DBNull.Value ? 0 : Convert.ToInt64(result);

            var employee = GetAllEmployees().FirstOrDefault(e => e.Id == employeeId);
            if (employee == null) return 0;

            return totalHours * employee.HourlyRate;
        }

    }

}