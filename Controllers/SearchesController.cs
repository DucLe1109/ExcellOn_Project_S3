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
            List<List_Staff_Free_Of_Service> reference_list_Staff_Free_Of_Services = new List<List_Staff_Free_Of_Service>();
            DateTime Date_Start = searchRequest.Date_Start;
            DateTime Date_End = searchRequest.Date_End;
            int year_start_default = Date_Start.Year;
            int year_end_default = Date_End.Year;
            List<int> list_service_id = searchRequest.List_Service_Id;

            if (year_start_default != 1 && year_end_default != 1)
            {
                int interval = (int)(Date_End - Date_Start).TotalDays;

                DateTime Date_Start_Plus = Date_End;
                DateTime Date_End_Plus = Date_Start_Plus.AddDays(interval);

                DateTime Date_Start_Substract = new DateTime();
                DateTime Date_End_Substract = new DateTime();



                int current_interval = (int)(Date_Start - DateTime.Now).TotalDays;
                int _current_interval = (int)(Date_Start.AddDays(-interval) - DateTime.Now).TotalDays;
                if (current_interval > 0 && _current_interval > 0)
                {
                    Date_End_Substract = Date_Start;
                    Date_Start_Substract = Date_End_Substract.AddDays(-interval);
                }
                else
                {
                    Date_Start_Substract = Date_End_Plus;
                    Date_End_Substract = Date_Start_Substract.AddDays(interval);
                }
                
                if (list_service_id != null)
                {
                    foreach (var item in list_service_id)
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

                        List<Staff> reference_list_staff_free = orderDetail_Function.Take_List_Staff_Free(item, Date_Start_Plus, Date_End_Plus);
                        List_Staff_Free_Of_Service _new_ = new List_Staff_Free_Of_Service();
                        if (reference_list_staff_free != null)
                        {
                            _new_.list_staff_free_count = reference_list_staff_free.Count;
                            _new_.service = service;
                            _new_.Date_Start = Date_Start_Plus;
                            _new_.Date_End = Date_End_Plus;
                        }
                        else
                        {
                            _new_.list_staff_free_count = 0;
                            _new_.service = service;
                            _new_.Date_Start = Date_Start_Plus;
                            _new_.Date_End = Date_End_Plus;
                        }
                        reference_list_Staff_Free_Of_Services.Add(_new_);

                        List<Staff> _reference_list_staff_free = orderDetail_Function.Take_List_Staff_Free(item, Date_Start_Substract, Date_End_Substract);
                        List_Staff_Free_Of_Service _new__ = new List_Staff_Free_Of_Service();
                        if (_reference_list_staff_free != null)
                        {
                            _new__.list_staff_free_count = _reference_list_staff_free.Count;
                            _new__.service = service;
                            _new__.Date_Start = Date_Start_Substract;
                            _new__.Date_End = Date_End_Substract;
                        }
                        else
                        {
                            _new__.list_staff_free_count = 0;
                            _new__.service = service;
                            _new__.Date_Start = Date_Start_Substract;
                            _new__.Date_End = Date_End_Substract;
                        }
                        reference_list_Staff_Free_Of_Services.Add(_new__);
                    }
                }
                else
                {
                    List<int> _list_service_id = new List<int>();
                    //_list_service_id.Add(1); _list_service_id.Add(2); _list_service_id.Add(3);
                    List<Service> list_service = db.Services.ToList();
                    if (list_service != null)
                    {
                        foreach (var item in list_service)
                        {
                            _list_service_id.Add(item.Id);
                        }
                    }
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
            }
            ViewBag.list_Staff_Free_Of_Services = list_Staff_Free_Of_Services;
            ViewBag.reference_list_Staff_Free_Of_Services = reference_list_Staff_Free_Of_Services;
            ViewBag.List_Service_Id = list_service_id;
            return View();
        }
    }
}