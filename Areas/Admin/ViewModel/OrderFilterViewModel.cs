using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class OrderFilterViewModel
    {
        public List<int> List_Order_Status_Id { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
    }
}