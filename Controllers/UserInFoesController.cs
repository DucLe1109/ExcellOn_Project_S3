using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _ExcellOn_.Models;

namespace _ExcellOn_.Controllers
{
    public class UserInFoesController : Controller
    {
        private Entities db = new Entities();

        // GET: UserInFoes
        public ActionResult Index()
        {
            var userInFoes = db.UserInFoes.Include(u => u.Service);
            return View(userInFoes.ToList());
        }

        // GET: UserInFoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInFo userInFo = db.UserInFoes.Find(id);
            if (userInFo == null)
            {
                return HttpNotFound();
            }
            return View(userInFo);
        }

        // GET: UserInFoes/Create
        public ActionResult Create()
        {
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Service_Name");
            return View();
        }

        // POST: UserInFoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User_Name,User_Password,User_Email,User_Gender,User_Age,User_Status,User_Phone,User_Address,User_Avatar,User_FullName,User_AboutMe,ServiceId")] UserInFo userInFo)
        {
            if (ModelState.IsValid)
            {
                db.UserInFoes.Add(userInFo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Service_Name", userInFo.ServiceId);
            return View(userInFo);
        }

        // GET: UserInFoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInFo userInFo = db.UserInFoes.Find(id);
            if (userInFo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Service_Name", userInFo.ServiceId);
            return View(userInFo);
        }

        // POST: UserInFoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,User_Name,User_Password,User_Email,User_Gender,User_Age,User_Status,User_Phone,User_Address,User_Avatar,User_FullName,User_AboutMe,ServiceId")] UserInFo userInFo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userInFo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Service_Name", userInFo.ServiceId);
            return View(userInFo);
        }

        // GET: UserInFoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInFo userInFo = db.UserInFoes.Find(id);
            if (userInFo == null)
            {
                return HttpNotFound();
            }
            return View(userInFo);
        }

        // POST: UserInFoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserInFo userInFo = db.UserInFoes.Find(id);
            db.UserInFoes.Remove(userInFo);
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
    }
}
