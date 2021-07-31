using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class CompanyManagementController : BaseController
    {
        private Entities db = new Entities();

        public ActionResult CompanyIndex()
        {
            Session["return_url"] = "/Admin/CompanyManagement/CompanyIndex";
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;
                var list_company = db.Companies.ToList();
                ViewBag.list_company = list_company;
                return View();
            }
            else
            {
                return Redirect("/AdminLogin");
            }
        }

        [HttpPost]
        public JsonResult Add(Company companyObj)
        {
            Session["return_url"] = "/Admin/CompanyManagement/Add";
            if (check_auth())
            {
                Company newCompany = new Company();
                newCompany.Company_Address = companyObj.Company_Address;
                newCompany.Company_Description = companyObj.Company_Description;
                newCompany.Company_Email = companyObj.Company_Email;
                newCompany.Company_Name = companyObj.Company_Name;
                newCompany.Company_Phone = companyObj.Company_Phone;
                db.Companies.Add(newCompany);
                db.SaveChanges();
                return Json("/Admin/CompanyManagement/CompanyIndex", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetById(int CompanyId)
        {
            Session["return_url"] = "/Admin/CompanyManagement/GetById";
            if (check_auth())
            {
                Company company = db.Companies.Where(x=> x.Id == CompanyId).FirstOrDefault();
                return Json(company, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(Company companyObj)
        {
            Session["return_url"] = "/Admin/CompanyManagement/Update";
            if (check_auth())
            {
                Company company = db.Companies.Where(x=> x.Id == companyObj.Id).FirstOrDefault();
                company.Company_Address = companyObj.Company_Address;
                company.Company_Description = companyObj.Company_Description;
                company.Company_Email = companyObj.Company_Email;
                company.Company_Name = companyObj.Company_Name;
                company.Company_Phone = companyObj.Company_Phone;
                db.SaveChanges();
                return Json("/Admin/CompanyManagement/CompanyIndex", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Delete(int CompanyId)
        {
            Session["return_url"] = "/Admin/CompanyManagement/Delete";
            if (check_auth())
            {
                Company company = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                if(company!= null)
                {
                    db.Companies.Remove(company);
                    db.SaveChanges();
                    return Json("/Admin/CompanyManagement/CompanyIndex", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("/Admin/CompanyManagement/CompanyIndex", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }
    }
}