using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        public string Customer_Name { get; set; }

        public string Customer_Phone { get; set; }

        public string Customer_Email { get; set; }
        public int Customer_Gender { get; set; }

        public string Customer_UserName { get; set; }

        public string Customer_Password { get; set; }

        public Nullable<int> CompanyId { get; set; }

        public string Customer_Comment { get; set; }

        public string Customer_Avatar { get; set; }
    }
}