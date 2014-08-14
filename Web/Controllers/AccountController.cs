using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Services.Authentication;
using SocialFund.Models.Account;
using DataModel;
using Services;
using SocialFund.Tools;

namespace SocialFund.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController()
        {
            _userService = new UserService();
        }

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
            RegisterModel regm = new RegisterModel();
            return View(regm);
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model, string returnUrl)
        {
            if (model.Captcha != (string)Session[CaptchaImage.CaptchaValueKey])
            {
                ModelState.AddModelError("Captcha", "Text from the image in an incorrect");
            }

            if (ModelState.IsValid)
            {
                MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(model.UserName, model.Password, model.Email, model.Phone, model.Address, model.IsNotif);

                if (membershipUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    new UserService().SandMail(model.Email, "Registration", "Congratulations! Your registration has been successfully submitted.");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Error during registration");
                }
            }
            return View(model);
        }

        public ActionResult Captcha()
        {
            Session[CaptchaImage.CaptchaValueKey] = new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString();
            var ci = new CaptchaImage(Session[CaptchaImage.CaptchaValueKey].ToString(), 211, 50, "Arial");

            // Change the response headers to output a JPEG image.
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            // Write the image to the response stream in JPEG format.
            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.
            ci.Dispose();
            return null;
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserInformation(User user)
        {
            _userService.UpdateUser(user);
            return RedirectToAction("Index", "Home");
            //MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).GetUser(User.Identity.Name, true);

            //if (membershipUser != null)
            //{
            //    membershipUser.Email = model.Email;
            //    ((CustomMembershipProvider)Membership.Provider).UpdateUser(membershipUser);
            //    model.UserName = User.Identity.Name;
            //}
            //else
            //{
            //    ModelState.AddModelError("", "Error during updating");
            //}

            //return View(model);


        }

        [Authorize]
        public ActionResult UserInformation()
        {

            return View(((IProcessingUser)Membership.Provider).GetUserInformationByName(User.Identity.Name));

            //var i = User.Identity.Name;
            //User user = ((IProcessingUser)Membership.Provider).GetUserInformationByName(User.Identity.Name);

            //RegisterModel model = new RegisterModel();
            //model.UserName = user.Name;
            //model.Email = user.Email;

            //return View(model);
        }

        [Authorize]
        public ActionResult UserDetails(string id)
        {
            if (System.String.Compare(User.Identity.Name, id, System.StringComparison.CurrentCulture) == 0)
            {
                return RedirectToAction("UserInformation");    
            }
            var user = ((IProcessingUser) Membership.Provider).GetUserInformationByName(id);
            var vm = new UserDetailsViewModel()
            {
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone
            };
            return View(vm);
        }
    }
}