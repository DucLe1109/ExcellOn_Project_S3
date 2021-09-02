using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute(
                name: "AdminLogin",
                url: "AdminLogin",
                defaults: new { controller = "User", action = "Login" }
            );

            context.MapRoute(
                name: "StaffLogin",
                url: "StaffLogin",
                defaults: new { controller = "Staff", action = "Login" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "AdminHome", action = "DashboardIndex", id = UrlParameter.Optional }
            );
            
        }
    }
}