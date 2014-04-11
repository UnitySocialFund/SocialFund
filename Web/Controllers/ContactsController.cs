using DataModel;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialFund.Models.Contacts;

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
            // Send info to mail
            return View(model);
        }

        #endregion
    }
}
