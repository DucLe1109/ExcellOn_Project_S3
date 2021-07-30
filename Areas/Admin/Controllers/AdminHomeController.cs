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
        public ActionResult DashboardIndex()
        {
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;
                return View();
            }else
            {
                return Redirect("/AdminLogin");
            }
        }
    }
}