using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class AdminHomeController : BaseController
    {
        private Entities db = new Entities();
        // GET: Admin/Home
        [HasPermission(Permission ="Home_Dashboard")]
        public ActionResult DashboardIndex()
        {
            List<Order> list_order_is_comming_start = new List<Order>();
            List<Order> list_order_is_comming_expire = new List<Order>();
            DateTime today = DateTime.Now;
            DateTime future = today.AddDays(100);

            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrderDetail_Status != 3).ToList();
            foreach (var item in list_ord)
            {
                DateTime date_start = (DateTime)item.OrderDetail_DateStart;
                DateTime date_end = (DateTime)item.OrderDetail_DateEnd;
                if ( (date_start - today).TotalDays <= 3 && (date_start - today).TotalDays > 0)
                {
                    if (list_order_is_comming_start.Contains(item.Order) == false)
                    {
                        list_order_is_comming_start.Add(item.Order);
                    }
                }else if ((date_end - today).TotalDays <= 3 && (date_end - today).TotalDays > 0)
                {
                    if (list_order_is_comming_expire.Contains(item.Order) == false)
                    {
                        list_order_is_comming_expire.Add(item.Order);
                    }
                }
            }

            OrderDetail_Function orderDetail_Function = new OrderDetail_Function();
            List<Order> list_order_ready = db.Orders.Where(x => x.Order_Status == 99).ToList();
            List<Order> list_order_completed = db.Orders.Where(x=>x.Order_Status == 2).ToList();
            List<Order> list_order_started = db.Orders.Where(x => x.Order_Status == 1).ToList();
            List<Order> list_order_pending = db.Orders.Where(x => x.Order_Status == 0).ToList();
            List<Customer> list_customer = db.Customers.ToList();
            List<Staff> list_staff = db.Staffs.Where(x=>x.Deleted != 1).ToList();

            List<Staff> list_staff_free_inbound = orderDetail_Function.Take_List_Staff_Free(1, today,future);
            List<Staff> list_staff_free_outbound = orderDetail_Function.Take_List_Staff_Free(2, today,future);
            List<Staff> list_staff_free_telemarketing = orderDetail_Function.Take_List_Staff_Free(3, today,future);

            List<Staff> list_staff_free = new List<Staff>();
            list_staff_free.AddRange(list_staff_free_inbound);
            list_staff_free.AddRange(list_staff_free_outbound);
            list_staff_free.AddRange(list_staff_free_telemarketing);

            int list_staff_in_working_count = list_staff.Count - list_staff_free.Count;


            ViewBag.list_order_is_comming_start = list_order_is_comming_start;
            ViewBag.list_order_is_comming_expire = list_order_is_comming_expire;
            ViewBag.list_order_ready = list_order_ready;
            ViewBag.list_order_completed = list_order_completed;
            ViewBag.list_order_started = list_order_started;
            ViewBag.list_order_pending = list_order_pending;
            ViewBag.list_customer = list_customer;
            ViewBag.list_staff = list_staff;
            ViewBag.list_staff_free = list_staff_free;
            ViewBag.list_staff_in_working_count = list_staff_in_working_count;
            return View();
        }

        [HasPermission(Permission = "Home_Dashboard")]
        public JsonResult GetJsonOrder(int num_of_days_ago)
        {
            // Lấy ra số ngày trước ngày hiện tại. VD: 5 ngày trước ngày hiện tại.(có thêm cả ngày hiện tại để hiển thị)
            DateTime _now = DateTime.Now;
            List<DateTime> list_date = new List<DateTime>();
            list_date.Add(_now);
            for (int i = 1;i<= num_of_days_ago;i++)
            {
                DateTime _new = _now.AddDays(-i);
                list_date.Add(_new);
            }

            List<Order> list_order = new List<Order>();
            foreach (var item in list_date)
            {
                string _date = item.ToString("MMMM/dd/yyyy");
                List<Order> _order = db.Orders.Where(x => (x.Order_Status == 2 || x.Order_Status == 1) && (x.Order_DateCreate == _date)).ToList();
                if (_order != null)
                {
                    foreach (var i in _order)
                    {
                        list_order.Add(i);
                    }
                }
            }
            
            List<OrderViewModel> list_order_vmd = new List<OrderViewModel>();
            foreach (var item in list_order)
            {
                OrderViewModel _new = new OrderViewModel();
                _new.Id = item.Id;
                _new.Order_DateCreate = item.Order_DateCreate;
                _new.Order_Description = item.Order_Description;
                _new.Order_Status = item.Order_Status;
                _new.Order_TotalCost = item.Order_TotalCost;
                if (list_order_vmd.Exists(x => x.Order_DateCreate == item.Order_DateCreate) == true)
                {
                    OrderViewModel orderView = list_order_vmd.Where(x => x.Order_DateCreate == item.Order_DateCreate).FirstOrDefault();
                    orderView.Order_TotalCost += item.Order_TotalCost;
                }
                else
                {
                    list_order_vmd.Add(_new);
                }
            }

            list_order_vmd.Reverse();

            return Json(list_order_vmd, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Home_Dashboard")]
        public JsonResult GetJsonOrderDetail()
        {
            List<Service_OrderDetail> list_service_OrderDetail = new List<Service_OrderDetail>();
            List<OrderDetail> list_ord = db.OrderDetails.Where(x=>x.OrderDetail_Status != 3).ToList();
            List<Service> list_service = db.Services.ToList();
            
            if (list_service != null)
            {
                foreach (var item in list_service)
                {
                    Service_OrderDetail _new = new Service_OrderDetail();
                    _new.Service_Name = item.Service_Name;
                    int count = 0;
                    if (list_ord.Count > 0)
                    {
                        foreach (var item2 in list_ord)
                        {
                            if (item2.ServiceId == item.Id)
                            {
                                count += 1;
                            }
                        }
                        _new.Number_Of_OrderDetail = count;
                        list_service_OrderDetail.Add(_new);
                    }
                }
            }
            return Json(list_service_OrderDetail, JsonRequestBehavior.AllowGet);
        }
    }
}