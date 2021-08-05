using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Model
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }
        public HasPermissionAttribute()
        {
            this.Permission = Permission;
        }
       
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session_user = HttpContext.Current.Session["UserName"];
            if (session_user == null)
            {
                return false;
            }
            List<string> permissions = this.getPermission();
            List<string> roles = this.getRole();
            if (permissions.Contains(this.Permission) || roles.Contains("Admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected List<string> getPermission()
        {
            var list_permission = (List<string>)HttpContext.Current.Session["ListPermission"];
            return list_permission;
        }
        protected List<string> getRole()
        {
            var list_role = (List<string>)HttpContext.Current.Session["ListRole"];
            return list_role;
        }
    }
}