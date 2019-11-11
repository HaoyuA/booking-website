using FIT5032_Assign.Models;
using FIT5032_Assign.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FIT5032_Assign.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
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
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult BulkEmail()
        {
            return View(new SendEmailViewModel());
        }

        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send_Email(SendEmailViewModel model, HttpPostedFileBase postedFile)
        {
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;
                   
                    String fileName = postedFile.FileName;
                    String path = Server.MapPath("~/Uploads/");
                    if (postedFile != null)
                    {
                        
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        postedFile.SaveAs(path +
                        Path.GetFileName(postedFile.FileName));
                        ViewBag.Message = "File uploaded successfully.";
                    }
                    
                    String filePath = path + postedFile.FileName;
                    EmailSender es = new EmailSender();
                    es.Send(toEmail, subject, contents, fileName, filePath);

                    ViewBag.Result = "Email has been send.";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BulkEmail(SendEmailViewModel model, HttpPostedFileBase postedFile)
        {
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                try
                {
                    
                    String subject = model.Subject;
                    String contents = model.Contents;

                    String fileName = postedFile.FileName;
                    String path = Server.MapPath("~/Uploads/");
                    if (postedFile != null)
                    {

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        postedFile.SaveAs(path +
                        Path.GetFileName(postedFile.FileName));
                        ViewBag.Message = "File uploaded successfully.";
                    }

                    String filePath = path + postedFile.FileName;
                    EmailSender es = new EmailSender();
                    using (var userContext = new ApplicationDbContext())
                    {
                        foreach(var user in userContext.Users){
                            var email = user.Email;
                            es.Send(email, subject, contents, fileName, filePath);
                        }
                    }

                   
                    ViewBag.Result = "Email has been send.";
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

    }
}