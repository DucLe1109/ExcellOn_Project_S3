using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Models;
using _ExcellOn_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class SearchesController : Controller
    {
        // GET: Searches
        private Entities db = new Entities();
        private OrderDetail_Function orderDetail_Function = new OrderDetail_Function();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(SearchRequest2 searchRequest)
        {
            List<List_Staff_Free_Of_Service> list_Staff_Free_Of_Services = new List<List_Staff_Free_Of_Service>();
            DateTime Date_Start = searchRequest.Date_Start;
            DateTime Date_End = searchRequest.Date_End;
            List<int> list_service_id = searchRequest.List_Service_Id;
            if (list_service_id != null)
            {
                foreach (var item in list_service_id)
                {
                    Service service = db.Services.Where(x => x.Id == item).FirstOrDefault();
                    List<Staff> list_staff_free = orderDetail_Function.Take_List_Staff_Free(item,Date_Start,Date_End);
                    List_Staff_Free_Of_Service _new = new List_Staff_Free_Of_Service();
                    if (list_staff_free != null)
                    {
                        _new.list_staff_free_count = list_staff_free.Count;
                        _new.service = service;
                        _new.Date_Start = Date_Start;
                        _new.Date_End = Date_End;
                    }
                    else
                    {
                        _new.list_staff_free_count = 0;
                        _new.service = service;
                        _new.Date_Start = Date_Start;
                        _new.Date_End = Date_End;
                    }
                    list_Staff_Free_Of_Services.Add(_new);
                }
            }
            else
            {
                List<int> _list_service_id = new List<int>();
                _list_service_id.Add(1); _list_service_id.Add(2); _list_service_id.Add(3);
                foreach (var item in _list_service_id)
                {
                    Service service = db.Services.Where(x => x.Id == item).FirstOrDefault();
                    List<Staff> list_staff_free = orderDetail_Function.Take_List_Staff_Free(item, Date_Start, Date_End);
                    List_Staff_Free_Of_Service _new = new List_Staff_Free_Of_Service();
                    if (list_staff_free != null)
                    {
                        _new.list_staff_free_count = list_staff_free.Count;
                        _new.service = service;
                        _new.Date_Start = Date_Start;
                        _new.Date_End = Date_End;
                    }
                    else
                    {
                        _new.list_staff_free_count = 0;
                        _new.service = service;
                        _new.Date_Start = Date_Start;
                        _new.Date_End = Date_End;
                    }
                    list_Staff_Free_Of_Services.Add(_new);
                }
            }
            ViewBag.list_Staff_Free_Of_Services = list_Staff_Free_Of_Services;
            ViewBag.List_Service_Id = list_service_id;
            return View();
        }
    }
}