using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        //C1: chưa tối ưu
        //public BaseController()
        //{
        //    if (System.Web.HttpContext.Current.Session["UserName"] == null)
        //    {
        //        System.Web.HttpContext.Current.Response.Redirect("/AdminLogin");
        //    }
        //}


        // C2
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Session["UserName"];
            string url = (string)Session["return_url"];
            if (session != null)
            {

            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "User",
                    action = "Login",
                    Area = "Admin"
                }));
            }
            base.OnActionExecuting(filterContext);
        }

        //protected bool check_auth()
        //{
        //    var session = Session["UserName"];
        //    if (session == null)
        //    {
        //        return false;
        //    }return true;
        //}

    }
}