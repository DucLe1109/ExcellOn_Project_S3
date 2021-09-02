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
    public class BankingController : BaseController
    {
        private Entities db = new Entities();

        [HasPermission(Permission = "Admin")]
        public ActionResult BankingIndex()
        {
            List<Banking> list_banking = db.Bankings.Where(x => x.Deleted != 1).ToList();
            ViewBag.list_banking = list_banking;
            return View();
        }

        [HasPermission(Permission = "Admin")]
        public ActionResult BankingIndex2()
        {
            List<Banking> list_banking = db.Bankings.Where(x => x.Deleted != 1).ToList();
            foreach (var item in list_banking)
            {
                item.Status = 1;
            }
            db.SaveChanges();
            ViewBag.list_banking = list_banking;
            return View("/Areas/Admin/Views/Banking/BankingIndex.cshtml");
        }

        [HasPermission(Permission = "Admin")]
        public ActionResult BankingDeletedIndex()
        {
            List<Banking> list_banking = db.Bankings.Where(x => x.Deleted == 1).ToList();
            ViewBag.list_banking = list_banking;
            return View();
        }

        [HasPermission(Permission = "Admin")]
        public ActionResult Filter(BankingFilter request)
        {
            List<Banking> _list_banking = db.Bankings.Where(x => x.Deleted != 1).ToList();
            List<Banking> list_banking_filtered = new List<Banking>();
            DateTime Start = request.Start;
            DateTime End = request.End;
            int year_start_default = request.Start.Year;
            int year_end_default = request.End.Year;

            if (year_start_default != 1 && year_end_default != 1)
            {
                if (_list_banking != null)
                {
                    foreach (var item in _list_banking)
                    {
                        DateTime date_create = (DateTime)item.DateCreate;
                        if ((int)(Start - date_create).TotalDays <= 0 && (int)(End - date_create).TotalDays >= 0)
                        {
                            list_banking_filtered.Add(item);
                        }
                    }
                }
                ViewBag.list_banking = list_banking_filtered;
                return View("/Areas/Admin/Views/Banking/BankingIndex.cshtml");
            }
            else if (year_start_default != 1 && year_end_default == 1)
            {
                if (_list_banking != null)
                {
                    foreach (var item in _list_banking)
                    {
                        DateTime date_create = (DateTime)item.DateCreate;
                        if ((int)(Start - date_create).TotalDays <= 0)
                        {
                            list_banking_filtered.Add(item);
                        }
                    }
                }
                ViewBag.list_banking = list_banking_filtered;
                return View("/Areas/Admin/Views/Banking/BankingIndex.cshtml");
            }
            else if (year_start_default == 1 && year_end_default != 1)
            {
                if (_list_banking != null)
                {
                    foreach (var item in _list_banking)
                    {
                        DateTime date_create = (DateTime)item.DateCreate;
                        if ((int)(End - date_create).TotalDays >= 0)
                        {
                            list_banking_filtered.Add(item);
                        }
                    }
                }
                ViewBag.list_banking = list_banking_filtered;
                return View("/Areas/Admin/Views/Banking/BankingIndex.cshtml");
            }
            else
            {
                ViewBag.list_banking = _list_banking;
                return View("/Areas/Admin/Views/Banking/BankingIndex.cshtml");
            }
        }

        [HasPermission(Permission = "Admin")]
        public ActionResult Filter2(BankingFilter request)
        {
            List<Banking> _list_banking = db.Bankings.Where(x => x.Deleted == 1).ToList();
            List<Banking> list_banking_filtered = new List<Banking>();
            DateTime Start = request.Start;
            DateTime End = request.End;
            int year_start_default = request.Start.Year;
            int year_end_default = request.End.Year;

            if (year_start_default != 1 && year_end_default != 1)
            {
                if (_list_banking != null)
                {
                    foreach (var item in _list_banking)
                    {
                        DateTime date_create = (DateTime)item.DateCreate;
                        if ((int)(Start - date_create).TotalDays <= 0 && (int)(End - date_create).TotalDays >= 0)
                        {
                            list_banking_filtered.Add(item);
                        }
                    }
                }
                ViewBag.list_banking = list_banking_filtered;
                return View("/Areas/Admin/Views/Banking/BankingDeletedIndex.cshtml");
            }
            else if (year_start_default != 1 && year_end_default == 1)
            {
                if (_list_banking != null)
                {
                    foreach (var item in _list_banking)
                    {
                        DateTime date_create = (DateTime)item.DateCreate;
                        if ((int)(Start - date_create).TotalDays <= 0)
                        {
                            list_banking_filtered.Add(item);
                        }
                    }
                }
                ViewBag.list_banking = list_banking_filtered;
                return View("/Areas/Admin/Views/Banking/BankingDeletedIndex.cshtml");
            }
            else if (year_start_default == 1 && year_end_default != 1)
            {
                if (_list_banking != null)
                {
                    foreach (var item in _list_banking)
                    {
                        DateTime date_create = (DateTime)item.DateCreate;
                        if ((int)(End - date_create).TotalDays >= 0)
                        {
                            list_banking_filtered.Add(item);
                        }
                    }
                }
                ViewBag.list_banking = list_banking_filtered;
                return View("/Areas/Admin/Views/Banking/BankingDeletedIndex.cshtml");
            }
            else
            {
                ViewBag.list_banking = _list_banking;
                return View("/Areas/Admin/Views/Banking/BankingDeletedIndex.cshtml");
            }
        }

        [HasPermission(Permission = "Admin")]
        [HttpGet]
        public JsonResult Delete(int BankingId)
        {
            Banking banking = db.Bankings.Where(x => x.Id == BankingId).FirstOrDefault();
            if (banking != null)
            {
                banking.Deleted = 1;
                db.SaveChanges();
                return Json("Successfully", JsonRequestBehavior.AllowGet);
            }
            return Json(404, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Admin")]
        [HttpGet]
        public JsonResult Reset(int BankingId)
        {
            Banking banking = db.Bankings.Where(x => x.Id == BankingId).FirstOrDefault();
            if (banking != null)
            {
                banking.Deleted = 0;
                db.SaveChanges();
                return Json("Successfully", JsonRequestBehavior.AllowGet);
            }
            return Json(404, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Order_List")]
        // Dùng để xem chi tiết banking và gán trạng thái đã xem để k hiển thị trên thông báo nữa
        public ActionResult DetailBanking(int id)
        {
            Banking banking = db.Bankings.Where(x => x.OrderId == id).FirstOrDefault();
            if (banking != null)
            {
                banking.Status = 1; // Gán trạng thái đã xem cho thông báo này 
                db.SaveChanges();
            }
            
            Order or = db.Orders.Find(id);
            if (or != null)
            {
                Order_OrderDetail order_OrderDetail = new Order_OrderDetail();
                order_OrderDetail.Orders = or;
                List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == or.Id && x.OrderDetail_Status != 3).ToList();
                if (list_ord != null)
                {
                    order_OrderDetail.List_OrderDetail = list_ord;
                }
                return View("/Areas/Admin/Views/Banking/DetailBanking.cshtml",order_OrderDetail);
            }
            return HttpNotFound();
        }

        [HasPermission(Permission = "Order_List")]
        // Dùng để xem chi tiết banking
        public ActionResult DetailBanking2(int id)
        {
            Order or = db.Orders.Find(id);
            if (or != null)
            {
                Order_OrderDetail order_OrderDetail = new Order_OrderDetail();
                order_OrderDetail.Orders = or;
                List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.OrdersId == or.Id && x.OrderDetail_Status != 3).ToList();
                if (list_ord != null)
                {
                    order_OrderDetail.List_OrderDetail = list_ord;
                }
                return View("/Areas/Admin/Views/Banking/DetailBanking.cshtml", order_OrderDetail);
            }
            return HttpNotFound();
        }
    }
}