using DataModel;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialFund.Models.Contacts;
using Common;

namespace SocialFund.Controllers
{
    public class ContactsController : Controller
    {
        #region Controller methods

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FeedbackModel model)
        {
            if (this.ModelState.IsValid == true)
            {
                if (MailSender.ValidateEmail(model.Email))
                {
                    MailSender sender = new MailSender();
                    sender.InitializeMailMessage(model.Email, "unitysocialfund@gmail.com", "Feedback from " + model.Name,
                        model.Text);
                    sender.SendSmtp(false);
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "No valid E-Mail.");    
                }
                
            }

            return View(model);
        }

        #endregion
    }
}
