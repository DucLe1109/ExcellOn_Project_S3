using _ExcellOn_.Areas.Admin.ViewModel;
using _ExcellOn_.Models;
using System.IO;
using System.Linq;
using System.Web;
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
                        string return_url = (string)Session["return_url"];
                        if (return_url != null)
                        {
                            return Redirect(return_url);
                        }
                        else
                        {
                            return RedirectToAction("DashboardIndex", "AdminHome");
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

        //function Logout User 
        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/AdminLogin");
        }

    }
}