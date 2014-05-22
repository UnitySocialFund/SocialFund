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
            
            ViewData["Message"] = "Welcome to Social Fund!";

            return View();
        }

        #endregion
    }
}
