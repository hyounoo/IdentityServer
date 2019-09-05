using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; }
        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            //발송용 메일주소
            var emailFrom = Configuration["Email:From"];
            //패스워드
            var mailPass = Configuration["Email:Password"];

            MailMessage msg = new MailMessage(emailFrom, email, subject, message);
            msg.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;

            //발송용 메일주소, 패스워드
            smtp.Credentials = new NetworkCredential(emailFrom, mailPass);

            smtp.SendMailAsync(msg).Wait();

            return Task.CompletedTask;
        }

        // SendGrid
        //public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    var apiKey = Configuration["SENDGRID_APIKEY"];
        //    var client = new SendGridClient(apiKey);
        //    var msg = new SendGridMessage()
        //    {
        //        From = new EmailAddress(Configuration["Email:From"], "Foresting support"),
        //        Subject = subject,
        //        HtmlContent = htmlMessage
        //    };
        //    msg.AddTo(new EmailAddress(email));
        //    var response = await client.SendEmailAsync(msg);
        //}
    }
}
