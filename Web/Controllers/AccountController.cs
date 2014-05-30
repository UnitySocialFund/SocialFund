using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Services.Authentication;
using SocialFund.Models.Account;
using DataModel;
using Services;

namespace SocialFund.Controllers
{
    public class AccountController : Controller
    {

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
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

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

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(model.UserName, model.Password, model.Email);

                if (membershipUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Error during registration");
                }
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserInformation(RegisterModel model)
        {
            MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).GetUser(User.Identity.Name, true);

            if (membershipUser != null)
            {
                membershipUser.Email = model.Email;
                ((CustomMembershipProvider)Membership.Provider).UpdateUser(membershipUser);
                model.UserName = User.Identity.Name;
            }
            else
            {
                ModelState.AddModelError("", "Error during updating");
            }

            return View(model);
        }

        [Authorize]
        public ActionResult UserInformation()
        {
            var i = User.Identity.Name;
            User user = ((IProcessingUser)Membership.Provider).GetUserInformationByName(User.Identity.Name);

            RegisterModel model = new RegisterModel();
            model.UserName = user.Name;
            model.Email = user.Email;

            return View(model);
        }
    }
}