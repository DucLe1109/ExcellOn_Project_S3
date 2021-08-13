using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace _ExcellOn_.Models
{
    public class SendMail
    {
        public void sendMail(string toEmailAddress, string subject, string content)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            //var toEmailAddress = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            bool enableSSL = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"].ToString());
            string body = content;
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            var client = new SmtpClient();
            client.EnableSsl = enableSSL;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }

    }

}