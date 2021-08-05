using _ExcellOn_.Areas.Admin.Model;
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
        [HasPermission(Permission = "Image_List")]
        public ActionResult ImageIndex()
        {
            var list = db.Images.ToList();
            return View(db.Images.ToList());

        }
    }
}