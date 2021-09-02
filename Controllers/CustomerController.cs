using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using _ExcellOn_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class CustomerController : Controller
    {
        private Entities db = new Entities();

        // Function SignUp
        [HttpPost]
        public JsonResult SignUp(SignUpViewModel modelSignUp)
        {
            if (modelSignUp.UserName != null && modelSignUp.Email != null && modelSignUp.Password != null)
            {
                var User = db.Customers.Where(x => x.Customer_UserName == modelSignUp.UserName || x.Customer_Email == modelSignUp.Email).FirstOrDefault();
                if (User != null)
                {
                    string message = "Username or Email is available !";
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Customer newCustomer = new Customer();
                    newCustomer.Customer_UserName = modelSignUp.UserName;
                    newCustomer.Customer_Email = modelSignUp.Email;
                    newCustomer.Customer_Phone = modelSignUp.Telephone;
                    newCustomer.Customer_Name = modelSignUp.FullName;
                    newCustomer.Customer_Gender = modelSignUp.Gender;
                    if (modelSignUp.Gender == 1)
                    {
                        newCustomer.Customer_Avatar = "/Public/Image/a19.jfif";
                    }
                    else
                    {
                        newCustomer.Customer_Avatar = "/Public/Image/a17.jfif";
                    }
                    newCustomer.Customer_Password = BCrypt.Net.BCrypt.HashPassword(modelSignUp.Password);

                    newCustomer.Customer_Email = modelSignUp.Email;
                    db.Customers.Add(newCustomer);
                    db.SaveChanges();
                    string message = "Create Successfully !";
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string message = "Create not successfully !";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        // Function Login View
        [HttpGet]
        public ActionResult Login()
        {
            Session["CustomerName"] = null;
            return View("~/Views/Customer/Login.cshtml");
        }

        // Function Login
        [HttpPost]
        public ActionResult Login(SignInViewModel modelSignIn)
        {
            if (modelSignIn.UserName != null && modelSignIn.Password != null)
            {
                 Customer cus = db.Customers.Where(x => x.Customer_UserName == modelSignIn.UserName).FirstOrDefault();
                if (cus != null)
                {
                    bool test = BCrypt.Net.BCrypt.Verify(modelSignIn.Password, cus.Customer_Password);
                    if (test)
                    {

                        Session["CustomerName"] = cus;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.message = "Account or Password is not correct !";
                        return View("~/Views/Customer/Login.cshtml");
                    }
                }
                else
                {
                    ViewBag.message = "Account or Password is not correct !";
                    return View("~/Views/Customer/Login.cshtml");
                }
            }
            else
            {
                ViewBag.message = "Account or Password is null !";
                return View("~/Views/Customer/Login.cshtml");
            }
        }

        [HttpPost]
        public JsonResult LoginAjax(SignInViewModel modelSignIn)
        {
            if (modelSignIn.UserName != null && modelSignIn.Password != null)
            {
                Customer cus = db.Customers.Where(x => x.Customer_UserName == modelSignIn.UserName).FirstOrDefault();
                if (cus != null)
                {
                    bool test = BCrypt.Net.BCrypt.Verify(modelSignIn.Password, cus.Customer_Password);
                    if (test)
                    {

                        Session["CustomerName"] = cus;

                        return Json("Successfull",JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Account or Password is not correct !", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Account or Password is not correct !", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Account or Password is not correct !", JsonRequestBehavior.AllowGet);
            }
        }
        // Function Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Section_Customer()
        {
            return PartialView(db.Customers.ToList());
        }
    }
}