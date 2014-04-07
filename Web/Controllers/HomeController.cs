using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
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
