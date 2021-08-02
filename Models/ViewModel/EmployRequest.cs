using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Models.ViewModel
{
    public class EmployRequest
    {
        public int Service_Id { get; set; }
        public int Number_Of_Employee { get; set; }
        public DateTime Date_Start { get; set; }
        public DateTime Date_End { get; set; }
        public string Description { get; set; }
    }
}