using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class StaffViewModel
    {
        public int Id { get; set; }

        public string Staff_UserName { get; set; }

        public string Staff_Password { get; set; }

        public string Staff_Email { get; set; }

        public Nullable<int> Staff_Gender { get; set; }

        public Nullable<int> Staff_Age { get; set; }

        public Nullable<int> Staff_Status { get; set; }

        public string Staff_Phone { get; set; }

        public string Staff_Address { get; set; }

        public string Staff_Avatar { get; set; }

        public string Staff_FullName { get; set; }

        public string Staff_AboutMe { get; set; }

        public Nullable<int> ServiceId { get; set; }
    }
}