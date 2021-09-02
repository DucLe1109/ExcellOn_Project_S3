using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class StaffManagementController : BaseController
    {

        private Entities db = new Entities();

        [HasPermission(Permission = "Staff_List")]
        public ActionResult StaffIndex()
        {
            var list_staff = db.Staffs.Where(x=>x.Deleted != 1).ToList();
            return View(list_staff);
        }

        [HasPermission(Permission = "Staff_List")]
        public ActionResult StaffDeletedIndex()
        {
            var list_staff = db.Staffs.Where(x=>x.Deleted == 1).ToList();
            return View(list_staff);
        }

        [HasPermission(Permission = "Staff_List")]
        public ActionResult StaffFreeIndex()
        {
            DateTime today = DateTime.Now;
            DateTime future = today.AddDays(100);
            OrderDetail_Function orderDetail_Function = new OrderDetail_Function();
            List<Staff> list_staff_free_inbound = orderDetail_Function.Take_List_Staff_Free(1, today, future);
            List<Staff> list_staff_free_outbound = orderDetail_Function.Take_List_Staff_Free(2, today, future);
            List<Staff> list_staff_free_telemarketing = orderDetail_Function.Take_List_Staff_Free(3, today, future);

            List<Staff> list_staff_free = new List<Staff>();
            list_staff_free.AddRange(list_staff_free_inbound);
            list_staff_free.AddRange(list_staff_free_outbound);
            list_staff_free.AddRange(list_staff_free_telemarketing);

            ViewBag.list_staff_free = list_staff_free;
            return View();
        }

        [HasPermission(Permission = "Staff_List")]
        public ActionResult StaffInWorkingIndex()
        {

            List<Staff> _list_staff = db.Staffs.Where(x => x.ServiceId != null).ToList();

            DateTime today = DateTime.Now;
            DateTime future = today.AddDays(100);
            OrderDetail_Function orderDetail_Function = new OrderDetail_Function();
            List<Staff> list_staff_free_inbound = orderDetail_Function.Take_List_Staff_Free(1, today, future);
            List<Staff> list_staff_free_outbound = orderDetail_Function.Take_List_Staff_Free(2, today, future);
            List<Staff> list_staff_free_telemarketing = orderDetail_Function.Take_List_Staff_Free(3, today, future);

            List<Staff> list_staff_free = new List<Staff>();
            list_staff_free.AddRange(list_staff_free_inbound);
            list_staff_free.AddRange(list_staff_free_outbound);
            list_staff_free.AddRange(list_staff_free_telemarketing);

            List<Staff> list_staff_working = new List<Staff>();
            foreach (var item in _list_staff)
            {
                if (list_staff_free.Exists(x => x.Id == item.Id) == false)
                {
                    list_staff_working.Add(item);
                }
            }
            ViewBag.list_staff_working = list_staff_working;
            return View();
        }

        [HasPermission(Permission = "Staff_Add")]
        [HttpGet]
        public ActionResult Add()
        {
            var list_service = db.Services.ToList();
            ViewBag.list_service = list_service;
            return View();

        }

        [HasPermission(Permission = "Staff_Add")]
        [HttpPost]
        public ActionResult Add(Staff request, HttpPostedFileBase AvatarUpload)
        {

            Staff new_staff = new Staff();
            new_staff.Staff_Age = request.Staff_Age;
            new_staff.Staff_Email = request.Staff_Email;
            new_staff.Staff_FullName = request.Staff_FullName;
            new_staff.Staff_Gender = request.Staff_Gender;
            new_staff.ServiceId = request.ServiceId;
            if (request.Staff_Password != null)
            {
                new_staff.Staff_Password = BCrypt.Net.BCrypt.HashPassword(request.Staff_Password);
            }
            else
            {
                new_staff.Staff_Password = null;
            }

            new_staff.Staff_Phone = request.Staff_Phone;
            new_staff.Staff_Address = request.Staff_Address;
            new_staff.Staff_AboutMe = request.Staff_AboutMe;
            if (AvatarUpload != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(AvatarUpload.FileName);
                string Extension = Path.GetExtension(AvatarUpload.FileName);
                FileName = FileName + Extension;
                new_staff.Staff_Avatar = "/Public/Image/" + FileName;
                AvatarUpload.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
            }
            else
            {
                new_staff.Staff_Avatar = null;
            }
            db.Staffs.Add(new_staff);
            db.SaveChanges();
            TempData["message"] = "Create successfully";
            return Redirect("/Admin/StaffManagement/StaffIndex");

        }

        [HasPermission(Permission = "Staff_Edit")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var list_service = db.Services.ToList();
            ViewBag.list_service = list_service;
            Staff staff = db.Staffs.Where(x => x.Id == id).FirstOrDefault();
            return View(staff);
        }

        [HasPermission(Permission = "Staff_Edit")]
        //Function Update information for User
        [HttpPost]

        [HasPermission(Permission = "Staff_Edit")]
        public ActionResult Update_Profile(Staff CurrentStaff, HttpPostedFileBase AvatarUpload)
        {
            var Staff = db.Staffs.Where(x => x.Staff_UserName == CurrentStaff.Staff_UserName).FirstOrDefault();
            if (Staff != null)
            {
                Staff.Staff_Age = CurrentStaff.Staff_Age;
                Staff.Staff_Email = CurrentStaff.Staff_Email;
                Staff.Staff_FullName = CurrentStaff.Staff_FullName;
                Staff.Staff_Gender = CurrentStaff.Staff_Gender;
                Staff.ServiceId = CurrentStaff.ServiceId;
                if (CurrentStaff.Staff_Password != null)
                {
                    Staff.Staff_Password = BCrypt.Net.BCrypt.HashPassword(CurrentStaff.Staff_Password);
                }
                else
                {
                    Staff.Staff_Password = Staff.Staff_Password;
                }

                Staff.Staff_Phone = CurrentStaff.Staff_Phone;
                Staff.Staff_Address = CurrentStaff.Staff_Address;
                Staff.Staff_AboutMe = CurrentStaff.Staff_AboutMe;
                if (AvatarUpload != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(AvatarUpload.FileName);
                    string Extension = Path.GetExtension(AvatarUpload.FileName);
                    FileName = FileName + Extension;
                    Staff.Staff_Avatar = "/Public/Image/" + FileName;
                    AvatarUpload.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
                }
                else
                {
                    Staff.Staff_Avatar = Staff.Staff_Avatar;
                }

                db.SaveChanges();
                TempData["message"] = "Update successfully";
            }
            return Redirect("/Admin/StaffManagement/StaffIndex");


        }

        [HasPermission(Permission = "Staff_Delete")]
        [HttpGet]
        public JsonResult Delete(int StaffId)
        {
            Staff staff = db.Staffs.Where(x => x.Id == StaffId).FirstOrDefault();
            if (staff != null)
            {
                staff.Deleted = 1;
                db.SaveChanges();
                return Json("Deleted", JsonRequestBehavior.AllowGet);
            }
            return Json(404, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Reset(int StaffId)
        {
            Staff staff = db.Staffs.Where(x => x.Id == StaffId).FirstOrDefault();
            if (staff != null)
            {
                staff.Deleted = 0;
                db.SaveChanges();
                return Json("Deleted", JsonRequestBehavior.AllowGet);
            }
            return Json(404, JsonRequestBehavior.AllowGet);
        }
        
        [HasPermission(Permission = "Staff_Status")]
        public JsonResult ChangeStatus(int StaffId)
        {

            Staff staff = db.Staffs.Where(x => x.Id == StaffId).FirstOrDefault();
            if (staff.Staff_Status == 0)
            {
                staff.Staff_Status = 1;
                db.SaveChanges();
                return Json("status_1", JsonRequestBehavior.AllowGet);
            }
            else
            {
                staff.Staff_Status = 0;
                db.SaveChanges();
                return Json("status_0", JsonRequestBehavior.AllowGet);
            }

        }

        
    }
}