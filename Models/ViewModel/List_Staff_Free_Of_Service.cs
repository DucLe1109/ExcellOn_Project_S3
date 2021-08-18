using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Models.ViewModel
{
    public class List_Staff_Free_Of_Service
    {
        public DateTime Date_Start { get; set; }
        public DateTime Date_End { get; set; }
        public int list_staff_free_count { get; set; }
        public Service service { get; set; }
    }
}