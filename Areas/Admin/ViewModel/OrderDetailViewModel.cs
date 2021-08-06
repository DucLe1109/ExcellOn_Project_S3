using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }

        public Nullable<System.DateTime> OrderDetail_DateStart { get; set; }

        public Nullable<System.DateTime> OrderDetail_DateEnd { get; set; }

        public Nullable<int> OrderDetail_Status { get; set; }

        public Nullable<int> OrderDetail_NumberOfPeople { get; set; }

        public Nullable<int> ServiceId { get; set; }

        public Nullable<int> OrdersId { get; set; }
    }
}