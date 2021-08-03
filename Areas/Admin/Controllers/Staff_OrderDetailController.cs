using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class Staff_OrderDetailController : BaseController
    {
        private Entities db = new Entities();
        // GET: Admin/Staff_OrderDetail
        public ActionResult Staff_OrderDetailIndex()
        {
            Session["return_url"] = "/Admin/Staff_OrderDetail/Staff_OrderDetailIndex";
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;

                var list_staff_orderdetail = db.Staff_OrderDetail.ToList();
                return View(list_staff_orderdetail);
            }
            else
            {
                return Redirect("/AdminLogin");
            }
        }
    }
}