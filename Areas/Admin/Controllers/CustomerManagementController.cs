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

        [HttpPost]
        public JsonResult Add(Customer customerObj)
        {
            Session["return_url"] = "/Admin/CustomerManagement/Add";
            if (check_auth())
            {
                Customer newCustomer = new Customer();
                newCustomer.Customer_Email = customerObj.Customer_Email;
                newCustomer.Customer_Name = customerObj.Customer_Name;
                newCustomer.Customer_Phone = customerObj.Customer_Phone;
                newCustomer.Customer_Comment = customerObj.Customer_Comment;
                db.Customers.Add(newCustomer);
                db.SaveChanges();
                return Json("/Admin/CustomerManagement/CustomerIndex", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetById(int CustomerId)
        {
            Session["return_url"] = "/Admin/CustomerManagement/GetById";
            if (check_auth())
            {
                Customer customer = db.Customers.Where(x => x.Id == CustomerId).FirstOrDefault();
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(Customer customerObj)
        {
            Session["return_url"] = "/Admin/CustomerManagement/Update";
            if (check_auth())
            {
                Customer customer = db.Customers.Where(x => x.Id == customerObj.Id).FirstOrDefault();
                customer.Customer_Name = customerObj.Customer_Name;
                customer.Customer_Phone = customerObj.Customer_Phone;
                customer.Customer_Email = customerObj.Customer_Email;
                customer.Customer_Comment = customerObj.Customer_Comment;
                db.SaveChanges();
                return Json("/Admin/CustomerManagement/CustomerIndex", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Delete(int CustomerId)
        {
            Session["return_url"] = "/Admin/CustomerManagement/Delete";
            if (check_auth())
            {
                Customer customer = db.Customers.Where(x => x.Id == CustomerId).FirstOrDefault();
                db.Customers.Remove(customer);
                db.SaveChanges();
                return Json("/Admin/CustomerManagement/CustomerIndex", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }
    }
}