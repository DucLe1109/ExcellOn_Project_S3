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
        [HasPermission(Permission ="Admin")]
        public ActionResult DashboardIndex()
        {
            var list_order = db.Orders.Where(x=>x.Order_Status == 2).ToList();
            var list_customer = db.Customers.ToList();
            var list_staff = db.Staffs.ToList();
            ViewBag.list_order = list_order;
            ViewBag.list_customer = list_customer;
            ViewBag.list_staff = list_staff;
            return View();
        }

        [HasPermission(Permission = "Order_List")]
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
                Order _order = db.Orders.Where(x => (x.Order_Status == 2 || x.Order_Status == 1) && (x.Order_DateCreate == _date)).FirstOrDefault();
                if (_order != null)
                {
                    list_order.Add(_order);
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

        [HasPermission(Permission = "OrderDetail_List")]
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