using _ExcellOn_.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Section_Contact()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string fullName, string email, string phone, string subject, string content)
        {
            fullName = Request.Form["name"];
            phone = Request.Form["phone"];
            email = Request.Form["email"];
            subject = Request.Form["subject"];
            content = Request.Form["content"];
            if (ModelState.IsValid)
            {
                string ct = System.IO.File.ReadAllText(Server.MapPath("~/Content/Manh/assets/templateMail/Mail.html"));
                string ph = System.IO.File.ReadAllText(Server.MapPath("~/Content/Manh/assets/templateMail/feedbackmail.html"));
                ct = ct.Replace("{{CustomerName}}", fullName);
                ct = ct.Replace("{{Phone}}", phone);
                ct = ct.Replace("{{EmailName}}", email);
                ct = ct.Replace("{{Subject}}", subject);
                ct = ct.Replace("{{Content}}", content);              
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new SendMail().sendMail(toEmail, "Nội dung phản hồi", ct);
                new SendMail().sendMail(email, "Nội dung phản hồi", ph);

            }
            else { }
            return View();
        }
    }
 }
