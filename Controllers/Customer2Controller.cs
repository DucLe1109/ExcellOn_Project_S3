
using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class Customer2Controller : CustomerBaseController
    {
        private Entities db = new Entities();
        //Profile Customer
        [HttpGet]
        public ActionResult MyProfileCustomer()
        {
            Customer cus = (Customer)Session["CustomerName"];
            if (cus == null)
            {
                return HttpNotFound();
            }
            return View(cus);

        }
        //Function Update information for Customer
        [HttpPost]
        public ActionResult Update_ProfileCustomer(Customer CurrentCustomer, HttpPostedFileBase AvatarUpload)
        {
            Customer customer = db.Customers.FirstOrDefault(x => x.Id == CurrentCustomer.Id);
            if (customer != null)
            {
                customer.Customer_Email = CurrentCustomer.Customer_Email;
                customer.Customer_Name = CurrentCustomer.Customer_Name;
                if (CurrentCustomer.Customer_Password != null)
                {
                    customer.Customer_Password = BCrypt.Net.BCrypt.HashPassword(CurrentCustomer.Customer_Password);
                }
                else
                {
                    customer.Customer_Password = customer.Customer_Password;
                }

                customer.Customer_Phone = CurrentCustomer.Customer_Phone;
                customer.Customer_UserName = CurrentCustomer.Customer_UserName;
                customer.Customer_Comment = CurrentCustomer.Customer_Comment;
                if (AvatarUpload != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(AvatarUpload.FileName);
                    string Extension = Path.GetExtension(AvatarUpload.FileName);
                    FileName = FileName + Extension;
                    customer.Customer_Avatar = "/Public/Image/" + FileName;

                    //string fullPath = Request.MapPath("~/Public/Image/" + FileName);
                    //if (System.IO.File.Exists(fullPath))
                    //{
                    //    System.IO.File.Delete(fullPath);
                    //}

                    AvatarUpload.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
                }
                else
                {
                    customer.Customer_Avatar = customer.Customer_Avatar;
                }

                db.SaveChanges();
            }
            return RedirectToAction("MyProfileCustomer", new { id = customer.Id });
        }
        //OrderList Customer
        public ActionResult OrderListIndex()
        {
            Customer customer = (Customer)Session["CustomerName"];
            List<Order> order = db.Orders.Where(x => x.CustomerId == customer.Id).OrderBy(x=>x.Order_Status).ToList();
            return View(order);
        }
        //Order Detail Customer
        public ActionResult OrderDetailIndex(string id)
        {
            int id_1 = int.Parse(id);
            var TTOrderDetail = db.OrderDetails.Where(e => e.OrdersId == id_1).Select(e => e.OrdersId).ToList();
            List<Order_OrderDetail> list_order_orderdetail = new List<Order_OrderDetail>();

            List<Order> list_or = db.Orders.Where(x => x.Order_Status != 3 && TTOrderDetail.Contains(x.Id)).OrderByDescending(x => x.Order_DateCreate).ToList();
            foreach (var item in list_or)
            {
                Order_OrderDetail _new = new Order_OrderDetail();
                _new.Orders = item;
                List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id && x.OrderDetail_Status != 3).ToList();
                if (list_ord != null)
                {
                    _new.List_OrderDetail = list_ord;
                }
                list_order_orderdetail.Add(_new);
            }
            List<int> List_Order_Status_Id = new List<int>();
            List_Order_Status_Id.Add(0);
            List_Order_Status_Id.Add(1);
            List_Order_Status_Id.Add(2);
            ViewBag.list_order_orderdetail = list_order_orderdetail;
            ViewBag.List_Order_Status_Id = List_Order_Status_Id;
            return View();
        }
    }
}