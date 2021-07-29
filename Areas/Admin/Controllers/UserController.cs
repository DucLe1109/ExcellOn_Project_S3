using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private Entities db = new Entities();

        // Function Login View
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // Function Login handle request
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel.UserName != null && loginViewModel.Password != null)
            {
                var User = db.UserInFoes.Where(x => x.User_Name == loginViewModel.UserName).FirstOrDefault();
                if (User != null)
                {
                    bool test = BCrypt.Net.BCrypt.Verify(loginViewModel.Password, User.User_Password);
                    if (test)
                    {
                        Session["UserName"] = loginViewModel.UserName;
                        return RedirectToAction("Index", "AdminHome");
                    }
                    else
                    {
                        ViewBag.message = "Account or Password is not correct !";
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "Account or Password is not correct !";
                    return View();
                }
            }
            else
            {
                ViewBag.message = "Account or Password is null !";
                return View();
            }
        }

        //Function get Information for CurrentUser
        public ActionResult MyProfile()
        {
            string UserName = (string)Session["UserName"];
            var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
            if (CurrentUser != null)
            {
                var CurrentService = db.Services.Where(x => x.Id == CurrentUser.ServiceId).FirstOrDefault();
                var list_service = db.Services.ToList();
                ViewBag.list_service = list_service;
                ViewBag.CurrentService = CurrentService;
                ViewBag.CurrentUser = CurrentUser;
                return View(CurrentUser);
            }
            else
            {
                return Redirect("/Admin/User/Login");
            }
        }

        //Function Update information for User
        [HttpPost]
        public ActionResult Update_Profile(UserInFo CurrentUser)
        {
            var User = db.UserInFoes.Where(x => x.User_Name == CurrentUser.User_Name).FirstOrDefault();
            if (User != null)
            {
                User.User_Age = CurrentUser.User_Age;
                User.User_Email = CurrentUser.User_Email;
                User.User_FullName = CurrentUser.User_FullName;
                User.User_Gender = CurrentUser.User_Gender;
                User.User_Password = CurrentUser.User_Password;
                User.User_Phone = CurrentUser.User_Phone;
                User.User_Address = CurrentUser.User_Address;
                User.User_AboutMe = CurrentUser.User_AboutMe;
                User.ServiceId = CurrentUser.ServiceId;
                if (CurrentUser.AvatarUpload != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(CurrentUser.AvatarUpload.FileName);
                    string Extension = Path.GetExtension(CurrentUser.AvatarUpload.FileName);
                    FileName = FileName + Extension;
                    User.User_Avatar = "~/Public/Image/" + FileName;

                    //string fullPath = Request.MapPath("~/Public/Image/" + FileName);
                    //if (System.IO.File.Exists(fullPath))
                    //{
                    //    System.IO.File.Delete(fullPath);
                    //}

                    CurrentUser.AvatarUpload.SaveAs(Path.Combine(Server.MapPath("~/Public/Image/"), FileName));
                }

                db.SaveChanges();
            }
            return RedirectToAction("MyProfile");
        }

        //function Logout User 
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

    }
}