using _ExcellOn_.Models;
using _ExcellOn_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class OrderManagementController : Controller
    {
        private Entities db = new Entities();
        
        [HttpPost]
        public JsonResult CreateOrder(EmployRequest employRequest)
        {
            int Service_Id = employRequest.Service_Id;
            int Number_Of_Employee = employRequest.Number_Of_Employee;
            DateTime Date_Start = employRequest.Date_Start;
            DateTime Date_End = employRequest.Date_End;

            // Khởi tạo bảng Staff_OrderDetail. (Nếu chưa có bất kì đơn hàng chi tiết nào thì lấy luôn số lượng staff cần được thuê trong bảng Staff và khởi tạo đơn hàng chi tiết mới)
            List<Staff> list_Staff_free = db.Staffs.Where(x=> x.Staff_OrderDetail.Count == 0 && x.ServiceId == Service_Id).ToList();
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
                    return Json("The Employee is greater than our Staff",JsonRequestBehavior.AllowGet); 
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
                    if ( ( ((Date_Start - (DateTime)item.Date_End).TotalDays > 0) && ((Date_Start - (DateTime)item.Date_Start).TotalDays > 0))  || ( ((Date_End - (DateTime)item.Date_Start).TotalDays < 0) && ((Date_End - (DateTime)item.Date_Start).TotalDays < 0) ) )
                    {
                        Staff free_staff = db.Staffs.Where(x=>x.Id == item.Staff_Id).FirstOrDefault();
                        if (_list_Staff_free.Exists(x=> x.Id == free_staff.Id))
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
        public JsonResult SubmitOrder(float total_cost,string description)
        {
            var session = Session["OrderDetail"];
            if (session != null)
            {
                List<OrderDetail> list_ord = (List<OrderDetail>)Session["OrderDetail"];
                foreach(var item in list_ord)
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
                            Order new_Order = new Order();
                            new_Order.Order_DateCreate = DateTime.Now.ToString("MMMM/dd/yyyy");
                            new_Order.Order_Description = description;
                            new_Order.Order_TotalCost = total_cost;
                            new_Order.Order_Status = 0;
                            db.Orders.Add(new_Order);

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
                            return Json("/Admin/OrderManagement/Index", JsonRequestBehavior.AllowGet);
                            
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
                                Order new_Order = new Order();
                                new_Order.Order_DateCreate = DateTime.Now.ToString("MMMM/dd/yyyy");
                                new_Order.Order_Description = description;
                                new_Order.Order_TotalCost = total_cost;
                                new_Order.Order_Status = 0;
                                db.Orders.Add(new_Order);

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
                                return Json("/Admin/OrderManagement/Index", JsonRequestBehavior.AllowGet);
                                // Lưu dữ liệu vào db

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
                                    Order new_Order = new Order();
                                    new_Order.Order_DateCreate = DateTime.Now.ToString("MMMM/dd/yyyy");
                                    new_Order.Order_Description = description;
                                    new_Order.Order_TotalCost = total_cost;
                                    new_Order.Order_Status = 0;
                                    db.Orders.Add(new_Order);

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
                                    return Json("/Admin/OrderManagement/Index", JsonRequestBehavior.AllowGet);

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
            else
            {
                return Json(Response.StatusCode = 404, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult RemoveItem(int OrderDetail_ID)
        {
            var session = Session["OrderDetail"];
            if (session != null)
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
            else
            {
                return Json(Response.StatusCode = 404, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult form_search()
        {
            return View();
        }
        public ActionResult Search(SearchRequest searchRequest)
        {
            int Service_Id = searchRequest.Service_Id;
            DateTime Date_Start = searchRequest.Date_Start;
            DateTime Date_End = searchRequest.Date_End;
            List<Staff> list_Staff_free = db.Staffs.Where(x => x.Staff_OrderDetail.Count == 0 && x.ServiceId == Service_Id).ToList();
            if (list_Staff_free.Count > 0)
            {
                ViewBag.free_staff = list_Staff_free.Count;
                return View();
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
                    ViewBag.free_staff = _list_Staff_free_service.Count;
                    return View();
                }
                // Th2: đơn hàng mới trùng với các đơn đã lên.
                else
                {
                    List<Staff> list_staff_have_inTable = db.Staffs.Where(x => x.ServiceId == Service_Id).ToList();
                    if (_list_Staff_Not_free_service.Count <= list_staff_have_inTable.Count)
                    {
                        var list_staff_free = list_staff_have_inTable.Except(_list_Staff_Not_free_service);
                        ViewBag.free_staff = list_staff_free.Count();
                        return View();
                    }
                    else
                    {
                        ViewBag.free_staff = 0;
                        return View();
                    }
                }

            }
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
    }
}