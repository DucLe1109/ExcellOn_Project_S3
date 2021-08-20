using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Section_Blog()
        {
            return PartialView();
        }
    }
}