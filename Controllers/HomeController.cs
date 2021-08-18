using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Main()
        {
            return PartialView("Main");
        }
        public ActionResult InfoNumber()
        {
            var list_order = db.Orders.Where(x => x.Order_Status == 2).ToList();
            var list_customer = db.Customers.ToList();
            var list_staff = db.Staffs.ToList();
            ViewBag.list_order = list_order;
            ViewBag.list_customer = list_customer;
            ViewBag.list_staff = list_staff;
            return View();
        }

    }
}