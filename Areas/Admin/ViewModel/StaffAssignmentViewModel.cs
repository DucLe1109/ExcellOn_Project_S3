using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class StaffAssignmentViewModel
    {
        public int total_page { get; set; }
        public List<StaffViewModel> list_staff { get; set; }
    }
}