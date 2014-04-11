using DataModel;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialFund.Controllers
{
    public class HomeController : Controller
    {
        #region Controller methods

        public ActionResult Index()
        {
            if (true)
            {
                return this.RedirectToAction("Index", "Log", new { groupId = 1});
            }

            ViewData["Message"] = "Welcome to Social Fund!";

            return View();
        }

        #endregion
    }
}
