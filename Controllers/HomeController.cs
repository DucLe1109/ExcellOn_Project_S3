using _ExcellOn_.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Controllers
{
    public class HomeController : Controller
    {
        // Webpage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
           return View();
        }
        [HttpPost]
        public ViewResult Contact(_ExcellOn_.Models.MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Public/assets/template/neworder.html"));

                content = content.Replace("{{CustomerName}}", _objModelMail.w3lName);
                content = content.Replace("{{Phone}}", _objModelMail.w3lPhone);
                content = content.Replace("{{Email}}", _objModelMail.w3lSender);
                content = content.Replace("{{text}}", _objModelMail.w3lMessage);
                mail.To.Add("phuchuong120519951@gmail.com");
                mail.From = new MailAddress(_objModelMail.w3lSender);
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.w3lMessage;
                mail.Body = content;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("phuchuong120519951@gmail.com", "phuchuong95"); // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("Contact", _objModelMail);
            }
            else
            {
                return View();
            }
        }
    }
}