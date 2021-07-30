using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class CustomerManagementController : BaseController
    {
        private Entities db = new Entities();
        
        public ActionResult CustomerIndex()
        {
            Session["return_url"] = "/Admin/CustomerManagement/CustomerIndex";
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;
                var list_customer = db.Customers.ToList();
                ViewBag.list_customer = list_customer;
                return View();
            }
            else
            {
                return Redirect("/AdminLogin");
            }
            
        }
    }
}