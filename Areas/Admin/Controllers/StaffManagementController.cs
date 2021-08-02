﻿using _ExcellOn_.Models;
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

        public ActionResult StaffIndex()
        {
            Session["return_url"] = "/Admin/StaffManagement/StaffIndex";
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;

                var list_staff = db.Staffs.ToList();
                ViewBag.list_staff = list_staff;
                return View();
            }
            else
            {
                return Redirect("/AdminLogin");
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            Session["return_url"] = "/Admin/StaffManagement/Add";
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;
                var list_service = db.Services.ToList();
                ViewBag.list_service = list_service;
                return View();
            }
            else
            {
                return Redirect("/AdminLogin");
            }
        }
        [HttpPost]
        public ActionResult Add(Staff request,HttpPostedFileBase AvatarUpload)
        {
            Session["return_url"] = "/Admin/StaffManagement/Add";
            if (check_auth())
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

                return Redirect("/Admin/StaffManagement/StaffIndex");
            }
            else
            {
                return Redirect("/AdminLogin");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Session["return_url"] = "/Admin/StaffManagement/Edit/" + id;
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;
                var list_service = db.Services.ToList();
                ViewBag.list_service = list_service;
                Staff staff = db.Staffs.Where(x => x.Id == id).FirstOrDefault();
                return View(staff);
            }
            else
            {
                return Redirect("/AdminLogin");
            }

        }

        //Function Update information for User
        [HttpPost]
        public ActionResult Update_Profile(Staff CurrentStaff, HttpPostedFileBase AvatarUpload)
        {
            Session["return_url"] = "/Admin/StaffManagement/Update_Profile";
            if (check_auth())
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
                }
                return Redirect("/Admin/StaffManagement/StaffIndex");
            }
            else
            {
                return Redirect("/AdminLogin");
            }

        }

        [HttpGet]
        public JsonResult Delete(int StaffId)
        {
            Session["return_url"] = "/Admin/ServiceManagement/Delete?StaffId=" + StaffId;
            if (check_auth())
            {
                Staff staff = db.Staffs.Where(x => x.Id == StaffId).FirstOrDefault();
                
                db.Staffs.Remove(staff);
                db.SaveChanges();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }
    }
}