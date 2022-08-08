using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Blog.DTOs;

namespace Blog.Services
{
    public class EmailService
    {
        public bool Send(Email email)
        {

            var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);
            
            smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            var mail = new MailMessage();
            mail.From = new MailAddress(email.FromEmail, email.FromName);
            mail.To.Add(new MailAddress(email.ToEmail, email.ToName));
            mail.Subject = email.Subject;
            mail.Body = email.Body;
            mail.IsBodyHtml = true;

            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }   
    }
}