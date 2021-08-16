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
    public class UserManagementController : BaseController
    {

        private Entities db = new Entities();

        [HasPermission(Permission ="Admin")]
        public ActionResult UserIndex()
        {
            UserInFo user = (UserInFo)Session["UserName"];
            var list_user = db.UserInFoes.Where(x=>x.Id != user.Id).ToList();
            return View(list_user);
        }

        //Add function 
        [HasPermission(Permission = "Admin")]
        [HttpGet]
        public ActionResult AddUser()
        {
            var list_role = db.Roles.ToList();
            ViewBag.list_role = list_role;
            return View();
        } 
        
        [HasPermission(Permission = "Admin")]
        [HttpPost]
        public ActionResult Submit_Add(UserInfoViewModel new_UserInfoViewModel, HttpPostedFileBase AvatarUpload)
        {
            if (new_UserInfoViewModel != null)
            {
                UserInFo new_User = new UserInFo();
                new_User.User_Name = new_UserInfoViewModel.User_Name;
                new_User.User_Age = new_UserInfoViewModel.User_Age;
                new_User.User_Email = new_UserInfoViewModel.User_Email;
                new_User.User_FullName = new_UserInfoViewModel.User_FullName;
                new_User.User_Gender = new_UserInfoViewModel.User_Gender;
                if (new_UserInfoViewModel.User_Password != null)
                {
                    new_User.User_Password = BCrypt.Net.BCrypt.HashPassword(new_UserInfoViewModel.User_Password);
                }
                else
                {
                    new_User.User_Password = null;
                }

                new_User.User_Phone = new_UserInfoViewModel.User_Phone;
                new_User.User_Address = new_UserInfoViewModel.User_Address;
                new_User.User_AboutMe = new_UserInfoViewModel.User_AboutMe;
                if (AvatarUpload != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(AvatarUpload.FileName);
                    string Extension = Path.GetExtension(AvatarUpload.FileName);
                    FileName = FileName + Extension;
                    new_User.User_Avatar = "/Public/Image/" + FileName;

                    //string fullPath = Request.MapPath("~/Public/Image/" + FileName);
                    //if (System.IO.File.Exists(fullPath))
                    //{
                    //    System.IO.File.Delete(fullPath);
                    //}

                    AvatarUpload.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
                }
                else
                {
                    new_User.User_Avatar = null;
                }
                db.UserInFoes.Add(new_User);
                if (new_UserInfoViewModel.RoleId != null)
                {
                    foreach (var item in new_UserInfoViewModel.RoleId)
                    {
                        UserRole new_user_role = new UserRole();
                        new_user_role.UserId = new_User.Id;
                        new_user_role.RoleId = item;
                        db.UserRoles.Add(new_user_role);
                    }
                }
                
                db.SaveChanges();
                TempData["message"] = "Create successfully";
                return RedirectToAction("UserIndex");
            }
            else
            {
                TempData["message"] = "Create unsuccessfully";
                return RedirectToAction("UserIndex");
            }
        }

        //Function return view Edit profile User
        [HasPermission(Permission = "Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UserInFo user = db.UserInFoes.Where(x=>x.Id == id).FirstOrDefault();
            var list_role = db.Roles.ToList();
            var list_user_role = db.UserRoles.ToList();
            ViewBag.list_role = list_role;
            ViewBag.list_user_role = list_user_role;
            return View(user);
        }

        //Function Submit Edit profile User
        [HasPermission(Permission = "Admin")]
        [HttpPost]
        public ActionResult Submit_Edit(UserInfoViewModel new_UserInfoViewModel, HttpPostedFileBase AvatarUpload)
        {
            UserInFo user = db.UserInFoes.Where(x => x.Id == new_UserInfoViewModel.Id).FirstOrDefault();
            if (new_UserInfoViewModel != null && user != null)
            {

                user.User_Name = new_UserInfoViewModel.User_Name;
                user.User_Age = new_UserInfoViewModel.User_Age;
                user.User_Email = new_UserInfoViewModel.User_Email;
                user.User_FullName = new_UserInfoViewModel.User_FullName;
                user.User_Gender = new_UserInfoViewModel.User_Gender;
                if (new_UserInfoViewModel.User_Password != null)
                {
                    user.User_Password = BCrypt.Net.BCrypt.HashPassword(new_UserInfoViewModel.User_Password);
                }
                else
                {
                    user.User_Password = user.User_Password;
                }

                user.User_Phone = new_UserInfoViewModel.User_Phone;
                user.User_Address = new_UserInfoViewModel.User_Address;
                user.User_AboutMe = new_UserInfoViewModel.User_AboutMe;
                if (AvatarUpload != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(AvatarUpload.FileName);
                    string Extension = Path.GetExtension(AvatarUpload.FileName);
                    FileName = FileName + Extension;
                    user.User_Avatar = "/Public/Image/" + FileName;

                    //string fullPath = Request.MapPath("~/Public/Image/" + FileName);
                    //if (System.IO.File.Exists(fullPath))
                    //{
                    //    System.IO.File.Delete(fullPath);
                    //}

                    AvatarUpload.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
                }
                else
                {
                    user.User_Avatar = user.User_Avatar;
                }
                if (new_UserInfoViewModel.RoleId != null)
                {
                    var list_user_role = db.UserRoles.Where(x => x.UserId == new_UserInfoViewModel.Id).ToList();
                    foreach (var role in list_user_role)
                    {
                        db.UserRoles.Remove(role);
                    }
                    foreach (var item in new_UserInfoViewModel.RoleId)
                    {
                        UserRole new_user_role = new UserRole();
                        new_user_role.UserId = user.Id;
                        new_user_role.RoleId = item;
                        db.UserRoles.Add(new_user_role);
                    }
                }

                db.SaveChanges();
                TempData["message"] = "Update successfully";
                return RedirectToAction("UserIndex");
            }
            else
            {
                TempData["message"] = "Update successfully";
                return RedirectToAction("UserIndex");
            }
        }

        //Function Submit Delete User
        [HasPermission(Permission = "Admin")]
        [HttpGet]
        public JsonResult Delete(int UserId)
        {
            List<UserRole> list_user_role = db.UserRoles.Where(x => x.UserId == UserId).ToList();
            if (list_user_role != null)
            {
                foreach (var item in list_user_role)
                {
                    db.UserRoles.Remove(item);
                }
            }
            UserInFo user = db.UserInFoes.Where(x => x.Id == UserId).FirstOrDefault();
            db.UserInFoes.Remove(user);
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);

        }

        //Function get Information for CurrentUser
        public ActionResult MyProfile()
        {
            var list_role = db.Roles.ToList();
            ViewBag.list_role = list_role;
            UserInFo user = (UserInFo)Session["UserName"];

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