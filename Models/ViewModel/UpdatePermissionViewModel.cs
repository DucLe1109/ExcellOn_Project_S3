using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Models.ViewModel
{
    public class UpdatePermissionViewModel
    {
        public int RoleId { get; set; }
        public int[] List_Permission_Id { get; set; }
    }
}