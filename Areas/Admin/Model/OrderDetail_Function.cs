using _ExcellOn_.Areas.Admin.Model.Order_Interface;
using _ExcellOn_.Models;
using _ExcellOn_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _ExcellOn_.Areas.Admin.Model
{
    public class OrderDetail_Function : OrderDetail_Interface
    {
        private Entities db = new Entities();
        public List<Staff> Take_List_Staff_Free(int ord_id)
        {
            List<Staff> list_staff_free_for_thisOrd = new List<Staff>();
            OrderDetail ord = db.OrderDetails.Where(x => x.Id == ord_id).FirstOrDefault();
            int Service_Id = (int)ord.ServiceId;
            DateTime Date_Start = (DateTime)ord.OrderDetail_DateStart;
            DateTime Date_End = (DateTime)ord.OrderDetail_DateEnd;
            List<Staff> list_Staff_free = db.Staffs.Where(x => x.Staff_OrderDetail.Count == 0 && x.ServiceId == Service_Id).ToList();
            if (list_Staff_free.Count > 0)
            {
                foreach (var item in list_Staff_free)
                {
                    list_staff_free_for_thisOrd.Add(item);
                }
            }
            // Nếu tất cả các Staff đều đã tham gia tối thiểu vào 1 OrderDetail thì phải xử lý trùng lặp ngày tháng trong này.
            var list_Staff_OrderDetail = db.Staff_OrderDetail.ToList();
            List<Staff> _list_Staff_free = new List<Staff>(); // Danh sách những Staff không bị trùng lịch với đơn đặt hàng mới 
            List<Staff> _list_Staff_Not_free = new List<Staff>(); //  Danh sách những Staff bị trùng lịch với đơn đặt hàng mới 
            foreach (var item in list_Staff_OrderDetail)
            {
                if ((((Date_Start - (DateTime)item.Date_End).TotalDays > 0) && ((Date_Start - (DateTime)item.Date_Start).TotalDays > 0)) || (((Date_End - (DateTime)item.Date_Start).TotalDays < 0) && ((Date_End - (DateTime)item.Date_Start).TotalDays < 0)))
                {
                    Staff free_staff = db.Staffs.Where(x => x.Id == item.Staff_Id).FirstOrDefault();
                    if (_list_Staff_free.Exists(x => x.Id == free_staff.Id))
                    {
                        // không add những staff đã lặp lại
                    }
                    else
                    {
                        _list_Staff_free.Add(free_staff);
                    }

                }
                else
                {
                    Staff free_staff = db.Staffs.Where(x => x.Id == item.Staff_Id).FirstOrDefault();
                    if (_list_Staff_Not_free.Exists(x => x.Id == free_staff.Id))
                    {
                        // không add những staff đã lặp lại
                    }
                    else
                    {
                        _list_Staff_Not_free.Add(free_staff);
                    }
                }
            }
            List<Staff> _list_Staff_free_service = _list_Staff_free.Where(x => x.ServiceId == Service_Id).ToList(); //  Danh sách những Staff không bị trùng lịch với đơn đặt hàng mới lọc theo loại dịch vụ
            List<Staff> _list_Staff_Not_free_service = _list_Staff_Not_free.Where(x => x.ServiceId == Service_Id).ToList(); //  Danh sách những Staff bị trùng lịch với đơn đặt hàng mới lọc theo loại dịch vụ

            // TH1: đơn hàng không trùng ngày với các đơn đã lên.
            if (_list_Staff_Not_free_service.Count == 0)
            {
                foreach (var item in _list_Staff_free_service)
                {
                    if (list_staff_free_for_thisOrd.Exists(x => x.Id == item.Id) == true)
                    {

                    }
                    else
                    {
                        list_staff_free_for_thisOrd.Add(item);
                    }
                }
            }
            // Th2: đơn hàng mới trùng với các đơn đã lên.
            else
            {
                List<Staff> list_staff_have_inTable = db.Staffs.Where(x => x.ServiceId == Service_Id).ToList();
                if (_list_Staff_Not_free_service.Count <= list_staff_have_inTable.Count)
                {
                    var list_staff_free = list_staff_have_inTable.Except(_list_Staff_Not_free_service);
                    foreach (var item in list_staff_free)
                    {
                        if (list_staff_free_for_thisOrd.Exists(x => x.Id == item.Id) == true)
                        {

                        }
                        else
                        {
                            list_staff_free_for_thisOrd.Add(item);
                        }
                    }
                }
            }
            return list_staff_free_for_thisOrd;
        }

        public List<Staff> Take_List_Staff_Free(SearchRequest searchRequest)
        {
            List<Staff> list_staff_free_for_thisOrd = new List<Staff>();
            int Service_Id = searchRequest.Service_Id;
            DateTime Date_Start = searchRequest.Date_Start;
            DateTime Date_End = searchRequest.Date_End;

            List<Staff> list_Staff_free = db.Staffs.Where(x => x.Staff_OrderDetail.Count == 0 && x.ServiceId == Service_Id).ToList();
            if (list_Staff_free.Count > 0)
            {
                foreach (var item in list_Staff_free)
                {
                    list_staff_free_for_thisOrd.Add(item);
                }
            }
            // Nếu tất cả các Staff đều đã tham gia tối thiểu vào 1 OrderDetail thì phải xử lý trùng lặp ngày tháng trong này.
            var list_Staff_OrderDetail = db.Staff_OrderDetail.ToList();
            List<Staff> _list_Staff_free = new List<Staff>(); // Danh sách những Staff không bị trùng lịch với đơn đặt hàng mới 
            List<Staff> _list_Staff_Not_free = new List<Staff>(); //  Danh sách những Staff bị trùng lịch với đơn đặt hàng mới 
            foreach (var item in list_Staff_OrderDetail)
            {
                if ((((Date_Start - (DateTime)item.Date_End).TotalDays > 0) && ((Date_Start - (DateTime)item.Date_Start).TotalDays > 0)) || (((Date_End - (DateTime)item.Date_Start).TotalDays < 0) && ((Date_End - (DateTime)item.Date_Start).TotalDays < 0)))
                {
                    Staff free_staff = db.Staffs.Where(x => x.Id == item.Staff_Id).FirstOrDefault();
                    if (_list_Staff_free.Exists(x => x.Id == free_staff.Id))
                    {
                        // không add những staff đã lặp lại
                    }
                    else
                    {
                        _list_Staff_free.Add(free_staff);
                    }

                }
                else
                {
                    Staff free_staff = db.Staffs.Where(x => x.Id == item.Staff_Id).FirstOrDefault();
                    if (_list_Staff_Not_free.Exists(x => x.Id == free_staff.Id))
                    {
                        // không add những staff đã lặp lại
                    }
                    else
                    {
                        _list_Staff_Not_free.Add(free_staff);
                    }
                }
            }
            List<Staff> _list_Staff_free_service = _list_Staff_free.Where(x => x.ServiceId == Service_Id).ToList(); //  Danh sách những Staff không bị trùng lịch với đơn đặt hàng mới lọc theo loại dịch vụ
            List<Staff> _list_Staff_Not_free_service = _list_Staff_Not_free.Where(x => x.ServiceId == Service_Id).ToList(); //  Danh sách những Staff bị trùng lịch với đơn đặt hàng mới lọc theo loại dịch vụ

            // TH1: đơn hàng không trùng ngày với các đơn đã lên.
            if (_list_Staff_Not_free_service.Count == 0)
            {
                foreach (var item in _list_Staff_free_service)
                {
                    if (list_staff_free_for_thisOrd.Exists(x=>x.Id == item.Id) == true)
                    {

                    }
                    else
                    {
                        list_staff_free_for_thisOrd.Add(item);
                    }
                    
                }
            }
            // Th2: đơn hàng mới trùng với các đơn đã lên.
            else
            {
                List<Staff> list_staff_have_inTable = db.Staffs.Where(x => x.ServiceId == Service_Id).ToList();
                if (_list_Staff_Not_free_service.Count <= list_staff_have_inTable.Count)
                {
                    var list_staff_free = list_staff_have_inTable.Except(_list_Staff_Not_free_service);
                    foreach (var item in list_staff_free)
                    {
                        if (list_staff_free_for_thisOrd.Exists(x => x.Id == item.Id) == true)
                        {

                        }
                        else
                        {
                            list_staff_free_for_thisOrd.Add(item);
                        }
                        
                    }
                }
            }

            return list_staff_free_for_thisOrd;
        }
    }
}