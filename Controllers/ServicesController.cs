using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _ExcellOn_.Models;

namespace _ExcellOn_.Controllers
{
    public class ServicesController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Section_Service()
        {
            return PartialView(db.Services.ToList());
        }
    }
}
