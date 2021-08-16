using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class CustomerManagementController : BaseController
    {
        private Entities db = new Entities();


        [HasPermission(Permission = "Customer_List")]
        public ActionResult CustomerIndex()
        {
            var list_customer = db.Customers.ToList();
            ViewBag.list_customer = list_customer;
            return View();
        }
        
        [HasPermission(Permission = "Customer_List")]
        public ActionResult CustomerInOrderIndex()
        {
            List<Order> list_order = db.Orders.Where(x => x.Order_Status != 3).ToList();
            List<Customer> list_customer = new List<Customer>();
            if (list_order != null)
            {
                foreach (var item in list_order)
                {
                    if (item.Customer != null)
                    {
                        list_customer.Add(item.Customer);
                    }
                }
            }
            
            ViewBag.list_customer = list_customer;
            return View();
        }

        [HasPermission(Permission = "Customer_Add")]
        [HttpPost]
        public JsonResult Add(Customer customerObj)
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

        [HasPermission(Permission = "Customer_Edit")]
        [HttpGet]
        public JsonResult GetById(int CustomerId)
        {
            Customer customer = db.Customers.Where(x => x.Id == CustomerId).FirstOrDefault();
            CustomerViewModel customerView = new CustomerViewModel();
            customerView.Id = customer.Id;
            customerView.Customer_UserName = customer.Customer_UserName;
            customerView.Customer_Password = customer.Customer_Password;
            customerView.Customer_Phone = customer.Customer_Phone;
            customerView.Customer_Name = customer.Customer_Name;
            customerView.Customer_Email = customer.Customer_Email;
            customerView.Customer_Comment = customer.Customer_Comment;
            customerView.Customer_Avatar = customer.Customer_Avatar;
            customerView.CompanyId = customer.CompanyId;
            return Json(customerView, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Customer_Eit")]
        [HttpPost]
        public JsonResult Update(Customer customerObj)
        {

            Customer customer = db.Customers.Where(x => x.Id == customerObj.Id).FirstOrDefault();
            customer.Customer_Name = customerObj.Customer_Name;
            customer.Customer_Phone = customerObj.Customer_Phone;
            customer.Customer_Email = customerObj.Customer_Email;
            customer.Customer_Comment = customerObj.Customer_Comment;
            db.SaveChanges();
            return Json("/Admin/CustomerManagement/CustomerIndex", JsonRequestBehavior.AllowGet);

        }

        [HasPermission(Permission = "Customer_Delete")]
        [HttpGet]
        public JsonResult Delete(int CustomerId)
        {
            Customer customer = db.Customers.Where(x => x.Id == CustomerId).FirstOrDefault();
            db.Customers.Remove(customer);
            db.SaveChanges();
            return Json("/Admin/CustomerManagement/CustomerIndex", JsonRequestBehavior.AllowGet);

        }
        
        [HasPermission(Permission = "Customer_List")]
        [HttpPost]
        public ActionResult SendEmail(SendEmailModel model)
        {
            using (MailMessage m = new MailMessage())
            {
                m.From = new MailAddress("huyducle1109@gmail.com", model.From);
                m.To.Add(model.To);
                m.Subject = model.Subject;
                m.Body = model.Body;
                m.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("huyducle1109@gmail.com", "huyduc123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(m);

                
            };
            TempData["message"] = "Send email successfully!";
            return RedirectToAction("CustomerIndex");
        }
    }
}