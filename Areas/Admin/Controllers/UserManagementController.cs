﻿using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class UserManagementController : BaseController
    {

        private Entities db = new Entities();

        //Function get Information for CurrentUser
        public ActionResult MyProfile()
        {
            Session["return_url"] = "/Admin/UserManagement/MyProfile";
            if (check_auth())
            {
                
                string url = (string)Session["return_url"];
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                var CurrentService = db.Services.Where(x => x.Id == CurrentUser.ServiceId).FirstOrDefault();
                var list_service = db.Services.ToList();
                ViewBag.list_service = list_service;
                ViewBag.CurrentService = CurrentService;
                ViewBag.CurrentUser = CurrentUser;
                return View(CurrentUser);
            }
            else
            {
                return RedirectToRoute("AdminLogin");
            }
        }

        //Function Update information for User
        [HttpPost]
        public ActionResult Update_Profile(UserInFo CurrentUser, HttpPostedFileBase AvatarUpload)
        {
            Session["return_url"] = "/Admin/UserManagement/MyProfile";
            if (check_auth())
            {
                var User = db.UserInFoes.Where(x => x.User_Name == CurrentUser.User_Name).FirstOrDefault();
                if (User != null)
                {
                    User.User_Age = CurrentUser.User_Age;
                    User.User_Email = CurrentUser.User_Email;
                    User.User_FullName = CurrentUser.User_FullName;
                    User.User_Gender = CurrentUser.User_Gender;
                    User.User_Password = BCrypt.Net.BCrypt.HashPassword(CurrentUser.User_Password);
                    User.User_Phone = CurrentUser.User_Phone;
                    User.User_Address = CurrentUser.User_Address;
                    User.User_AboutMe = CurrentUser.User_AboutMe;
                    User.ServiceId = CurrentUser.ServiceId;
                    if (AvatarUpload != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(AvatarUpload.FileName);
                        string Extension = Path.GetExtension(AvatarUpload.FileName);
                        FileName = FileName + Extension;
                        User.User_Avatar = "~/Public/Image/" + FileName;

                        //string fullPath = Request.MapPath("~/Public/Image/" + FileName);
                        //if (System.IO.File.Exists(fullPath))
                        //{
                        //    System.IO.File.Delete(fullPath);
                        //}

                        AvatarUpload.SaveAs(Path.Combine(Server.MapPath("~/Public/Image/"), FileName));
                    }else
                    {
                        User.User_Avatar = User.User_Avatar;
                    }

                    db.SaveChanges();
                }
                return RedirectToAction("MyProfile");
            }else
            {
                return Redirect("AdminLogin");
            }
           
        }
    }
}