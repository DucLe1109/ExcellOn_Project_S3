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
            var list_staff = db.Staffs.ToList();
            var list_orderdetail = db.OrderDetails.ToList();
            var list_customer = db.Customers.ToList();
            var list_service = db.Services.ToList();
            var myCompany = db.MyCompanies.FirstOrDefault();
            ViewBag.myCompany = myCompany;
            ViewBag.list_service = list_service;
            ViewBag.list_staff = list_staff;
            ViewBag.list_orderdetail = list_orderdetail;
            ViewBag.list_customer = list_customer;
            return View();
        }
    }
}