using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class ServiceManagementController : BaseController
    {
        private Entities db = new Entities();
        private OrderDetail_Function orderDetail_Function = new OrderDetail_Function();
        // GET: Admin/ServiceManagement

        [HasPermission(Permission = "Service_List")]
        public ActionResult ServiceIndex()
        {
            var list_service = db.Services.ToList();
            ViewBag.list_service = list_service;
            return View();

        }

        [HasPermission(Permission = "OrderDetail_List")]
        public ActionResult ServiceIndex2()
        {
            List<Service> list_service = db.Services.ToList();
            return View(list_service);

        }

        [HasPermission(Permission = "OrderDetail_Assign")]
        public ActionResult Assign_Staff(int id)
        {
            OrderDetail ord = db.OrderDetails.Where(x => x.Id == id).FirstOrDefault();
            List<Staff_OrderDetail> list_id_staff_ord = db.Staff_OrderDetail.Where(x => x.OrderDetail_Id == id).ToList();
            List<Staff> list_staff_this_ord = new List<Staff>();
            foreach (var item in list_id_staff_ord)
            {
                Staff _staff = db.Staffs.Where(x => x.Id == item.Staff_Id).FirstOrDefault();
                list_staff_this_ord.Add(_staff);
            }
            int list_staff_this_ord_count = list_staff_this_ord.Count;

            if (list_staff_this_ord_count < 21)
            {
                int list_staff_free_add_count = 21 - list_staff_this_ord_count;
                List<Staff> list_staff_free = orderDetail_Function.Take_List_Staff_Free(id);
                List<Staff> list_staff_append = new List<Staff>();

                var _list_staff_free = list_staff_free.Take(list_staff_free_add_count);
                foreach (var item in _list_staff_free)
                {
                    list_staff_append.Add(item);
                }

                ViewBag.list_staff_this_ord = list_staff_this_ord;
                ViewBag.list_staff_append = list_staff_append;
                return View(ord);
            }
            ViewBag.list_staff_this_ord = list_staff_this_ord;
            return View(ord);
        }

        [HasPermission(Permission = "OrderDetail_Assign")]
        [HttpGet]
        public JsonResult LoadMore(int ord_id, int list_staff_was_append_count)
        {
            AppendedStaff list_appendedStaff = new AppendedStaff();

            OrderDetail ord = db.OrderDetails.Where(x => x.Id == ord_id).FirstOrDefault();
            List<Staff> list_staff_free = orderDetail_Function.Take_List_Staff_Free(ord.Id);
            var list_staff_append = (from x in list_staff_free select x).Skip(list_staff_was_append_count).Take(21);
            List<StaffViewModel> list_staff_vmd_free_append = new List<StaffViewModel>();
            foreach (var item in list_staff_append)
            {
                StaffViewModel _new = new StaffViewModel();
                _new.Id = item.Id;
                _new.ServiceId = item.ServiceId;
                _new.Staff_AboutMe = item.Staff_AboutMe;
                _new.Staff_Address = item.Staff_Address;
                _new.Staff_Age = item.Staff_Age;
                _new.Staff_Avatar = item.Staff_Avatar;
                _new.Staff_Email = item.Staff_Email;
                _new.Staff_FullName = item.Staff_FullName;
                _new.Staff_Gender = item.Staff_Gender;
                _new.Staff_Password = item.Staff_Password;
                _new.Staff_Phone = item.Staff_Phone;
                _new.Staff_Status = item.Staff_Status;
                _new.Staff_UserName = item.Staff_UserName;
                list_staff_vmd_free_append.Add(_new);
            }

            if (list_staff_vmd_free_append.Count < 21)
            {
                list_appendedStaff.isLoadMore = false;
                list_appendedStaff.list_staff_append = list_staff_vmd_free_append;
            }
            else
            {
                list_appendedStaff.isLoadMore = true;
                list_appendedStaff.list_staff_append = list_staff_vmd_free_append;
            }
            return Json(list_appendedStaff, JsonRequestBehavior.AllowGet);
        }
        //
        //public JsonResult GetTotalPage(int items, int ord_id)
        //{
        //    // Take list_staff_free
        //    var list_staff_free_for_thisOrd = orderDetail_Function.Take_List_Staff_Free(ord_id);
        //    // End Take list_staff_free

        //    // take total page
        //    int p;
        //    var list_staff_free_for_thisOrd_Count = list_staff_free_for_thisOrd.Count;
        //    float pages_tamp = (float)list_staff_free_for_thisOrd_Count / items;
        //    float pages_tamp2 = (int)pages_tamp;
        //    pages_tamp2 = (float)(pages_tamp2 + 0.00000000001);
        //    if (pages_tamp > pages_tamp2)
        //    {
        //        p = (int)pages_tamp2 + 1;
        //    }
        //    else
        //    {
        //        p = (int)pages_tamp2;
        //    }
        //    // end take total page

        //    return Json(p, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetItem(GetItemObj getItemObj)
        //{
        //    if (getItemObj.list_staff_id_checked != null)
        //    {
        //        List<int> list_staff_id_checked = getItemObj.list_staff_id_checked;
        //        List<int> list_staff_id_checked_session = (List<int>)Session["list_staff_id_checked"];

        //        // Th1: chưa có session thì khởi tạo session mới
        //        if (list_staff_id_checked_session == null)
        //        {
        //            List<int> list_staff_id_checked_new_session = new List<int>();
        //            foreach (var item in list_staff_id_checked)
        //            {
        //                list_staff_id_checked_new_session.Add(item);
        //            }
        //            Session["list_staff_id_checked"] = list_staff_id_checked_new_session;
        //        }
        //        // Th2: có session rồi thì chia 2 trường hợp là thêm các checked hoặc bớt các checked
        //        else
        //        {

        //            // TH1:  request bỏ các checked (bớt), hoặc check thêm từ trang mới.
        //            if (list_staff_id_checked_session.Count > list_staff_id_checked.Count)
        //            {
        //                List<int> list_staff_id_checked_session_haveto_delete_or_add = new List<int>();
        //                foreach (var item in list_staff_id_checked_session)
        //                {
        //                    if (list_staff_id_checked.Contains(item) == false)
        //                    {
        //                        list_staff_id_checked_session_haveto_delete_or_add.Add(item);
        //                    }
        //                }
        //                if (list_staff_id_checked_session_haveto_delete_or_add.Count == list_staff_id_checked_session.Count && list_staff_id_checked_session_haveto_delete_or_add.Except(list_staff_id_checked_session).Any() == false)
        //                {
        //                    foreach (var item in list_staff_id_checked)
        //                    {
        //                        list_staff_id_checked_session.Add(item);
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (var item in list_staff_id_checked_session_haveto_delete_or_add)
        //                    {
        //                        if (list_staff_id_checked_session.Contains(item))
        //                        {
        //                            list_staff_id_checked_session.Remove(item);
        //                        }
        //                    }
        //                }


        //                Session["list_staff_id_checked"] = list_staff_id_checked_session;

        //            }
        //            // TH2: request có các checked mới (thêm)
        //            else if (list_staff_id_checked_session.Count < list_staff_id_checked.Count)
        //            {
        //                foreach (var item in list_staff_id_checked)
        //                {
        //                    if (list_staff_id_checked_session.Contains(item) == false)
        //                    {
        //                        list_staff_id_checked_session.Add(item);
        //                    }
        //                }
        //                Session["list_staff_id_checked"] = list_staff_id_checked_session;
        //            }
        //            else if (list_staff_id_checked_session.Count == list_staff_id_checked.Count)
        //            {

        //                list_staff_id_checked_session.Clear();

        //                foreach (var item in list_staff_id_checked)
        //                {
        //                    list_staff_id_checked_session.Add(item);
        //                }
        //                Session["list_staff_id_checked"] = list_staff_id_checked_session;
        //            }
        //        }
        //    }

        //    int current_page = getItemObj.current_page;
        //    int ord_id = getItemObj.ord_id;

        //    // Take all staff free for this ord 
        //    List<Staff> list_staff_free = orderDetail_Function.Take_List_Staff_Free(ord_id);
        //    // End 

        //    // Take all staff got assigned for this ord
        //    List<Staff_OrderDetail> list_id_staff_of_ord = db.Staff_OrderDetail.Where(x => x.OrderDetail_Id == ord_id).ToList();
        //    foreach (var item in list_id_staff_of_ord)
        //    {
        //        Staff staff = db.Staffs.Where(x => x.Id == item.Staff_Id).FirstOrDefault();
        //        // Add staff got assigned in first's list
        //        list_staff_free.Insert(0, staff);
        //    }
        //    // End

        //    int offset = (current_page - 1) * 21;
        //    var list_staff = (from x in list_staff_free select x).Skip(offset).Take(21);
        //    List<StaffViewModel> list_staff_vm = new List<StaffViewModel>();
        //    foreach (var item in list_staff)
        //    {
        //        StaffViewModel _new = new StaffViewModel();
        //        _new.Id = item.Id;
        //        _new.ServiceId = item.ServiceId;
        //        _new.Staff_AboutMe = item.Staff_AboutMe;
        //        _new.Staff_Address = item.Staff_Address;
        //        _new.Staff_Age = item.Staff_Age;
        //        _new.Staff_Avatar = item.Staff_Avatar;
        //        _new.Staff_Email = item.Staff_Email;
        //        _new.Staff_FullName = item.Staff_FullName;
        //        _new.Staff_Gender = item.Staff_Gender;
        //        _new.Staff_Password = item.Staff_Password;
        //        _new.Staff_Phone = item.Staff_Phone;
        //        _new.Staff_Status = item.Staff_Status;
        //        _new.Staff_UserName = item.Staff_UserName;
        //        list_staff_vm.Add(_new);
        //    }

        //    GetItemObj_Return getItemObj_Return = new GetItemObj_Return();
        //    getItemObj_Return.list_staff = list_staff_vm;
        //    getItemObj_Return.list_staff_id_checked = (List<int>)Session["list_staff_id_checked"];
        //    return Json(getItemObj_Return, JsonRequestBehavior.AllowGet);
        //}

        [HasPermission(Permission = "OrderDetail_Assign")]
        [HttpPost]
        public JsonResult SubmitAssignment(AssignmentSubmitViewModel request)
        {
            int ord_id = request.orderdetail_id;
            OrderDetail ord = db.OrderDetails.Where(x => x.Id == ord_id).FirstOrDefault();

            List<int> list_staff_id = request.list_staff_id;
            List<Staff_OrderDetail> list_staff_OrderDetail = db.Staff_OrderDetail.Where(x => x.OrderDetail_Id == ord_id).ToList();
            foreach (var item in list_staff_OrderDetail)
            {
                if (list_staff_id.Contains((int)item.Staff_Id) == false)
                {
                    db.Staff_OrderDetail.Remove(item);
                }
            }
            foreach (var item in list_staff_id)
            {
                if (list_staff_OrderDetail.Exists(x => x.Staff_Id == item) == false)
                {
                    Staff_OrderDetail _new = new Staff_OrderDetail();
                    _new.Staff_Id = item;
                    _new.OrderDetail_Id = ord_id;
                    _new.Date_Start = ord.OrderDetail_DateStart;
                    _new.Date_End = ord.OrderDetail_DateEnd;
                    db.Staff_OrderDetail.Add(_new);
                }
            }
            db.SaveChanges();
            return Json("/Admin/ServiceManagement/Assign_Staff/" + ord_id, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "OrderDetail_Activate")]
        [HttpGet]
        public JsonResult Activate(int ord_id)
        {
            OrderDetail orderDetail = db.OrderDetails.Where(x => x.Id == ord_id).FirstOrDefault();
            if (orderDetail != null)
            {
                orderDetail.OrderDetail_Status = 1;
            }
            db.SaveChanges();
            return Json(orderDetail.Id, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "OrderDetail_Complete")]
        [HttpGet]
        public JsonResult Complete(int ord_id)
        {
            OrderDetail orderDetail = db.OrderDetails.Where(x => x.Id == ord_id).FirstOrDefault();
            if (orderDetail != null)
            {
                orderDetail.OrderDetail_Status = 2;
            }
            db.SaveChanges();
            return Json(orderDetail.Id, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "OrderDetail_Delete")]
        [HttpGet]
        public JsonResult Delete_ord(int ord_id)
        {
            OrderDetail orderDetail = db.OrderDetails.Where(x => x.Id == ord_id).FirstOrDefault();
            int interval = (int)((DateTime)orderDetail.OrderDetail_DateEnd - (DateTime)orderDetail.OrderDetail_DateStart).TotalDays;
            if (orderDetail != null)
            {
                orderDetail.OrderDetail_Status = 3;
            }

            // Trừ total cost của order liên quan đến orderdetail bị xóa.
            Order order = db.Orders.Where(x => x.Id == orderDetail.OrdersId).FirstOrDefault();
            order.Order_TotalCost = order.Order_TotalCost - (orderDetail.Service.Service_Price * orderDetail.OrderDetail_NumberOfPeople * interval);
            if (order.Order_TotalCost < 0)
            {
                order.Order_TotalCost = 0;
            }
            // Xóa các bản ghi trong bảng Staff_OrderDetail để cập nhật nhân sự còn đang rảnh.
            List<Staff_OrderDetail> list_staff_ord = db.Staff_OrderDetail.Where(x => x.OrderDetail_Id == orderDetail.Id).ToList();
            foreach (var items in list_staff_ord)
            {
                db.Staff_OrderDetail.Remove(items);
            }


            db.SaveChanges();
            return Json(orderDetail.Id, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "OrderDetail_Reset")]
        [HttpGet]
        public JsonResult Reset(int ord_id)
        {
            OrderDetail orderDetail = db.OrderDetails.Where(x => x.Id == ord_id).FirstOrDefault();
            if (orderDetail != null)
            {
                orderDetail.OrderDetail_Status = 0;
            }
            db.SaveChanges();
            return Json(orderDetail.Id, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "OrderDetail_List")]
        [HttpGet]
        public JsonResult GetOrderDetail(int Service_ID)
        {

            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.ServiceId == Service_ID && x.OrderDetail_Status != 3).OrderBy(x => x.OrderDetail_Status).ToList();
            List<OrderDetailViewModel> list_ord_vm = new List<OrderDetailViewModel>();
            foreach (var item in list_ord)
            {
                OrderDetailViewModel _new = new OrderDetailViewModel();
                _new.OrderDetail_DateStart = (DateTime)item.OrderDetail_DateStart;
                _new.OrderDetail_DateEnd = (DateTime)item.OrderDetail_DateEnd;
                _new.OrderDetail_NumberOfPeople = item.OrderDetail_NumberOfPeople;
                _new.OrderDetail_Status = item.OrderDetail_Status;
                _new.Id = item.Id;
                _new.OrdersId = item.OrdersId;
                _new.ServiceId = item.ServiceId;
                list_ord_vm.Add(_new);
            }
            //string json_convert = JsonConvert.SerializeObject(list_ord_vm);
            return Json(list_ord_vm, JsonRequestBehavior.AllowGet);

        }

        [HasPermission(Permission = "Service_Add")]
        public ActionResult Add()
        {

            return View();


        }

        [HasPermission(Permission = "Service_Add")]
        public ActionResult Add_Submit(Service request, HttpPostedFileBase FeatureImage, HttpPostedFileBase[] Service_AdditionalImage)
        {

            // Create Service and add Feature Image
            Service newService = new Service();
            newService.Service_Description = request.Service_Description;
            newService.Service_Name = request.Service_Name;
            newService.Service_Price = request.Service_Price;
            newService.Service_PriceSale = request.Service_PriceSale;
            newService.Service_SaleStatus = request.Service_SaleStatus;
            if (FeatureImage != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(FeatureImage.FileName);
                string Extension = Path.GetExtension(FeatureImage.FileName);
                FileName = FileName + Extension;
                newService.Service_Image = "/Public/Image/" + FileName;

                FeatureImage.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
            }
            db.Services.Add(newService);

            // Add Image for Service

            foreach (HttpPostedFileBase file in Service_AdditionalImage)
            {
                //Checking file is available to save.  
                if (file != null)
                {
                    Image newImage = new Image();
                    string filename = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    filename = filename + extension;

                    newImage.Image_link = "/Public/Image/" + filename;
                    file.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), filename));
                    newImage.ServiceId = newService.Id;
                    db.Images.Add(newImage);
                }
            }
            db.SaveChanges();
            return RedirectToAction("ServiceIndex");


        }

        [HasPermission(Permission = "Service_Edit")]
        public ActionResult Edit_Submit(Service request, HttpPostedFileBase FeatureImage, HttpPostedFileBase[] Service_AdditionalImage)
        {

            // Create Service and add Feature Image
            Service service = db.Services.Where(x => x.Id == request.Id).FirstOrDefault();
            if (service != null)
            {
                if (Service_AdditionalImage[0] == null)
                {

                }
                else
                {
                    //Remove all Image Detail in table Image of current Service after update image
                    List<Image> list_image = db.Images.Where(x => x.ServiceId == service.Id).ToList();
                    foreach (var item in list_image)
                    {
                        db.Images.Remove(item);
                    }
                    // Add Image for Service
                    foreach (HttpPostedFileBase file in Service_AdditionalImage)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {
                            Image newImage = new Image();
                            string filename = Path.GetFileNameWithoutExtension(file.FileName);
                            string extension = Path.GetExtension(file.FileName);
                            filename = filename + extension;

                            newImage.Image_link = "/Public/Image/" + filename;
                            file.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), filename));
                            newImage.ServiceId = service.Id;
                            db.Images.Add(newImage);
                        }
                    }
                }

                service.Service_Description = request.Service_Description;
                service.Service_Name = request.Service_Name;
                service.Service_Price = request.Service_Price;
                service.Service_PriceSale = request.Service_PriceSale;
                service.Service_SaleStatus = request.Service_SaleStatus;
                if (FeatureImage != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(FeatureImage.FileName);
                    string Extension = Path.GetExtension(FeatureImage.FileName);
                    FileName = FileName + Extension;
                    service.Service_Image = "/Public/Image/" + FileName;

                    FeatureImage.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
                }

                db.SaveChanges();
                return RedirectToAction("ServiceIndex");
            }
            else
            {
                return RedirectToAction("ServiceIndex");
            }



        }

        [HasPermission(Permission = "Service_Edit")]
        public ActionResult Edit(int id)
        {
            Service edit_service = db.Services.Where(x => x.Id == id).FirstOrDefault();
            return View(edit_service);
        }

        [HasPermission(Permission = "Service_Delete")]
        [HttpGet]
        public JsonResult Delete(int ServiceId)
        {

            Service service = db.Services.Where(x => x.Id == ServiceId).FirstOrDefault();
            List<Image> service_image = db.Images.Where(x => x.ServiceId == ServiceId).ToList();
            foreach (var item in service_image)
            {
                db.Images.Remove(item);
            }
            db.Services.Remove(service);
            db.SaveChanges();
            return Json("/Admin/ServiceManagement/ServiceIndex", JsonRequestBehavior.AllowGet);

        }
    }
}