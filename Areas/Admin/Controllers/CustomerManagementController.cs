using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Models;
using System.Linq;
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
            return Json(customer, JsonRequestBehavior.AllowGet);


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
    }
}