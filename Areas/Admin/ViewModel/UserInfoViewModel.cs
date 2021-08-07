using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class UserInfoViewModel
    {
        public int[] RoleId { get; set; }
        public int Id { get; set; }

        public string User_Name { get; set; }

        public string User_Password { get; set; }

        public string User_Email { get; set; }

        public Nullable<int> User_Gender { get; set; }

        public Nullable<int> User_Age { get; set; }

        public string User_Phone { get; set; }

        public string User_Address { get; set; }

        public string User_Avatar { get; set; }

        public string User_FullName { get; set; }

        public string User_AboutMe { get; set; }
    }
}