using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Models;
using System.Linq;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class CompanyManagementController : BaseController
    {
        private Entities db = new Entities();
        [HasPermission(Permission = "Company_List")]
        public ActionResult CompanyIndex()
        {

            var list_company = db.Companies.ToList();
            ViewBag.list_company = list_company;
            return View();

        }

        [HasPermission(Permission = "Company_Add")]
        [HttpPost]
        public JsonResult Add(Company companyObj)
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

        [HasPermission(Permission = "Company_Edit")]
        [HttpGet]
        public JsonResult GetById(int CompanyId)
        {

            Company company = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
            return Json(company, JsonRequestBehavior.AllowGet);

        }

        [HasPermission(Permission = "Company_Edit")]
        [HttpPost]
        public JsonResult Update(Company companyObj)
        {
            Company company = db.Companies.Where(x => x.Id == companyObj.Id).FirstOrDefault();
            company.Company_Address = companyObj.Company_Address;
            company.Company_Description = companyObj.Company_Description;
            company.Company_Email = companyObj.Company_Email;
            company.Company_Name = companyObj.Company_Name;
            company.Company_Phone = companyObj.Company_Phone;
            db.SaveChanges();
            return Json("/Admin/CompanyManagement/CompanyIndex", JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Company_Delete")]
        [HttpGet]
        public JsonResult Delete(int CompanyId)
        {
            Company company = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
            if (company != null)
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
    }
}