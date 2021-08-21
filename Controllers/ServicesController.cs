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
        
        public ActionResult Detail(int id)
        {
            return View(db.Services.Find(id));
        }

        public ActionResult Section_Service()
        {
            return PartialView(db.Services.ToList());
        }

        public ActionResult Section_Image(int id)
        {
            return PartialView(db.Images.Where(x => x.ServiceId == id).ToList());
        }

        public ActionResult Section_Staff(int id)
        {
            return PartialView(db.Staffs.Where(x => x.ServiceId == id).ToList());
        }
    }
}
