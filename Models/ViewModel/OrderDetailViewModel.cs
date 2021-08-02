using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Models.ViewModel
{
    public class OrderDetailViewModel
    {
        public int Number_Of_Employee { get; set; }
        public DateTime Employed_Date { get; set; }
        public DateTime Ended_Date { get; set; }
        public string Description { get; set; }

        public int Total_Price { get; set; }
        public int Total_Day { get; set; }
        public int Service_Id { get; set; }
    }
}