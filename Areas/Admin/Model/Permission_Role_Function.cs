using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.Model
{
    public class Permission_Role_Function
    {
        private Entities db = new Entities();
        public List<string> TakePermission(UserInFo user)
        {
            var list_permission_of_current_user = (from a in db.UserInFoes
                                                   join b in db.UserRoles on a.Id equals b.UserId
                                                   join c in db.PermissionRoles on b.Id equals c.RoleId
                                                   join d in db.Permissions on c.PermissionId equals d.Id
                                                   where a.Id == user.Id
                                                   select new
                                                   {
                                                       Permission_Name = d.Permission_Name
                                                   }).ToList();
            List<string> list_permission = new List<string>();
            foreach (var item in list_permission_of_current_user)
            {
                list_permission.Add(item.Permission_Name);
            }
            return list_permission;
        }
        public List<string> TakeRole(UserInFo user)
        {
            var list_role_of_current_user = (from a in db.UserInFoes
                                                   join b in db.UserRoles on a.Id equals b.UserId
                                                   join c in db.Roles on b.RoleId equals c.Id
                                                   where a.Id == user.Id
                                                   select new
                                                   {
                                                       Role_Name = c.Role_Name
                                                   }).ToList();
            List<string> list_role = new List<string>();
            foreach (var item in list_role_of_current_user)
            {
                list_role.Add(item.Role_Name);
            }
            return list_role;
        }
    }
}