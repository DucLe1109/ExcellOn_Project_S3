using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Models.ViewModel
{
    public class SearchRequest2
    {
        public List<int> List_Service_Id { get; set; }
        public DateTime Date_Start { get; set; }
        public DateTime Date_End { get; set; }
    }
}