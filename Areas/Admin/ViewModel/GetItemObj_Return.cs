using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class GetItemObj_Return
    {
        public List<StaffViewModel> list_staff { get; set; }
        public List<int> list_staff_id_checked { get; set; }
    }
}