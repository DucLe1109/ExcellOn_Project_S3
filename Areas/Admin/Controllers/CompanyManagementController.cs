using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Areas.Admin.ViewModel;
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
            var list_company = db.Companies.Where(x=>x.Deleted != 1).ToList();
            return View(list_company);
        }

        [HasPermission(Permission = "Company_List")]
        public ActionResult CompanyDeletedIndex()
        {
            var list_company = db.Companies.Where(x=>x.Deleted == 1).ToList();
            return View(list_company);
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
            CompanyViewModel companyViewModel = new CompanyViewModel();
            companyViewModel.Id = company.Id;
            companyViewModel.Company_Name = company.Company_Name;
            companyViewModel.Company_Phone = company.Company_Phone;
            companyViewModel.Company_Email = company.Company_Email;
            companyViewModel.Company_Address = company.Company_Address;
            companyViewModel.Company_Description = company.Company_Description;
            return Json(companyViewModel, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Company_Edit")]
        [HttpPost]
        public JsonResult Update(Company companyObj)
        {
            Company company = db.Companies.Where(x => x.Id == companyObj.Id).FirstOrDefault();
            if (company != null)
            {
                company.Company_Address = companyObj.Company_Address;
                company.Company_Description = companyObj.Company_Description;
                company.Company_Email = companyObj.Company_Email;
                company.Company_Name = companyObj.Company_Name;
                company.Company_Phone = companyObj.Company_Phone;
                db.SaveChanges();
            }
            CompanyViewModel companyViewModel = new CompanyViewModel();
            companyViewModel.Id = company.Id;
            companyViewModel.Company_Name = company.Company_Name;
            companyViewModel.Company_Phone = company.Company_Phone;
            companyViewModel.Company_Email = company.Company_Email;
            companyViewModel.Company_Address = company.Company_Address;
            companyViewModel.Company_Description = company.Company_Description;
            return Json(companyViewModel, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Company_Delete")]
        [HttpGet]
        public JsonResult Delete(int CompanyId)
        {
            Company company = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
            if (company != null)
            {
                company.Deleted = 1;
                db.SaveChanges();
                return Json("Successfully", JsonRequestBehavior.AllowGet);
            }
            return Json(404, JsonRequestBehavior.AllowGet);
        }

        [HasPermission(Permission = "Company_Delete")]
        [HttpGet]
        public JsonResult Reset(int CompanyId)
        {
            Company company = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
            if (company != null)
            {
                company.Deleted = 0;
                db.SaveChanges();
                return Json("/Admin/CompanyManagement/CompanyIndex", JsonRequestBehavior.AllowGet);
            }
            return Json("Error !", JsonRequestBehavior.AllowGet);
        }
    }
}