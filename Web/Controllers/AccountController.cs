using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SocialFund.Models.Account;

namespace SocialFund.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}
