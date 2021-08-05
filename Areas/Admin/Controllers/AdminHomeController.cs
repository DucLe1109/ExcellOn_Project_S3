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
            return View();
        }
    }
}