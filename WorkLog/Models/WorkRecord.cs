using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLog.Models
{
   public class WorkRecord
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string Date { get; set; } = string.Empty;
        public int HoursWorked { get; set; }
        public string EmployeeName { get; set; } = "";
        public string ProjectName { get; set; } = "";
    }
}
