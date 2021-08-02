using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class CartController : Controller
    {
        // GET: Admin/Cart
        private Entities db = new Entities();
        public ActionResult Index()
        {

            List<OrderDetail> list_ord = (List<OrderDetail>)Session["OrderDetail"];
            if (list_ord.Count > 0)
            {
                return View(list_ord);
            }
            else
            {
                return RedirectToAction("CreateOrder", "OrderManagement");
            }
        }

    }
}