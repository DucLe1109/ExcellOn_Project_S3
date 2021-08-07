using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Models;
using System.Linq;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class CompanyManagementController : BaseController
    {
        private Entities db = new Entities();

        // Test pagination by ajax
        public JsonResult GetTotalPage(int items)
        {
            int p;
            var list_company = db.Companies.ToList();
            var Count_list_company = list_company.Count();
            float pages_tamp = (float)Count_list_company / items;
            float pages_tamp2 = (int)pages_tamp;
            pages_tamp2 = (float)(pages_tamp2 + 0.00001);
            if (pages_tamp > pages_tamp2)
            {
                p = (int)pages_tamp2 + 1;
            }
            else
            {
                p = (int)pages_tamp2;
            }
            return Json(p, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetData(int CurrentPage, int items)
        {
            int offset = (CurrentPage - 1) * items;
            var list_company = db.Companies.ToList();
            var company_ofPage = (from p in list_company select p).Skip(offset).Take(items);
            return Json(company_ofPage, JsonRequestBehavior.AllowGet);
        }

        // End test pagination by ajax


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