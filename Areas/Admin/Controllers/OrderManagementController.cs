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
            List<Order_OrderDetail> list_order_orderdetail = new List<Order_OrderDetail>();

            List<Order> list_or = db.Orders.Where(x => x.Order_Status != 3).ToList();
            int count = 0;
            list_or.Reverse();
            foreach (var item in list_or)
            {
                // Lấy dữ liệu bảng order và các bảng orderdetail tương ứng
                Order_OrderDetail _new = new Order_OrderDetail();

                List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id).ToList();
                if (list_ord != null)
                {
                    _new.List_OrderDetail = list_ord;
                }
                list_order_orderdetail.Add(_new);

                // Kiểm tra nếu tất cả order detail có cùng trạng thái thì update trạng thái tương ứng của order
                if (list_ord.Count > 0)
                {
                    int list_ord_count = list_ord.Count;
                    int status_count = 0;
                    var ord = list_ord.Take(1).ToList();
                    OrderDetail _ord = new OrderDetail();
                    foreach (var abc in ord)
                    {
                        _ord.Id = abc.Id;
                        _ord.OrderDetail_Status = abc.OrderDetail_Status;
                    }
                    int status = (int)_ord.OrderDetail_Status;
                    foreach (var item2 in list_ord)
                    {
                        if (item2.OrderDetail_Status == _ord.OrderDetail_Status)
                        {
                            status_count += 1;
                        }
                    }
                    if (status_count == list_ord_count)
                    {
                        item.Order_Status = _ord.OrderDetail_Status;
                    }
                }
                else
                {
                    item.Order_Status = 3;
                }

                // Sau khi cập nhật order status thì lấy những orderstatus có trạng thái chưa xóa
                if (item.Order_Status != 3)
                {
                    _new.Orders = item;
                }
                count += 1;
                if (count >= 15)
                {
                    break;
                }
            }
            db.SaveChanges();
            List<int> List_Order_Status_Id = new List<int>();
            List_Order_Status_Id.Add(0);
            List_Order_Status_Id.Add(1);
            List_Order_Status_Id.Add(2);
            ViewBag.list_order_orderdetail = list_order_orderdetail;
            ViewBag.List_Order_Status_Id = List_Order_Status_Id;
            return View();
        }

        [HasPermission(Permission = "Order_List")]
        public ActionResult Filter(OrderFilterViewModel request)
        {
            DateTime date_start = request.date_start;
            DateTime date_end = request.date_end;
            int year_start_default = date_start.Year;
            int year_end_default = date_end.Year;

            List<Order_OrderDetail> list_order_orderdetail = new List<Order_OrderDetail>();
            List<Order> list_or = new List<Order>();
            List<int> List_Order_Status_Id = request.List_Order_Status_Id;
            if (List_Order_Status_Id != null && List_Order_Status_Id.Count > 0)
            {
                if (year_start_default != 1 && year_end_default != 1)
                {
                    foreach (var Id in List_Order_Status_Id)
                    {
                        List<Order> list_order = db.Orders.Where(x => x.Order_Status == Id).ToList();
                        if (list_order != null)
                        {
                            foreach (var order in list_order)
                            {
                                DateTime order_datecreated = DateTime.Parse(order.Order_DateCreate);
                                int interval_first = (int)(date_start - order_datecreated).TotalDays;
                                int interval_after = (int)(date_end - order_datecreated).TotalDays;
                                if (interval_first <= 0 && interval_after >= 0)
                                {
                                    list_or.Add(order);
                                }
                            }
                        }
                    }
                    list_or.Reverse();
                    foreach (var item in list_or)
                    {
                        Order_OrderDetail _new = new Order_OrderDetail();
                        _new.Orders = item;
                        List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id).ToList();
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
                else if (year_start_default != 1)
                {
                    foreach (var Id in List_Order_Status_Id)
                    {
                        List<Order> list_order = db.Orders.Where(x => x.Order_Status == Id).ToList();
                        if (list_order != null)
                        {
                            foreach (var order in list_order)
                            {
                                DateTime order_datecreated = DateTime.Parse(order.Order_DateCreate);
                                int interval_first = (int)(date_start - order_datecreated).TotalDays;
                                if (interval_first <= 0)
                                {
                                    list_or.Add(order);
                                }
                            }
                        }
                    }
                    list_or.Reverse();
                    foreach (var item in list_or)
                    {
                        Order_OrderDetail _new = new Order_OrderDetail();
                        _new.Orders = item;
                        List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id).ToList();
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
                else if (year_end_default != 1)
                {
                    foreach (var Id in List_Order_Status_Id)
                    {
                        List<Order> list_order = db.Orders.Where(x => x.Order_Status == Id).ToList();
                        if (list_order != null)
                        {
                            foreach (var order in list_order)
                            {
                                DateTime order_datecreated = DateTime.Parse(order.Order_DateCreate);
                                int interval_after = (int)(date_end - order_datecreated).TotalDays;
                                if (interval_after >= 0)
                                {
                                    list_or.Add(order);
                                }
                            }
                        }
                    }
                    list_or.Reverse();
                    foreach (var item in list_or)
                    {
                        Order_OrderDetail _new = new Order_OrderDetail();
                        _new.Orders = item;
                        List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id).ToList();
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
                    foreach (var Id in List_Order_Status_Id)
                    {
                        List<Order> list_order = db.Orders.Where(x => x.Order_Status == Id).ToList();
                        if (list_order != null)
                        {
                            foreach (var order in list_order)
                            {
                                list_or.Add(order);
                            }
                        }
                    }
                    list_or.Reverse();
                    foreach (var item in list_or)
                    {
                        Order_OrderDetail _new = new Order_OrderDetail();
                        _new.Orders = item;
                        List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id).ToList();
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
            }
            else
            {
                if (year_end_default != 1 && year_start_default != 1)
                {

                    List<Order> list_order = db.Orders.OrderBy(x => x.Order_Status).ToList();
                    if (list_order != null)
                    {
                        foreach (var order in list_order)
                        {
                            DateTime order_datecreated = DateTime.Parse(order.Order_DateCreate);
                            int interval_first = (int)(date_start - order_datecreated).TotalDays;
                            int interval_after = (int)(date_end - order_datecreated).TotalDays;
                            if (interval_first <= 0 && interval_after >= 0)
                            {
                                list_or.Add(order);
                            }
                        }
                    }
                    foreach (var item in list_or)
                    {
                        Order_OrderDetail _new = new Order_OrderDetail();
                        _new.Orders = item;
                        List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id).ToList();
                        if (list_ord != null)
                        {
                            _new.List_OrderDetail = list_ord;
                        }
                        list_order_orderdetail.Add(_new);
                    }
                    ViewBag.list_order_orderdetail = list_order_orderdetail;
                    ViewBag.List_Order_Status_Id = null;
                    return View("/Areas/Admin/Views/OrderManagement/OrderIndex.cshtml");
                }
                else if (year_end_default != 1)
                {

                    List<Order> list_order = db.Orders.OrderBy(x => x.Order_Status).ToList();
                    if (list_order != null)
                    {
                        foreach (var order in list_order)
                        {
                            DateTime order_datecreated = DateTime.Parse(order.Order_DateCreate);
                            int interval_after = (int)(date_end - order_datecreated).TotalDays;
                            if (interval_after >= 0)
                            {
                                list_or.Add(order);
                            }
                        }
                    }
                    foreach (var item in list_or)
                    {
                        Order_OrderDetail _new = new Order_OrderDetail();
                        _new.Orders = item;
                        List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id).ToList();
                        if (list_ord != null)
                        {
                            _new.List_OrderDetail = list_ord;
                        }
                        list_order_orderdetail.Add(_new);
                    }
                    ViewBag.list_order_orderdetail = list_order_orderdetail;
                    ViewBag.List_Order_Status_Id = null;
                    return View("/Areas/Admin/Views/OrderManagement/OrderIndex.cshtml");
                }
                else if (year_start_default != 1)
                {

                    List<Order> list_order = db.Orders.OrderBy(x => x.Order_Status).ToList();
                    if (list_order != null)
                    {
                        foreach (var order in list_order)
                        {
                            DateTime order_datecreated = DateTime.Parse(order.Order_DateCreate);
                            int interval_first = (int)(date_start - order_datecreated).TotalDays;
                            if (interval_first <= 0)
                            {
                                list_or.Add(order);
                            }
                        }
                    }
                    foreach (var item in list_or)
                    {
                        Order_OrderDetail _new = new Order_OrderDetail();
                        _new.Orders = item;
                        List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id).ToList();
                        if (list_ord != null)
                        {
                            _new.List_OrderDetail = list_ord;
                        }
                        list_order_orderdetail.Add(_new);
                    }
                    ViewBag.list_order_orderdetail = list_order_orderdetail;
                    ViewBag.List_Order_Status_Id = null;
                    return View("/Areas/Admin/Views/OrderManagement/OrderIndex.cshtml");
                }
                else
                {
                    List<Order> list_order = db.Orders.Where(x => x.Order_Status != 3).OrderBy(x => x.Order_Status).ToList();
                    if (list_order != null)
                    {
                        foreach (var order in list_order)
                        {
                            list_or.Add(order);
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
            }
        }

        [HasPermission(Permission = "Order_List")]
        public ActionResult Filter2(int OrderStatus)
        {
            List<Order_OrderDetail> list_order_orderdetail = new List<Order_OrderDetail>();
            List<Order> list_or = db.Orders.Where(x => x.Order_Status == OrderStatus).ToList();
            List<int> List_Order_Status_Id = new List<int>();
            List_Order_Status_Id.Add(OrderStatus);
            foreach (var item in list_or)
            {
                Order_OrderDetail _new = new Order_OrderDetail();
                _new.Orders = item;
                List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == item.Id).ToList();
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

        [HasPermission(Permission = "Order_Create")]
        [HttpPost]
        public JsonResult CreateOrder(EmployRequest employRequest)
        {
            int Service_Id = employRequest.Service_Id;
            int Number_Of_Employee = employRequest.Number_Of_Employee;
            DateTime Date_Start = employRequest.Date_Start;
            DateTime Date_End = employRequest.Date_End;

            // Khởi tạo bảng Staff_OrderDetail. (Nếu chưa có bất kì đơn hàng chi tiết nào thì lấy luôn số lượng staff cần được thuê trong bảng Staff và khởi tạo đơn hàng chi tiết mới)
            List<Staff> list_Staff_free = db.Staffs.Where(x => x.Staff_OrderDetail.Count == 0 && x.ServiceId == Service_Id).ToList();
            int list_Staff_has_not_any_orderdetail_count = list_Staff_free.Count;
            if (list_Staff_free.Count > 0 && list_Staff_free.Count >= Number_Of_Employee)
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
                    if (list_Staff_has_not_any_orderdetail_count > 0)
                    {
                        foreach (var staff_has_not_any_orderdetail in list_Staff_free)
                        {
                            _list_Staff_free_service.Add(staff_has_not_any_orderdetail);
                        }
                    }

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

                        int a = list_staff_free.Count() + list_Staff_has_not_any_orderdetail_count;
                        if (a >= Number_Of_Employee)
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
                            if (temp < Number_Of_Employee)
                            {
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


        [HasPermission(Permission = "Order_Create")]
        [HttpGet]
        public JsonResult SubmitOrder(float total_cost, string Company_Description,string Customer_Description,int Customer_Gender, string Company_Name, string Company_Phone, string Company_Email, string Company_Address,string Customer_Name,string Customer_Phone,string Customer_Email)
        {
            Company company = db.Companies.Where(x => x.Company_Email == Company_Email).FirstOrDefault();

            Order new_Order = new Order();
            Customer cus = db.Customers.Where(x => x.Customer_Email == Customer_Email).FirstOrDefault();
            if (cus != null && company != null)
            {
                new_Order.Order_DateCreate = DateTime.Now.ToString("MMMM/dd/yyyy");
                new_Order.Order_Description = Customer_Description;
                new_Order.Order_TotalCost = total_cost;
                new_Order.Order_Status = 0;
                new_Order.CustomerId = cus.Id;
                cus.Company = company;
                db.Orders.Add(new_Order);
            }
            else if (cus == null && company == null)
            {
                Customer _new = new Customer();
                _new.Customer_UserName = Customer_Phone;
                _new.Customer_Name = Customer_Name;
                _new.Customer_Password = BCrypt.Net.BCrypt.HashPassword(Customer_Phone);
                _new.Customer_Email = Customer_Email;
                _new.Customer_Phone = Customer_Phone;
                _new.Customer_Gender = Customer_Gender;
                if (Customer_Gender == 0)
                {
                    _new.Customer_Avatar = "/Public/Image/a17.jfif";
                }
                else
                {
                    _new.Customer_Avatar = "/Public/Image/1761894.png";
                }
                db.Customers.Add(_new);

                Company _newCompany = new Company();
                _newCompany.Company_Address = Company_Address;
                _newCompany.Company_Description = Company_Description;
                _newCompany.Company_Email = Company_Email;
                _newCompany.Company_Name = Company_Name;
                _newCompany.Company_Phone = Company_Phone;
                db.Companies.Add(_newCompany);

                _new.Company = _newCompany;

                new_Order.Order_DateCreate = DateTime.Now.ToString("MMMM/dd/yyyy");
                new_Order.Order_Description = Customer_Description;
                new_Order.Order_TotalCost = total_cost;
                new_Order.Order_Status = 0;
                new_Order.CustomerId = _new.Id;
                db.Orders.Add(new_Order);
            }
            else if (cus != null && company == null)
            {
                Company _newCompany = new Company();
                _newCompany.Company_Address = Company_Address;
                _newCompany.Company_Description = Company_Description;
                _newCompany.Company_Email = Company_Email;
                _newCompany.Company_Name = Company_Name;
                _newCompany.Company_Phone = Company_Phone;
                cus.Company = _newCompany;
                db.Companies.Add(_newCompany);

                new_Order.Order_DateCreate = DateTime.Now.ToString("MMMM/dd/yyyy");
                new_Order.Order_Description = Customer_Description;
                new_Order.Order_TotalCost = total_cost;
                new_Order.Order_Status = 0;
                new_Order.CustomerId = cus.Id;
                db.Orders.Add(new_Order);
            }
            else if (cus == null && company != null)
            {
                Customer _new = new Customer();
                _new.Customer_UserName = Customer_Phone;
                _new.Customer_Name = Customer_Name;
                _new.Customer_Password = BCrypt.Net.BCrypt.HashPassword(Customer_Phone);
                _new.Customer_Email = Customer_Email;
                _new.Customer_Phone = Customer_Phone;
                _new.Customer_Gender = Customer_Gender;
                _new.Company = company;
                if (Customer_Gender == 0)
                {
                    _new.Customer_Avatar = "/Public/Image/a17.jfif";
                }
                else
                {
                    _new.Customer_Avatar = "/Public/Image/1761894.png";
                }
                
                db.Customers.Add(_new);
                new_Order.Order_DateCreate = DateTime.Now.ToString("MMMM/dd/yyyy");
                new_Order.Order_Description = Customer_Description;
                new_Order.Order_TotalCost = total_cost;
                new_Order.Order_Status = 0;
                new_Order.CustomerId = _new.Id;
                db.Orders.Add(new_Order);
            }
            List<OrderDetail> list_ord = (List<OrderDetail>)Session["OrderDetail"];
            db.SaveChanges();

            foreach (var item in list_ord)
            {
                int Service_Id = (int)item.ServiceId;
                int Number_Of_Employee = (int)item.OrderDetail_NumberOfPeople;
                DateTime Date_Start = (DateTime)item.OrderDetail_DateStart;
                DateTime Date_End = (DateTime)item.OrderDetail_DateEnd;

                // Create Order

                List<Staff> list_Staff_free = db.Staffs.Where(x => x.Staff_OrderDetail.Count == 0 && x.ServiceId == Service_Id).ToList();
                int list_Staff_has_not_any_orderdetail_count = list_Staff_free.Count;
                if (list_Staff_free.Count > 0 && list_Staff_free.Count >= Number_Of_Employee)
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
                        if (list_Staff_has_not_any_orderdetail_count > 0)
                        {
                            foreach (var staff_has_not_any_orderdetail in list_Staff_free)
                            {
                                _list_Staff_free_service.Add(staff_has_not_any_orderdetail);
                            }
                        }

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
                            int a = list_staff_free.Count() + list_Staff_has_not_any_orderdetail_count;
                            if (a >= Number_Of_Employee)
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
                                if (temp < Number_Of_Employee)
                                {
                                    foreach (var item3 in list_Staff_free)
                                    {
                                        Staff_OrderDetail new_Staff_OrderDetail = new Staff_OrderDetail();
                                        new_Staff_OrderDetail.OrderDetail_Id = new_OrderDetail.Id;
                                        new_Staff_OrderDetail.Staff_Id = item3.Id;
                                        new_Staff_OrderDetail.Date_Start = Date_Start;
                                        new_Staff_OrderDetail.Date_End = Date_End;
                                        db.Staff_OrderDetail.Add(new_Staff_OrderDetail);
                                        temp += 1;
                                        if (temp == Number_Of_Employee)
                                        {
                                            break;
                                        }
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
            Session["OrderDetail"] = null;
            return Json("Create Order Successfully", JsonRequestBehavior.AllowGet);

        }

        [HasPermission(Permission = "Order_Create")]
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

        //[HasPermission(Permission = "Order_List")]
        //public ActionResult form_search()
        //{
        //    return View();
        //}

        //[HasPermission(Permission = "Order_List")]
        //public ActionResult Search(SearchRequest searchRequest)
        //{
        //    List<Staff> list_Staff_free = orderDetail_Function.Take_List_Staff_Free(searchRequest);
        //    ViewBag.free_staff = list_Staff_free.Count;
        //    return View();
        //}

        [HasPermission(Permission = "Order_Create")]
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
            List<ViewModel.OrderDetailViewModel> list_ord_vmd = new List<ViewModel.OrderDetailViewModel>();
            foreach (var item in list_ord)
            {
                ViewModel.OrderDetailViewModel _new = new ViewModel.OrderDetailViewModel();
                _new.Id = item.Id;
                _new.OrderDetail_DateEnd = item.OrderDetail_DateEnd;
                _new.OrderDetail_DateStart = item.OrderDetail_DateStart;
                _new.OrderDetail_NumberOfPeople = item.OrderDetail_NumberOfPeople;
                _new.OrderDetail_Status = item.OrderDetail_Status;
                _new.OrdersId = item.OrdersId;
                _new.ServiceId = item.ServiceId;
                list_ord_vmd.Add(_new);
            }
            if (list_ord != null)
            {
                foreach (var item in list_ord)
                {
                    item.OrderDetail_Status = 3;
                    List<Staff_OrderDetail> list_staff_ord = db.Staff_OrderDetail.Where(x => x.OrderDetail_Id == item.Id).ToList();
                    foreach (var item2 in list_staff_ord)
                    {
                        db.Staff_OrderDetail.Remove(item2);
                    }
                }
            }
            order.Order_Status = 3;
            db.SaveChanges();
            return Json(list_ord_vmd, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Order_Activate")]
        [HttpGet]
        public JsonResult Activate(int OrderId)
        {
            Order order = db.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == order.Id).ToList();
            List<ViewModel.OrderDetailViewModel> list_ord_vmd = new List<ViewModel.OrderDetailViewModel>();
            foreach (var item in list_ord)
            {
                ViewModel.OrderDetailViewModel _new = new ViewModel.OrderDetailViewModel();
                _new.Id = item.Id;
                _new.OrderDetail_DateEnd = item.OrderDetail_DateEnd;
                _new.OrderDetail_DateStart = item.OrderDetail_DateStart;
                _new.OrderDetail_NumberOfPeople = item.OrderDetail_NumberOfPeople;
                _new.OrderDetail_Status = item.OrderDetail_Status;
                _new.OrdersId = item.OrdersId;
                _new.ServiceId = item.ServiceId;
                list_ord_vmd.Add(_new);
            }
            if (list_ord != null)
            {
                foreach (var item in list_ord)
                {
                    item.OrderDetail_Status = 1;
                }
            }
            order.Order_Status = 1;
            db.SaveChanges();
            return Json(list_ord_vmd, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Order_Complete")]
        [HttpGet]
        public JsonResult Complete(int OrderId)
        {
            Order order = db.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == order.Id).ToList();
            List<ViewModel.OrderDetailViewModel> list_ord_vmd = new List<ViewModel.OrderDetailViewModel>();
            foreach (var item in list_ord)
            {
                ViewModel.OrderDetailViewModel _new = new ViewModel.OrderDetailViewModel();
                _new.Id = item.Id;
                _new.OrderDetail_DateEnd = item.OrderDetail_DateEnd;
                _new.OrderDetail_DateStart = item.OrderDetail_DateStart;
                _new.OrderDetail_NumberOfPeople = item.OrderDetail_NumberOfPeople;
                _new.OrderDetail_Status = item.OrderDetail_Status;
                _new.OrdersId = item.OrdersId;
                _new.ServiceId = item.ServiceId;
                list_ord_vmd.Add(_new);
            }
            if (list_ord != null)
            {
                foreach (var item in list_ord)
                {
                    item.OrderDetail_Status = 2;
                }
            }
            order.Order_Status = 2;
            db.SaveChanges();
            return Json(list_ord_vmd, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Order_Reset")]
        [HttpGet]
        public JsonResult Reset(int OrderId)
        {
            Order order = db.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == order.Id).ToList();
            List<ViewModel.OrderDetailViewModel> list_ord_vmd = new List<ViewModel.OrderDetailViewModel>();
            foreach (var item in list_ord)
            {
                ViewModel.OrderDetailViewModel _new = new ViewModel.OrderDetailViewModel();
                _new.Id = item.Id;
                _new.OrderDetail_DateEnd = item.OrderDetail_DateEnd;
                _new.OrderDetail_DateStart = item.OrderDetail_DateStart;
                _new.OrderDetail_NumberOfPeople = item.OrderDetail_NumberOfPeople;
                _new.OrderDetail_Status = item.OrderDetail_Status;
                _new.OrdersId = item.OrdersId;
                _new.ServiceId = item.ServiceId;
                list_ord_vmd.Add(_new);
            }
            if (list_ord != null)
            {
                foreach (var item in list_ord)
                {
                    item.OrderDetail_Status = 0;
                }
            }
            order.Order_Status = 0;
            db.SaveChanges();
            return Json(list_ord_vmd, JsonRequestBehavior.AllowGet);
        }
    }
}