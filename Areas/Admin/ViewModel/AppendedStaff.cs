using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class AppendedStaff
    {
        public List<StaffViewModel> list_staff_append { get; set; }
        public bool isLoadMore { get; set; }
    }
}