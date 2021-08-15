using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string Order_DateCreate { get; set; }

        public string Order_Description { get; set; }

        public Nullable<double> Order_TotalCost { get; set; }

        public Nullable<int> Order_Status { get; set; }

        public Nullable<int> CustomerId { get; set; }
    }
}