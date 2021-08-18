using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _ExcellOn_.Controllers
{
    public class CustomerBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Session["CustomerName"];
            if (session != null)
            {

            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Customer",
                    action = "Login"
                }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}