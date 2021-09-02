using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class StaffBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Session["StaffName"];
            string url = (string)Session["return_url"];
            if (session != null)
            {

            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Staff",
                    action = "Login",
                    Area = "Admin"
                }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}