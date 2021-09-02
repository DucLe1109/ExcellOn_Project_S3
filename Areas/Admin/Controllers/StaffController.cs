using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class StaffController : Controller
    {
        Entities db = new Entities();

        [HttpGet]
        public ActionResult Login()
        {
            Session["StaffName"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel.UserName != null && loginViewModel.Password != null)
            {
                Staff staff = db.Staffs.Where(x => x.Staff_UserName == loginViewModel.UserName).FirstOrDefault();
                if (staff != null)
                {
                    bool test = BCrypt.Net.BCrypt.Verify(loginViewModel.Password, staff.Staff_Password);
                    if (test)
                    {
                        Session["StaffName"] = staff;

                        return Redirect("/Admin/Staff2/MyProfile");
                    }
                    else
                    {
                        ViewBag.message = "Account or Password is not correct !";
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "Account or Password is not correct !";
                    return View();
                }
            }
            else
            {
                ViewBag.message = "Account or Password is null !";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/StaffLogin");
        }

        
    }
}