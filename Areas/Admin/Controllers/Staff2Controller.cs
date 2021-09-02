using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class Staff2Controller : StaffBaseController
    {
        Entities db = new Entities();
        public ActionResult MyProfile()
        {
            Staff staff = (Staff)Session["StaffName"];

            Staff _staff = db.Staffs.Where(x => x.Id == staff.Id).FirstOrDefault();
            return View(_staff);
        }

        [HttpPost]
        public ActionResult Update_Profile(Staff CurrentStaff, HttpPostedFileBase AvatarUpload)
        {

            var staff = db.Staffs.Where(x => x.Staff_UserName == CurrentStaff.Staff_UserName).FirstOrDefault();
            if (staff != null)
            {
                staff.Staff_Age = CurrentStaff.Staff_Age;
                staff.Staff_Email = CurrentStaff.Staff_Email;
                staff.Staff_FullName = CurrentStaff.Staff_FullName;
                staff.Staff_Gender = CurrentStaff.Staff_Gender;
                if (CurrentStaff.Staff_Password != null)
                {
                    staff.Staff_Password = BCrypt.Net.BCrypt.HashPassword(CurrentStaff.Staff_Password);
                }
                else
                {
                    staff.Staff_Password = staff.Staff_Password;
                }

                staff.Staff_Phone = CurrentStaff.Staff_Phone;
                staff.Staff_Address = CurrentStaff.Staff_Address;
                staff.Staff_AboutMe = CurrentStaff.Staff_AboutMe;
                if (AvatarUpload != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(AvatarUpload.FileName);
                    string Extension = Path.GetExtension(AvatarUpload.FileName);
                    FileName = FileName + Extension;
                    staff.Staff_Avatar = "/Public/Image/" + FileName;
                    AvatarUpload.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
                }
                else
                {
                    staff.Staff_Avatar = staff.Staff_Avatar;
                }

                db.SaveChanges();
            }
            return RedirectToAction("MyProfile");
        }

        public ActionResult MyOrder()
        {
            Staff current_staff = (Staff)Session["StaffName"];
            List<Staff_OrderDetail> list_staff_ord = db.Staff_OrderDetail.Where(x => x.Staff_Id == current_staff.Id).ToList();
            List<OrderDetail> list_ord = new List<OrderDetail>();
            foreach (var item in list_staff_ord)
            {
                OrderDetail ord = db.OrderDetails.Where(x => x.Id == item.OrderDetail_Id).FirstOrDefault();
                if (ord != null)
                {
                    list_ord.Add(ord);
                }
            }
            list_ord.OrderBy(x => x.OrderDetail_DateStart);
            return View(list_ord);
        }
    }
}