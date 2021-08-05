using _ExcellOn_.Models;
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
            UserInFo user = (UserInFo)Session["UserName"];
            var list_service = db.Services.ToList();
            ViewBag.list_service = list_service;

            UserInFo _user = db.UserInFoes.Where(x=>x.Id == user.Id).FirstOrDefault();
            return View(_user);

        }

        //Function Update information for User
        [HttpPost]
        public ActionResult Update_Profile(UserInFo CurrentUser, HttpPostedFileBase AvatarUpload)
        {

            var User = db.UserInFoes.Where(x => x.User_Name == CurrentUser.User_Name).FirstOrDefault();
            if (User != null)
            {
                User.User_Age = CurrentUser.User_Age;
                User.User_Email = CurrentUser.User_Email;
                User.User_FullName = CurrentUser.User_FullName;
                User.User_Gender = CurrentUser.User_Gender;
                if (CurrentUser.User_Password != null)
                {
                    User.User_Password = BCrypt.Net.BCrypt.HashPassword(CurrentUser.User_Password);
                }
                else
                {
                    User.User_Password = User.User_Password;
                }

                User.User_Phone = CurrentUser.User_Phone;
                User.User_Address = CurrentUser.User_Address;
                User.User_AboutMe = CurrentUser.User_AboutMe;
                if (AvatarUpload != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(AvatarUpload.FileName);
                    string Extension = Path.GetExtension(AvatarUpload.FileName);
                    FileName = FileName + Extension;
                    User.User_Avatar = "/Public/Image/" + FileName;

                    //string fullPath = Request.MapPath("~/Public/Image/" + FileName);
                    //if (System.IO.File.Exists(fullPath))
                    //{
                    //    System.IO.File.Delete(fullPath);
                    //}

                    AvatarUpload.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
                }
                else
                {
                    User.User_Avatar = User.User_Avatar;
                }

                db.SaveChanges();
            }
            return RedirectToAction("MyProfile");
        }
    }
}