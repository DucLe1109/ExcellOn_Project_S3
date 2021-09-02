
using _ExcellOn_.Areas.Admin.Model;
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
    public class Customer2Controller : CustomerBaseController
    {
        private Entities db = new Entities();
        //Profile Customer
        [HttpGet]
        public ActionResult MyProfileCustomer()
        {
            Customer cus = (Customer)Session["CustomerName"];
            Customer _cus = db.Customers.Where(x => x.Id == cus.Id).FirstOrDefault();
            if (_cus == null)
            {
                return HttpNotFound();
            }
            return View(_cus);

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
                customer.Customer_Gender = CurrentCustomer.Customer_Gender;
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
            List<Order> order = db.Orders.Where(x => x.CustomerId == customer.Id).ToList();
            order.Reverse();
            return View(order);
        }
        //Order Detail Customer
        public ActionResult OrderDetailIndex(int id)
        {
            var TTOrderDetail = db.OrderDetails.Where(e => e.OrdersId == id).Select(e => e.OrdersId).ToList();
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

        [HttpPost]
        public JsonResult CreateOrder(EmployRequest employRequest)
        {
            OrderDetail_Function orderDetail_Function = new OrderDetail_Function();
            int Service_Id = employRequest.Service_Id;
            int Number_Of_Employee = employRequest.Number_Of_Employee;
            DateTime today = DateTime.Now;
            DateTime Date_Start = employRequest.Date_Start;
            DateTime Date_End = employRequest.Date_End;

            if ((today - Date_Start).TotalDays <= 0 && (Date_End - Date_Start).TotalDays > 0 && (Date_End-Date_Start).TotalDays > 0)
            {
                List<Staff> list_staff_free = orderDetail_Function.Take_List_Staff_Free(Service_Id, Date_Start, Date_End);
                if (list_staff_free.Count >= Number_Of_Employee)
                {
                    OrderDetail new_OrderDetail = new OrderDetail();
                    new_OrderDetail.OrderDetail_DateStart = Date_Start;
                    new_OrderDetail.OrderDetail_DateEnd = Date_End;
                    new_OrderDetail.OrderDetail_NumberOfPeople = Number_Of_Employee;
                    new_OrderDetail.OrderDetail_Status = 0;
                    new_OrderDetail.ServiceId = Service_Id;

                    if (Session["OrderDetail"] != null)
                    {
                        List<OrderDetail> session_orderdetail = (List<OrderDetail>)Session["OrderDetail"];
                        session_orderdetail.Add(new_OrderDetail);
                        Session["OrderDetail"] = session_orderdetail;
                    }
                    else
                    {
                        List<OrderDetail> session_orderdetail = new List<OrderDetail>();
                        session_orderdetail.Add(new_OrderDetail);
                        Session["OrderDetail"] = session_orderdetail;
                    }
                    return Json("Create successfully !", JsonRequestBehavior.AllowGet);
                }
                return Json("The Employee is greater than our Staff", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(404, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpGet]
        public JsonResult SubmitOrder(float total_cost, string Description, string Company_Name, string Company_Phone, string Company_Email, string Company_Address)
        {
            OrderDetail_Function orderDetail_Function = new OrderDetail_Function();
            Customer cus = (Customer)Session["CustomerName"];
            Customer _cus = db.Customers.Where(x => x.Id == cus.Id).FirstOrDefault();
            Company company = db.Companies.Where(x=>x.Company_Email == Company_Email).FirstOrDefault();
            if (company == null)
            {
                Company _new = new Company();
                _new.Company_Address = Company_Address;
                _new.Company_Name = Company_Name;
                _new.Company_Phone = Company_Phone;
                _new.Company_Email = Company_Email;
                _cus.Company = _new;
                db.Companies.Add(_new);
            }
            else
            {
                _cus.Company = company;
            }
    
            List<OrderDetail> list_ord = (List<OrderDetail>)Session["OrderDetail"];

            if (list_ord != null)
            {
                Order new_Order = new Order();
                new_Order.Order_DateCreate = DateTime.Now.ToString("MMMM/dd/yyyy");
                new_Order.Order_Description = Description;
                new_Order.Order_TotalCost = total_cost;
                new_Order.Order_Status = 0;
                new_Order.CustomerId = _cus.Id;
                db.Orders.Add(new_Order);
                db.SaveChanges();

                if (list_ord != null)
                {
                    foreach (var item in list_ord)
                    {
                        int Service_Id = (int)item.ServiceId;
                        int Number_Of_Employee = (int)item.OrderDetail_NumberOfPeople;
                        DateTime Date_Start = (DateTime)item.OrderDetail_DateStart;
                        DateTime Date_End = (DateTime)item.OrderDetail_DateEnd;
                        List<Staff> list_staff_free = orderDetail_Function.Take_List_Staff_Free(Service_Id, Date_Start, Date_End);

                        if (list_staff_free.Count >= Number_Of_Employee)
                        {
                            OrderDetail new_OrderDetail = new OrderDetail();
                            new_OrderDetail.OrderDetail_DateStart = Date_Start;
                            new_OrderDetail.OrderDetail_DateEnd = Date_End;
                            new_OrderDetail.OrderDetail_NumberOfPeople = Number_Of_Employee;
                            new_OrderDetail.OrderDetail_Status = 0;
                            new_OrderDetail.ServiceId = Service_Id;
                            new_OrderDetail.OrdersId = new_Order.Id;
                            db.OrderDetails.Add(new_OrderDetail);
                            int temp = 0;
                            foreach (var item2 in list_staff_free)
                            {
                                Staff_OrderDetail new_Staff_OrderDetail = new Staff_OrderDetail();
                                new_Staff_OrderDetail.OrderDetail_Id = new_OrderDetail.Id;
                                new_Staff_OrderDetail.Staff_Id = item2.Id;
                                new_Staff_OrderDetail.Date_Start = Date_Start;
                                new_Staff_OrderDetail.Date_End = Date_End;
                                db.Staff_OrderDetail.Add(new_Staff_OrderDetail);
                                temp += 1;
                                if (temp == Number_Of_Employee)
                                {
                                    break;
                                }
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            return Json("The Employee is greater than our Staff", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                Session["OrderDetail"] = null;
                return Json("Create Order Successfully", JsonRequestBehavior.AllowGet);
            }
            
            return Json("You don't have any order !", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Update_OrderDetail(OrderDetail ord, int service_id)
        {
            DateTime today = DateTime.Now;
            DateTime Date_Start = (DateTime)ord.OrderDetail_DateStart;
            DateTime Date_End = (DateTime)ord.OrderDetail_DateEnd;
            int Num = (int)ord.OrderDetail_NumberOfPeople;

            if ((today - Date_Start).TotalDays <= 0 && (Date_End - Date_Start).TotalDays > 0 && (Date_End - Date_Start).TotalDays > 0 && Num > 0) {
                OrderDetail_Function orderDetail_Function = new OrderDetail_Function();
                OrderDetail ord_for_edit = db.OrderDetails.Where(x => x.Id == ord.Id).FirstOrDefault();
                // Th1: Update các thông tin dịch vụ cũ.
                if (ord_for_edit.Service.Id == service_id)
                {

                    OrderDetail ord_old = db.OrderDetails.Where(x => x.Id == ord.Id).FirstOrDefault();
                    if (ord_old != null)
                    {
                        DateTime date_start = (DateTime)ord.OrderDetail_DateStart;
                        DateTime date_end = (DateTime)ord.OrderDetail_DateEnd;

                        // TH1: update đơn hàng chỉ thay đổi số lượng người thuê mà không thay đổi ngày bắt đầu - kết thúc nên có thể lấy đếm cả số lượng nhân viên đã gán cho đơn hàng trước đấy
                        if ((int)((DateTime)ord_old.OrderDetail_DateStart - date_start).TotalDays == 0 && (int)((DateTime)ord_old.OrderDetail_DateEnd - date_end).TotalDays == 0)
                        {
                            List<Staff_OrderDetail> staff_OrderDetails = db.Staff_OrderDetail.Where(x => x.OrderDetail_Id == ord_old.Id).ToList();
                            List<Staff> list_staff_free_before_edit = orderDetail_Function.Take_List_Staff_Free(ord_old.Service.Id, date_start, date_end);
                            // Nếu số nhân viên đã được gán cho đơn hàng cũ + số nhân viên còn rảnh cho đơn hàng mới > số nhân viên update thì có thể update đơn hàng
                            // Phải xóa các nhân viên cũ đã được gán cho đơn hàng cũ và tạo mới lại
                            if ((list_staff_free_before_edit.Count + staff_OrderDetails.Count) >= ord.OrderDetail_NumberOfPeople)
                            {
                                foreach (var item in staff_OrderDetails)
                                {
                                    db.Staff_OrderDetail.Remove(item);
                                }
                            }
                            db.SaveChanges();
                            List<Staff> list_staff_free_for_update = orderDetail_Function.Take_List_Staff_Free(ord_old.Service.Id, date_start, date_end);
                            if (list_staff_free_for_update.Count >= ord.OrderDetail_NumberOfPeople)
                            {
                                OrderDetail _new_ord = new OrderDetail();
                                _new_ord.OrderDetail_DateStart = date_start;
                                _new_ord.OrderDetail_DateEnd = date_end;
                                _new_ord.OrderDetail_Status = 0;
                                _new_ord.OrderDetail_NumberOfPeople = ord.OrderDetail_NumberOfPeople;
                                _new_ord.Service = ord_old.Service;
                                _new_ord.Order = ord_old.Order;

                                Order order = ord_old.Order;
                                order.Order_TotalCost = order.Order_TotalCost - (ord_old.Service.Service_Price * ord_old.OrderDetail_NumberOfPeople * (int)((DateTime)ord_old.OrderDetail_DateEnd - (DateTime)ord_old.OrderDetail_DateStart).TotalDays);
                                order.Order_TotalCost = order.Order_TotalCost + (_new_ord.Service.Service_Price * _new_ord.OrderDetail_NumberOfPeople * (int)((DateTime)_new_ord.OrderDetail_DateEnd - (DateTime)_new_ord.OrderDetail_DateStart).TotalDays);
                                db.OrderDetails.Add(_new_ord);
                                db.OrderDetails.Remove(ord_old);
                                int count = 0;
                                foreach (var item in list_staff_free_for_update)
                                {
                                    Staff_OrderDetail staff_OrderDetail = new Staff_OrderDetail();
                                    staff_OrderDetail.Date_Start = date_start;
                                    staff_OrderDetail.Date_End = date_end;
                                    staff_OrderDetail.OrderDetail_Id = _new_ord.Id;
                                    staff_OrderDetail.Staff_Id = item.Id;
                                    db.Staff_OrderDetail.Add(staff_OrderDetail);
                                    count += 1;
                                    if (count == ord.OrderDetail_NumberOfPeople)
                                    {
                                        break;
                                    }
                                }
                                db.SaveChanges();
                            }
                            else
                            {
                                return Json("The Staff is not sufficient for this order !", JsonRequestBehavior.AllowGet);
                            }
                        }
                        // TH2: Update đơn hàng thay đổi ngày bắt đầu và kết thúc nên không thể tính cả các nhân viên đã được gán cho đơn hàng trước đấy
                        else
                        {
                            List<Staff> list_staff_free_for_update = orderDetail_Function.Take_List_Staff_Free(ord_old.Service.Id, date_start, date_end);
                            if (list_staff_free_for_update.Count >= ord.OrderDetail_NumberOfPeople)
                            {
                                List<Staff_OrderDetail> staff_OrderDetails_old = db.Staff_OrderDetail.Where(x => x.OrderDetail_Id == ord.Id).ToList();
                                foreach (var item in staff_OrderDetails_old)
                                {
                                    db.Staff_OrderDetail.Remove(item);
                                }
                                OrderDetail _new_ord = new OrderDetail();
                                _new_ord.OrderDetail_DateStart = date_start;
                                _new_ord.OrderDetail_DateEnd = date_end;
                                _new_ord.OrderDetail_Status = 0;
                                _new_ord.OrderDetail_NumberOfPeople = ord.OrderDetail_NumberOfPeople;
                                _new_ord.Service = ord_old.Service;
                                _new_ord.Order = ord_old.Order;

                                Order order = ord_old.Order;
                                order.Order_TotalCost = order.Order_TotalCost - (ord_old.Service.Service_Price * ord_old.OrderDetail_NumberOfPeople * (int)((DateTime)ord_old.OrderDetail_DateEnd - (DateTime)ord_old.OrderDetail_DateStart).TotalDays);
                                order.Order_TotalCost = order.Order_TotalCost + (_new_ord.Service.Service_Price * _new_ord.OrderDetail_NumberOfPeople * (int)((DateTime)_new_ord.OrderDetail_DateEnd - (DateTime)_new_ord.OrderDetail_DateStart).TotalDays);
                                db.OrderDetails.Add(_new_ord);
                                db.OrderDetails.Remove(ord_old);
                                int count = 0;
                                foreach (var item in list_staff_free_for_update)
                                {
                                    Staff_OrderDetail staff_OrderDetail = new Staff_OrderDetail();
                                    staff_OrderDetail.Date_Start = date_start;
                                    staff_OrderDetail.Date_End = date_start;
                                    staff_OrderDetail.OrderDetail_Id = _new_ord.Id;
                                    staff_OrderDetail.Staff_Id = item.Id;
                                    db.Staff_OrderDetail.Add(staff_OrderDetail);
                                    count += 1;
                                    if (count == ord.OrderDetail_NumberOfPeople)
                                    {
                                        break;
                                    }
                                }
                                db.SaveChanges();
                            }
                            else
                            {
                                return Json("The Staff is not sufficient for this order !", JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        return Json(404, JsonRequestBehavior.AllowGet);
                    }
                    return Json("Update successfully", JsonRequestBehavior.AllowGet);
                }
                // Th2: Update thành dịch vụ mới
                else
                {
                    DateTime date_start = (DateTime)ord.OrderDetail_DateStart;
                    DateTime date_end = (DateTime)ord.OrderDetail_DateEnd;
                    int NumberOfPeople = (int)ord.OrderDetail_NumberOfPeople;
                    List<Staff> list_staff_free = orderDetail_Function.Take_List_Staff_Free(service_id, date_start, date_end);
                    if (list_staff_free.Count >= NumberOfPeople)
                    {
                        List<Staff_OrderDetail> staff_OrderDetails = db.Staff_OrderDetail.Where(x => x.OrderDetail_Id == ord.Id).ToList();
                        foreach (var item in staff_OrderDetails)
                        {
                            db.Staff_OrderDetail.Remove(item);
                        }
                        OrderDetail ord_old = db.OrderDetails.Where(x => x.Id == ord.Id).FirstOrDefault();
                        Order order = ord_old.Order;
                        order.Order_TotalCost = order.Order_TotalCost - (ord_old.Service.Service_Price * ord_old.OrderDetail_NumberOfPeople * (int)((DateTime)ord_old.OrderDetail_DateEnd - (DateTime)ord_old.OrderDetail_DateStart).TotalDays);
                        db.OrderDetails.Remove(ord_old);

                        OrderDetail new_ord = new OrderDetail();
                        new_ord.OrderDetail_Status = 0;
                        new_ord.OrderDetail_DateStart = date_start;
                        new_ord.OrderDetail_DateEnd = date_end;
                        new_ord.OrderDetail_NumberOfPeople = NumberOfPeople;
                        new_ord.Order = order;
                        Service service = db.Services.Where(x => x.Id == service_id).FirstOrDefault();
                        new_ord.Service = service;
                        db.OrderDetails.Add(new_ord);
                        order.Order_TotalCost = order.Order_TotalCost + (new_ord.Service.Service_Price * new_ord.OrderDetail_NumberOfPeople * (int)((DateTime)new_ord.OrderDetail_DateEnd - (DateTime)new_ord.OrderDetail_DateStart).TotalDays);
                        int count = 0;
                        foreach (var item in list_staff_free)
                        {
                            Staff_OrderDetail staff_OrderDetail = new Staff_OrderDetail();
                            staff_OrderDetail.Date_Start = date_start;
                            staff_OrderDetail.Date_End = date_end;
                            staff_OrderDetail.OrderDetail_Id = new_ord.Id;
                            staff_OrderDetail.Staff_Id = item.Id;
                            db.Staff_OrderDetail.Add(staff_OrderDetail);
                            count += 1;
                            if (count == ord.OrderDetail_NumberOfPeople)
                            {
                                break;
                            }
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        return Json("The Staff is not sufficient for this order !", JsonRequestBehavior.AllowGet);
                    }
                    return Json("Update successfully", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(404, JsonRequestBehavior.AllowGet);

        }
        public ActionResult OrderIndex()
        {
            List<OrderDetail> list_ord = (List<OrderDetail>)Session["OrderDetail"];

            return View(list_ord);

        }

        [HttpGet]
        public JsonResult RemoveItem(int OrderDetail_ID)
        {

            List<OrderDetail> list_ord = (List<OrderDetail>)Session["OrderDetail"];
            OrderDetail orderDetail = list_ord.Where(x => x.Id == OrderDetail_ID).FirstOrDefault();
            Service service = db.Services.Where(x => x.Id == orderDetail.ServiceId).FirstOrDefault();
            int total_day = (int)((DateTime)orderDetail.OrderDetail_DateEnd - (DateTime)orderDetail.OrderDetail_DateStart).TotalDays;
            float cost_remove = (float)(orderDetail.OrderDetail_NumberOfPeople * total_day * service.Service_Price);
            if (orderDetail != null)
            {
                list_ord.Remove(orderDetail);
            }
            Session["OrderDetail"] = list_ord;
            return Json(cost_remove, JsonRequestBehavior.AllowGet);

        }

        public ActionResult PaymentWithPaypal()
        {
            return View();
        }

        public JsonResult PayPal(Banking request)
        {
            if (request != null)
            {
                Banking new_banking = new Banking();
                new_banking.Status = 0;
                new_banking.Deleted = 0;
                Customer _cus = db.Customers.Where(x => x.Id == request.CustomerId).FirstOrDefault();
                if (_cus != null)
                {
                    new_banking.Customer = _cus;
                }
                Order or = db.Orders.Where(x => x.Id == request.OrderId).FirstOrDefault();
                List<OrderDetail> ord = db.OrderDetails.Where(x => x.OrdersId == or.Id).ToList();
                if (or != null)
                {
                    or.Order_Status = 99; // Status Ready
                    if (ord != null)
                    {
                        foreach (var item in ord) {
                            item.OrderDetail_Status = 99;
                        }
                    }
                    new_banking.Order = or;
                }
                new_banking.Money = request.Money;
                new_banking.DateCreate = DateTime.Now;
                db.Bankings.Add(new_banking);
                db.SaveChanges();
                return Json("Update successfully", JsonRequestBehavior.AllowGet);
            }
            return Json(404, JsonRequestBehavior.AllowGet);
        }
    }
}