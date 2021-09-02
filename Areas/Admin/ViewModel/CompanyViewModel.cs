using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.ViewModel
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        public string Company_Name { get; set; }

        public string Company_Phone { get; set; }

        public string Company_Address { get; set; }

        public string Company_Email { get; set; }

        public string Company_Description { get; set; }
    }
}