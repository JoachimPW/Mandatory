using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AarhusWebDevCoop.Views.ViewModel;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;


namespace AarhusWebDevCoop.Controller
{
    public class ContactFormSurfaceController : SurfaceController
    {

        
        // GET: ContactFormSurface
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
            TempData["success"] = true;
            MailMessage message = new MailMessage();
            message.To.Add("username@eaaa.dk");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");
            comment.SetValue("messageName", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("messageCount", model.Message);

            Services.ContentService.Save(comment);

            using (SmtpClient smtp = new SmtpClient())
            {
                
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("joachimpw@gmail.com", "exbxvwoolarkjhlv");
                smtp.Send(message);}


                return RedirectToCurrentUmbracoPage();
        }

    }
}