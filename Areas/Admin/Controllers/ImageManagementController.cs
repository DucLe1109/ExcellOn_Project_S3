using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class ImageManagementController : BaseController
    {
        private Entities db = new Entities();
        public ActionResult ImageIndex()
        {
            Session["return_url"] = "/Admin/ImageManagement/ImageIndex";
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;
                var list = db.Images.ToList();
                return View(db.Images.ToList());
            }
            else
            {
                return Redirect("/AdminLogin");
            }
        }
    }
}