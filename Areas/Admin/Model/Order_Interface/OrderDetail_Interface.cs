using _ExcellOn_.Models;
using _ExcellOn_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ExcellOn_.Areas.Admin.Model.Order_Interface
{
    public interface OrderDetail_Interface
    {
        List<Staff> Take_List_Staff_Free(int ord_id);
        List<Staff> Take_List_Staff_Free(SearchRequest searchRequest);
        List<Staff> Take_List_Staff_Free(int _Service_Id, DateTime _Date_Start, DateTime _Date_End);
    }
}
