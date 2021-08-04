using _ExcellOn_.Models;
using _ExcellOn_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class RoleManagementController : BaseController
    {
        private Entities db = new Entities();
        // GET: Admin/RoleManagement
        public ActionResult RoleIndex()
        {
            Session["return_url"] = "/Admin/RoleManagement/RoleIndex";
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;

                List<Permission> list_permission = db.Permissions.ToList();
                ViewBag.list_permission = list_permission;
                
                List<PermissionRole> list_permission_roles = db.PermissionRoles.ToList();
                ViewBag.list_permission_roles = list_permission_roles;

                List<Role> list_role = db.Roles.ToList();
                return View(list_role);
            }
            else
            {
                return Redirect("/AdminLogin");
            }
        }
        public JsonResult UpdatePermission(UpdatePermissionViewModel request)
        {

            if (request != null && request.List_Permission_Id != null)
            {
                List<PermissionRole> list_permissionRole = db.PermissionRoles.Where(x => x.RoleId == request.RoleId).ToList();
                foreach (var item in list_permissionRole)
                {
                    db.PermissionRoles.Remove(item);
                }
                int[] List_Permission_Id = request.List_Permission_Id;
                int RoleId = request.RoleId;
                for (int i = 0; i < List_Permission_Id.Length; i++)
                {
                    PermissionRole new_permissionRole = new PermissionRole();
                    new_permissionRole.RoleId = RoleId;
                    new_permissionRole.PermissionId = List_Permission_Id[i];
                    db.PermissionRoles.Add(new_permissionRole);
                }
                db.SaveChanges();
                return Json("Update Successfully", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Request is error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(Role roleObj)
        {
            Session["return_url"] = "/Admin/RoleManagement/Add";
            if (check_auth())
            {
                Role newRole = new Role();
                newRole.Role_Name = roleObj.Role_Name;
                newRole.Role_Description = roleObj.Role_Description;
                db.Roles.Add(newRole);
                db.SaveChanges();
                return Json("/Admin/RoleManagement/RoleIndex", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetById(int RoleId)
        {
            Session["return_url"] = "/Admin/RoleManagement/GetById?RoleId=" + RoleId;
            if (check_auth())
            {
                Role role = db.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
                return Json(role, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(Role role_request)
        {
            Session["return_url"] = "/Admin/RoleManagement/Update";
            if (check_auth())
            {
                Role role = db.Roles.Where(x => x.Id == role_request.Id).FirstOrDefault();
                role.Role_Name = role_request.Role_Name;
                role.Role_Description = role_request.Role_Description;
                db.SaveChanges();
                return Json("/Admin/RoleManagement/RoleIndex", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Delete(int RoleId)
        {
            Session["return_url"] = "/Admin/RoleManagement/Delete?RoleId=" + RoleId;
            if (check_auth())
            {
                Role role = db.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
                db.Roles.Remove(role);
                db.SaveChanges();
                return Json("/Admin/RoleManagement/RoleIndex", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }
    }
}