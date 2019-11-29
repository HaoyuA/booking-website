using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FIT5032_Assign.Models;
using System.IO;

namespace FIT5032_Assign.Utils
{
    public class EmailSender
    {
        private const String API_KEY = "";

        public void Send(String toEmail, String subject, String contents, String fileName, String filePath)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("noreply@localhost.com", "FIT5032 Example Email User");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var bytes = File.ReadAllBytes(filePath);
            var file = Convert.ToBase64String(bytes);
            msg.AddAttachment(fileName, file);

            var response = client.SendEmailAsync(msg);
        }

    }
}