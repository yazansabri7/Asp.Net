using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace YASHOP.BLL.Service.Clasess
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587) // In Server 
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("sabriyazan2@gmail.com", "zovo lgpc lvlc pngr")
            };

            return client.SendMailAsync(
                new MailMessage(from: "sabriyazan2@gmail.com",
                                to: email,
                                subject,
                                htmlMessage
                                )
                { IsBodyHtml = true });
        }
    }
}
