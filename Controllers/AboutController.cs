using _ExcellOn_.Models;
using System.Linq;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class AboutController : Controller
    {
        private Entities db = new Entities();
        // GET: About
        public ActionResult Index()
        {
            return View(db.MyCompanies.ToList());
        }
        public ActionResult HomeAbout()
        {
            return View(db.MyCompanies.ToList());
        }
    }
}