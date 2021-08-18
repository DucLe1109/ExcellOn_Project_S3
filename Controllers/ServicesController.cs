using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _ExcellOn_.Models;

namespace _ExcellOn_.Controllers
{
    public class ServicesController : Controller
    {
        private Entities db = new Entities();

        // GET: Services
        public ActionResult Index()
        {
            return View(db.Services.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Service_Name,Service_Price,Service_Description,Service_TaxPercentage,Service_SaleStatus,Service_PriceSale,Service_Image")] Service service,
            HttpPostedFileBase Image)
        {
           
            if (Image != null && Image.ContentLength > 0)
                try
                {
                    var fileName = Path.GetFileName(Image.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Manh/assets/images"), fileName);
                    Image.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            service.Service_Image = Image.FileName;
            db.Services.Add(service);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Service_Name,Service_Price,Service_Description,Service_SaleStatus,Service_PriceSale,Service_Image")] Service service, HttpPostedFileBase Image)
        {
            Service sv = db.Services.FirstOrDefault(x => x.Id == service.Id);
            sv.Service_Name = service.Service_Name;
            sv.Service_Price = service.Service_Price;
            sv.Service_Description = service.Service_Description;         
            sv.Service_SaleStatus = service.Service_SaleStatus;
            sv.Service_PriceSale = service.Service_PriceSale;
            if(Image!=null && Image.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Image.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/Manh/assets/images"), fileName);
                Image.SaveAs(path);
                sv.Service_Image = fileName;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        public ActionResult service()
        {
            return View(db.Services.ToList());
        }
        [HttpGet]
        public ActionResult company()
        {
            var company = db.Companies.ToList();
            return PartialView(company.Take(3));
        }
        public ActionResult detailservice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Service sv = db.Services.SingleOrDefault(n => n.Id == id);

            if (sv == null)
            {
                return HttpNotFound();
            }
            return View(sv);
        }
        public ActionResult detailteam(int? id)
        {

            List<Staff> staff = db.Staffs.ToList();
            List<Service> services = db.Services.ToList();
            
            //List<DietPlan> dietPlans = db.DietPlans.ToList();
            var Res = from o in staff
                      join w in services on o.ServiceId equals w.Id where o.ServiceId==id && o.Staff_Status==1
                       select new ResultTeam()
                      {
                          id =o.Id,
                          name = o.Staff_FullName,
                          avt = o.Staff_Avatar,
                          des = o.Staff_AboutMe,
                          age = (int)o.Staff_Age,
                          gender=(int)o.Staff_Gender              
                      };
            return View(Res);

        }
        public ActionResult detailteam1(int? id)
        {

            List<Staff> staff = db.Staffs.ToList();
            List<Service> services = db.Services.ToList();
            //List<DietPlan> dietPlans = db.DietPlans.ToList();
            var Res = from o in staff
                      join w in services on o.ServiceId equals w.Id
                      where o.ServiceId == id && o.Staff_Status == 0



                      select new ResultTeam()
                      {
                          id = o.Id,
                          name = o.Staff_FullName,
                          avt = o.Staff_Avatar,
                          des = o.Staff_AboutMe,
                          age = (int)o.Staff_Age,
                          gender = (int)o.Staff_Gender

                      };
            return View(Res);

        }

        public ActionResult feedback()
        {
            var cus = db.Customers.ToList();
            return PartialView(cus.Take(3));
        }
        public ActionResult DetailStaff(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Staff st = db.Staffs.SingleOrDefault(n => n.Id == id);

            if (st == null)
            {
                return HttpNotFound();
            }
            return View(st);
        }
    }
}
