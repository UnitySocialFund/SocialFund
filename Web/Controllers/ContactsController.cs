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
            MailSender sender = new MailSender();
            sender.SetCredential("unitysocialfund@mail.ru", "Install_new23");
            sender.InitializeMailMessage(model.Email, "unitysocialfund@mail.ru", model.Text, "Feedback from " + model.Name);
            sender.SendSmtp(false);

            return View(model);
        }

        #endregion
    }
}
