using Services;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using System.Web.Routing;
using Web.Models.LogViewModels;

namespace SocialFund.Controllers
{
    public class LogController : Controller
    {
        private readonly LogService _logService;

        private readonly GroupService _groupService;

        private void InitalizeFakeData()
        {
            var user = new User();
            user.Name = "User1";
            user.Email = "test@test.ru";
            user.Password = "newPassword";
            var group = new Group();
            group.User = user;
            group.Name = "Test group";
            var groupUser = new Group_User();
            groupUser.User = user;
            groupUser.Group = group;
            var log = new Log();
            log.Date = DateTime.Now;
            log.Group_User = groupUser;
            log.Coins = 25;
            log.Comment = "test log";
            using (var db = new SocialFundEntities())
            {
                db.User.Add(user);
                db.Group.Add(group);
                db.Group_User.Add(groupUser);
                db.Log.Add(log);
                db.SaveChanges();
            }
        }

        public LogController()
        {
            // InitalizeFakeData();
            _logService = new LogService();
            _groupService = new GroupService();
        }

        public ActionResult Index(int groupId)
        {
            int currentUserId = _logService.GetUserId(User.Identity.Name);
            var viewModel = new LogIndexViewModel();
            viewModel.Logs = _logService.GetGroupHistory(groupId);
            viewModel.Group = _groupService.GetGroup(groupId);
            viewModel.TatalBalnce = _logService.GetCurrentBalance(groupId);

            if (viewModel.Group.OwnerId == currentUserId)
            {
                viewModel.IsGroupOwner = true;
            }

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddCoins(int id)
        {
            var viewModel = new Log();
            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult AddCoins(int id, Log viewModel)
        {
            if (this.ValidateCoins(id, viewModel) == false)
            {
                return this.View(viewModel);
            }

            _logService.AddLog(viewModel, id, User.Identity.Name);
            return this.RedirectToAction("Index", new {groupId = id });
        }

        private bool ValidateCoins(int groupId, Log log)
        {
            if (this.ModelState.IsValid == false)
            {
                return false;
            }

            decimal balance = _logService.GetCurrentBalance(groupId);
            if (balance + log.Coins < 0)
            {
                string message = "Balance ca not be less then 0, total balance = " + balance.ToString();
                this.ModelState.AddModelError(string.Empty, message);
                return false;
            }

            return true;
        }
    }
}
