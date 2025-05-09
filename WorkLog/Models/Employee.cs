using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLog.Models
{
   public class Employee
    {

        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Name => $"{FirstName} {LastName}";
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public double HourlyRate { get; set; }

        public int TotalWorkedHours { get; set; }
        public string DisplayedHours => $"Odpracované hodiny: {TotalWorkedHours}";
    }
}
    
