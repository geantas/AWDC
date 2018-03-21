using AarhusWebDevCoop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using System.Net.Mail;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: Default
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            else
            {


                MailMessage message = new MailMessage();
                message.To.Add("eaagipo@students.eaaa.dk");
                message.Subject = model.Subject;
                message.From = new MailAddress(model.Email, model.Name);
                message.Body = model.Message;
               
                // Parameters – name, parentId, contentTypeAlias
                IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "message");
                // assign values
                comment.SetValue("messageName", model.Name);
                comment.SetValue("email", model.Email);
                comment.SetValue("subject", model.Subject);
                comment.SetValue("messageContent", model.Message);
                // save to Umbraco

                Services.ContentService.Save(comment);
                // Services.ContentService.SaveAndPublish(comment);
                


                using (SmtpClient smtp = new SmtpClient())
                {                                                
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("@gmail.com", "");
                    smtp.EnableSsl = true;
                    // send mail
                    smtp.Send(message);

                    TempData["success"] = true;

                    return RedirectToCurrentUmbracoPage();
                }

            }

        }

    }

}