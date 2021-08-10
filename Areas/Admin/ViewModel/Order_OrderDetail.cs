using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class Order_OrderDetail
    {
        public Order Orders { get; set; }
        public List<OrderDetail> List_OrderDetail { get; set; }
    }
}