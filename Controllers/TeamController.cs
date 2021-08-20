using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class TeamController : Controller
    {
        Entities db = new Entities();
        // GET: Team
        public ActionResult Section_Team()
        {
            UserInFo admin = db.UserInFoes.Where(x => x.User_Name == "huyduc").FirstOrDefault();
            ViewBag.admin = admin;

            UserInFo CEO = db.UserInFoes.Where(x => x.User_Name == "CEO").FirstOrDefault();
            ViewBag.CEO = CEO;

            UserInFo Manager = db.UserInFoes.Where(x => x.User_Name == "Manager").FirstOrDefault();
            ViewBag.Manager = Manager;

            return PartialView();
        }
    }
}