using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Areas.Admin.ViewModel;
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
        [HasPermission(Permission ="Admin")]
        public ActionResult RoleIndex()
        {
            List<Permission> list_permission = db.Permissions.ToList();
            ViewBag.list_permission = list_permission;

            List<PermissionRole> list_permission_roles = db.PermissionRoles.ToList();
            ViewBag.list_permission_roles = list_permission_roles;

            List<Role> list_role = db.Roles.ToList();
            return View(list_role);

        }        
        
        // 
        [HasPermission(Permission ="Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Role current_role = db.Roles.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.current_role = current_role;

            List<Permission> list_permission = db.Permissions.ToList();
            ViewBag.list_permission = list_permission;

            List<PermissionRole> list_permission_roles = db.PermissionRoles.ToList();
            ViewBag.list_permission_roles = list_permission_roles;

            return View();

        }
        [HasPermission(Permission = "Admin")]
        [HttpPost]
        public ActionResult Submit_Edit(UpdatePermissionViewModel request)
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
                TempData["message"] = "Change Successfully !";
                return RedirectToAction("RoleIndex");
            }
            else if (request.List_Permission_Id == null)
            {
                List<PermissionRole> list_permissionRole = db.PermissionRoles.Where(x => x.RoleId == request.RoleId).ToList();
                foreach (var item in list_permissionRole)
                {
                    db.PermissionRoles.Remove(item);
                }
                db.SaveChanges();
                TempData["message"] = "Change Successfully !";
                return RedirectToAction("RoleIndex");
            }
            else
            {
                TempData["message"] = "Change Successfully !";
                return RedirectToAction("RoleIndex");
            }

        }
        //
        [HasPermission(Permission = "Admin")]
        [HttpPost]
        public JsonResult Add(Role roleObj)
        {

            Role newRole = new Role();
            newRole.Role_Name = roleObj.Role_Name;
            newRole.Role_Description = roleObj.Role_Description;
            db.Roles.Add(newRole);
            db.SaveChanges();
            return Json("/Admin/RoleManagement/RoleIndex", JsonRequestBehavior.AllowGet);

        }

        //
        [HasPermission(Permission = "Admin")]
        [HttpGet]
        public JsonResult GetById(int RoleId)
        {
            Role role = db.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
            RoleViewModel viewModel = new RoleViewModel();
            viewModel.Id = role.Id;
            viewModel.Role_Name = role.Role_Name;
            viewModel.Role_Description = role.Role_Description;
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        //
        [HasPermission(Permission = "Admin")]
        [HttpPost]
        public JsonResult Update(Role role_request)
        {

            Role role = db.Roles.Where(x => x.Id == role_request.Id).FirstOrDefault();
            role.Role_Name = role_request.Role_Name;
            role.Role_Description = role_request.Role_Description;
            db.SaveChanges();
            return Json("/Admin/RoleManagement/RoleIndex", JsonRequestBehavior.AllowGet);

        }

        //
        [HasPermission(Permission = "Admin")]
        [HttpGet]
        public JsonResult Delete(int RoleId)
        {
            var user_role = db.UserRoles.Where(x => x.RoleId == RoleId).ToList();
            foreach(var item in user_role)
            {
                db.UserRoles.Remove(item);
            }
            Role role = db.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
            db.Roles.Remove(role);
            db.SaveChanges();
            return Json("/Admin/RoleManagement/RoleIndex", JsonRequestBehavior.AllowGet);

        }
    }
}