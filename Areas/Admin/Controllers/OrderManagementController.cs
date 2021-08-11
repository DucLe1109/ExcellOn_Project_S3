using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using _ExcellOn_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class OrderManagementController : BaseController
    {
        private Entities db = new Entities();
        private OrderDetail_Function orderDetail_Function = new OrderDetail_Function();

        [HasPermission(Permission = "Order_List")]
        public ActionResult OrderIndex()
        {
            //List<Order> list_or = db.Orders.Where(x=>x.Order_Status != 3).ToList();
            //return View(list_or);
            List<Order_OrderDetail> list_order_orderdetail = new List<Order_OrderDetail>();

            List<Order> list_or = db.Orders.Where(x => x.Order_Status != 3).OrderByDescending(x=>x.Order_DateCreate).ToList();
            foreach (var item in list_or)
            {
                Order_OrderDetail _new = new Order_OrderDetail();
                _new.Orders = item;
                List<OrderDetail> list_ord = db.OrderDetails.Where(x=>x.OrdersId == item.Id && x.OrderDetail_Status != 3).ToList();
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

        public ActionResult Filter(OrderFilterViewModel request)
        {
            List<Order_OrderDetail> list_order_orderdetail = new List<Order_OrderDetail>();
            List<Order> list_or = new List<Order>();
            List<int> List_Order_Status_Id = request.List_Order_Status_Id;
            if (List_Order_Status_Id != null && List_Order_Status_Id.Count > 0)
            {
                foreach (var Id in List_Order_Status_Id)
                {
                    List<Order> list_order = db.Orders.Where(x => x.Order_Status == Id).OrderByDescending(x=>x.Order_DateCreate).ToList();
                    if (list_order != null)
                    {
                        foreach (var order in list_order)
                        {
                            list_or.Add(order);
                        }
                    }
                }
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
                ViewBag.list_order_orderdetail = list_order_orderdetail;
                ViewBag.List_Order_Status_Id = List_Order_Status_Id;
                return View("/Areas/Admin/Views/OrderManagement/OrderIndex.cshtml");
            }
            else
            {
                List<Order_OrderDetail> list_order_orderdetail2 = new List<Order_OrderDetail>();

                List<Order> list_or2 = db.Orders.Where(x => x.Order_Status != 3).OrderByDescending(x => x.Order_DateCreate).ToList();
                foreach (var item in list_or2)
                {
                    Order_OrderDetail _new = new Order_OrderDetail();
                    _new.Orders = item;
                    List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id && x.OrderDetail_Status != 3).ToList();
                    if (list_ord != null)
                    {
                        _new.List_OrderDetail = list_ord;
                    }
                    list_order_orderdetail2.Add(_new);
                }
                
                ViewBag.list_order_orderdetail = list_order_orderdetail2;
                ViewBag.List_Order_Status_Id = List_Order_Status_Id;
                return View("/Areas/Admin/Views/OrderManagement/OrderIndex.cshtml");
            }
        }

        [HttpPost]
        public JsonResult CreateOrder(EmployRequest employRequest)
        {
            int Service_Id = employRequest.Service_Id;
            int Number_Of_Employee = employRequest.Number_Of_Employee;
            DateTime Date_Start = employRequest.Date_Start;
            DateTime Date_End = employRequest.Date_End;

            // Khởi tạo bảng Staff_OrderDetail. (Nếu chưa có bất kì đơn hàng chi tiết nào thì lấy luôn số lượng staff cần được thuê trong bảng Staff và khởi tạo đơn hàng chi tiết mới)
            List<Staff> list_Staff_free = db.Staffs.Where(x => x.Staff_OrderDetail.Count == 0 && x.ServiceId == Service_Id).ToList();
            if (list_Staff_free.Count > 0)
            {
                if (list_Staff_free.Count >= Number_Of_Employee)
                {
                    OrderDetail new_OrderDetail = new OrderDetail();
                    new_OrderDetail.OrderDetail_DateStart = Date_Start;
                    new_OrderDetail.OrderDetail_DateEnd = Date_End;
                    new_OrderDetail.OrderDetail_NumberOfPeople = Number_Of_Employee;
                    new_OrderDetail.OrderDetail_Status = 0;
                    new_OrderDetail.ServiceId = Service_Id;
                    db.OrderDetails.Add(new_OrderDetail);
                    int temp = 0;
                    foreach (var item in list_Staff_free)
                    {
                        Staff_OrderDetail new_Staff_OrderDetail = new Staff_OrderDetail();
                        new_Staff_OrderDetail.OrderDetail_Id = new_OrderDetail.Id;
                        new_Staff_OrderDetail.Staff_Id = item.Id;
                        new_Staff_OrderDetail.Date_Start = Date_Start;
                        new_Staff_OrderDetail.Date_End = Date_End;
                        db.Staff_OrderDetail.Add(new_Staff_OrderDetail);
                        temp += 1;
                        if (temp == Number_Of_Employee)
                        {
                            break;
                        }
                    }
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

                    return Json("/Admin/OrderManagement/Index", JsonRequestBehavior.AllowGet);
                    //db.SaveChanges();
                }
                else
                {
                    return Json("The Employee is greater than our Staff", JsonRequestBehavior.AllowGet);
                }
            }
            // Nếu tất cả các Staff đều đã tham gia tối thiểu vào 1 OrderDetail thì phải xử lý trùng lặp ngày tháng trong này.
            else
            {
                var list_Staff_OrderDetail = db.Staff_OrderDetail.ToList();
                List<Staff> _list_Staff_free = new List<Staff>(); // Danh sách những Staff không bị trùng lịch với đơn đặt hàng mới 
                List<Staff> _list_Staff_Not_free = new List<Staff>(); //  Danh sách những Staff bị trùng lịch với đơn đặt hàng mới 
                foreach (var item in list_Staff_OrderDetail)
                {
                    if ((((Date_Start - (DateTime)item.Date_End).TotalDays > 0) && ((Date_Start - (DateTime)item.Date_Start).TotalDays > 0)) || (((Date_End - (DateTime)item.Date_Start).TotalDays < 0) && ((Date_End - (DateTime)item.Date_Start).TotalDays < 0)))
                    {
                        Staff free_staff = db.Staffs.Where(x => x.Id == item.Staff_Id).FirstOrDefault();
                        if (_list_Staff_free.Exists(x => x.Id == free_staff.Id))
                        {
                            // không add những staff đã lặp lại
                        }
                        else
                        {
                            _list_Staff_free.Add(free_staff);
                        }

                    }
                    else
                    {
                        Staff free_staff = db.Staffs.Where(x => x.Id == item.Staff_Id).FirstOrDefault();
                        if (_list_Staff_Not_free.Exists(x => x.Id == free_staff.Id))
                        {
                            // không add những staff đã lặp lại
                        }
                        else
                        {
                            _list_Staff_Not_free.Add(free_staff);
                        }
                    }
                }
                List<Staff> _list_Staff_free_service = _list_Staff_free.Where(x => x.ServiceId == Service_Id).ToList(); //  Danh sách những Staff không bị trùng lịch với đơn đặt hàng mới lọc theo loại dịch vụ
                List<Staff> _list_Staff_Not_free_service = _list_Staff_Not_free.Where(x => x.ServiceId == Service_Id).ToList(); //  Danh sách những Staff bị trùng lịch với đơn đặt hàng mới lọc theo loại dịch vụ

                // TH1: đơn hàng không trùng ngày với các đơn đã lên.
                if (_list_Staff_Not_free_service.Count == 0)
                {

                    if (_list_Staff_free_service.Count >= Number_Of_Employee)
                    {
                        OrderDetail new_OrderDetail = new OrderDetail();
                        new_OrderDetail.OrderDetail_DateStart = Date_Start;
                        new_OrderDetail.OrderDetail_DateEnd = Date_End;
                        new_OrderDetail.OrderDetail_NumberOfPeople = Number_Of_Employee;
                        new_OrderDetail.OrderDetail_Status = 0;
                        new_OrderDetail.ServiceId = Service_Id;
                        db.OrderDetails.Add(new_OrderDetail);
                        int temp = 0;
                        foreach (var item in _list_Staff_free_service)
                        {
                            Staff_OrderDetail new_Staff_OrderDetail = new Staff_OrderDetail();
                            new_Staff_OrderDetail.OrderDetail_Id = new_OrderDetail.Id;
                            new_Staff_OrderDetail.Staff_Id = item.Id;
                            new_Staff_OrderDetail.Date_Start = Date_Start;
                            new_Staff_OrderDetail.Date_End = Date_End;
                            db.Staff_OrderDetail.Add(new_Staff_OrderDetail);
                            temp += 1;
                            if (temp == Number_Of_Employee)
                            {
                                break;
                            }
                        }
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
                        return Json("/Admin/OrderManagement/Index", JsonRequestBehavior.AllowGet);
                        // Lưu dữ liệu vào db
                        //db.SaveChanges();
                    }
                    else
                    {
                        return Json("The Employee is greater than our Staff", JsonRequestBehavior.AllowGet);
                        // num of employee is greater than free staff. 
                    }
                }
                // Th2: đơn hàng mới trùng với các đơn đã lên.
                else
                {
                    List<Staff> list_staff_have_inTable = db.Staffs.Where(x => x.ServiceId == Service_Id).ToList();
                    if (_list_Staff_Not_free_service.Count <= list_staff_have_inTable.Count)
                    {
                        var list_staff_free = list_staff_have_inTable.Except(_list_Staff_Not_free_service);
                        int a = list_staff_free.Count();
                        if (list_staff_free.Count() >= Number_Of_Employee)
                        {
                            OrderDetail new_OrderDetail = new OrderDetail();
                            new_OrderDetail.OrderDetail_DateStart = Date_Start;
                            new_OrderDetail.OrderDetail_DateEnd = Date_End;
                            new_OrderDetail.OrderDetail_NumberOfPeople = Number_Of_Employee;
                            new_OrderDetail.OrderDetail_Status = 0;
                            new_OrderDetail.ServiceId = Service_Id;
                            db.OrderDetails.Add(new_OrderDetail);
                            int temp = 0;
                            foreach (var item in list_staff_free)
                            {
                                Staff_OrderDetail new_Staff_OrderDetail = new Staff_OrderDetail();
                                new_Staff_OrderDetail.OrderDetail_Id = new_OrderDetail.Id;
                                new_Staff_OrderDetail.Staff_Id = item.Id;
                                new_Staff_OrderDetail.Date_Start = Date_Start;
                                new_Staff_OrderDetail.Date_End = Date_End;
                                db.Staff_OrderDetail.Add(new_Staff_OrderDetail);
                                temp += 1;
                                if (temp == Number_Of_Employee)
                                {
                                    break;
                                }
                            }
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
                            return Json("/Admin/OrderManagement/Index", JsonRequestBehavior.AllowGet);
                            //db.SaveChanges();
                        }
                        else
                        {
                            return Json("The Employee is greater than our Staff", JsonRequestBehavior.AllowGet);
                            // Không đủ Staff để cho thuê
                        }
                    }
                    else
                    {
                        return Json("The Employee is greater than our Staff", JsonRequestBehavior.AllowGet);
                        // Không đủ staff để cho thuê
                    }
                }
            }
            //
        }

        [HttpGet]
        public JsonResult SubmitOrder(float total_cost, string description)
        {

            List<OrderDetail> list_ord = (List<OrderDetail>)Session["OrderDetail"];
            Order new_Order = new Order();
            new_Order.Order_DateCreate = DateTime.Now.ToString("MMMM/dd/yyyy");
            new_Order.Order_Description = description;
            new_Order.Order_TotalCost = total_cost;
            new_Order.Order_Status = 0;
            db.Orders.Add(new_Order);
            foreach (var item in list_ord)
            {
                int Service_Id = (int)item.ServiceId;
                int Number_Of_Employee = (int)item.OrderDetail_NumberOfPeople;
                DateTime Date_Start = (DateTime)item.OrderDetail_DateStart;
                DateTime Date_End = (DateTime)item.OrderDetail_DateEnd;

                // Create Order
                List<Staff> list_Staff_free = db.Staffs.Where(x => x.Staff_OrderDetail.Count == 0 && x.ServiceId == Service_Id).ToList();
                if (list_Staff_free.Count > 0)
                {
                    if (list_Staff_free.Count >= Number_Of_Employee)
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
                        foreach (var item2 in list_Staff_free)
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
                // Nếu tất cả các Staff đều đã tham gia tối thiểu vào 1 OrderDetail thì phải xử lý trùng lặp ngày tháng trong này.
                else
                {
                    var list_Staff_OrderDetail = db.Staff_OrderDetail.ToList();
                    List<Staff> _list_Staff_free = new List<Staff>(); // Danh sách những Staff không bị trùng lịch với đơn đặt hàng mới 
                    List<Staff> _list_Staff_Not_free = new List<Staff>(); //  Danh sách những Staff bị trùng lịch với đơn đặt hàng mới 
                    foreach (var item2 in list_Staff_OrderDetail)
                    {
                        if ((((Date_Start - (DateTime)item2.Date_End).TotalDays > 0) && ((Date_Start - (DateTime)item2.Date_Start).TotalDays > 0)) || (((Date_End - (DateTime)item2.Date_Start).TotalDays < 0) && ((Date_End - (DateTime)item2.Date_Start).TotalDays < 0)))
                        {
                            Staff free_staff = db.Staffs.Where(x => x.Id == item2.Staff_Id).FirstOrDefault();
                            if (_list_Staff_free.Exists(x => x.Id == free_staff.Id))
                            {
                                // không add những staff đã lặp lại
                            }
                            else
                            {
                                _list_Staff_free.Add(free_staff);
                            }

                        }
                        else
                        {
                            Staff free_staff = db.Staffs.Where(x => x.Id == item2.Staff_Id).FirstOrDefault();
                            if (_list_Staff_Not_free.Exists(x => x.Id == free_staff.Id))
                            {
                                // không add những staff đã lặp lại
                            }
                            else
                            {
                                _list_Staff_Not_free.Add(free_staff);
                            }
                        }
                    }
                    List<Staff> _list_Staff_free_service = _list_Staff_free.Where(x => x.ServiceId == Service_Id).ToList(); //  Danh sách những Staff không bị trùng lịch với đơn đặt hàng mới lọc theo loại dịch vụ
                    List<Staff> _list_Staff_Not_free_service = _list_Staff_Not_free.Where(x => x.ServiceId == Service_Id).ToList(); //  Danh sách những Staff bị trùng lịch với đơn đặt hàng mới lọc theo loại dịch vụ

                    // TH1: đơn hàng không trùng ngày với các đơn đã lên.
                    if (_list_Staff_Not_free_service.Count == 0)
                    {

                        if (_list_Staff_free_service.Count >= Number_Of_Employee)
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
                            foreach (var item2 in _list_Staff_free_service)
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
                    // Th2: đơn hàng mới trùng với các đơn đã lên.
                    else
                    {
                        List<Staff> list_staff_have_inTable = db.Staffs.Where(x => x.ServiceId == Service_Id).ToList();
                        if (_list_Staff_Not_free_service.Count <= list_staff_have_inTable.Count)
                        {
                            var list_staff_free = list_staff_have_inTable.Except(_list_Staff_Not_free_service);
                            int a = list_staff_free.Count();
                            if (list_staff_free.Count() >= Number_Of_Employee)
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
                        else
                        {
                            return Json("The Employee is greater than our Staff", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json("Create Order Successfully", JsonRequestBehavior.AllowGet);

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
        public ActionResult form_search()
        {
            return View();
        }
        public ActionResult Search(SearchRequest searchRequest)
        {
            List<Staff> list_Staff_free = orderDetail_Function.Take_List_Staff_Free(searchRequest);
            ViewBag.free_staff = list_Staff_free.Count;
            return View();
        }
        public ActionResult Index()
        {
            List<OrderDetail> list_ord = (List<OrderDetail>)Session["OrderDetail"];
            if (list_ord != null)
            {
                return View(list_ord);
            }
            else
            {
                return RedirectToAction("ServiceIndex", "ServiceManagement");
            }
        }

        [HasPermission(Permission = "Order_Delete")]
        [HttpGet]
        public JsonResult Delete(int OrderId)
        {
            Order order = db.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == order.Id).ToList();
            if (list_ord != null)
            {
                foreach (var item in list_ord)
                {
                    item.OrderDetail_Status = 3;
                }
            }
            order.Order_Status = 3;
            db.SaveChanges();
            return Json("Delete successfully", JsonRequestBehavior.AllowGet);
        }
        
        [HasPermission(Permission = "Order_Activate")]
        [HttpGet]
        public JsonResult Activate(int OrderId)
        {
            Order order = db.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == order.Id).ToList();
            if (list_ord != null)
            {
                foreach (var item in list_ord)
                {
                    item.OrderDetail_Status = 1;
                }
            }
            order.Order_Status = 1;
            db.SaveChanges();
            return Json("Activate successfully", JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Order_Complete")]
        [HttpGet]
        public JsonResult Complete(int OrderId)
        {
            Order order = db.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == order.Id).ToList();
            if (list_ord != null)
            {
                foreach (var item in list_ord)
                {
                    item.OrderDetail_Status = 2;
                }
            } 
            order.Order_Status = 2;
            db.SaveChanges();
            return Json("Complete successfully", JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Order_Complete")]
        [HttpGet]
        public JsonResult Reset(int OrderId)
        {
            Order order = db.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == order.Id).ToList();
            if (list_ord != null)
            {
                foreach (var item in list_ord)
                {
                    item.OrderDetail_Status = 0;
                }
            }
            order.Order_Status = 0;
            db.SaveChanges();
            return Json("Reset successfully", JsonRequestBehavior.AllowGet);
        }
    }
}