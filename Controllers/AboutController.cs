using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class AboutController : Controller
    {
        Entities db = new Entities();
        // GET: About
        public ActionResult Index()
        {
            return View(db.MyCompanies.FirstOrDefault());
        }
    }
}