using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class AdminHomeController : BaseController
    {
        private Entities db = new Entities();
        // GET: Admin/Home
        [HasPermission(Permission ="Admin")]
        public ActionResult DashboardIndex()
        {
            var list_order = db.Orders.Where(x=>x.Order_Status == 2).ToList();
            var list_customer = db.Customers.ToList();
            var list_staff = db.Staffs.ToList();
            ViewBag.list_order = list_order;
            ViewBag.list_customer = list_customer;
            ViewBag.list_staff = list_staff;
            return View();
        }
    }
}