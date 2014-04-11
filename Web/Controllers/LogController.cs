using Services;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using System.Web.Routing;

namespace SocialFund.Controllers
{
    public class LogController : Controller
    {
        private readonly LogService _logService;

        private readonly GroupService _groupService;

        public LogController()
        {
            _logService = new LogService();
            _groupService = new GroupService();
        }

        public ActionResult Index(int groupId)
        {
            var viewModel = new LogIndexViewModel();
            viewModel.Logs = _logService.GetGroupHistory(groupId);
            viewModel.Group = _groupService.GetGroup(groupId);
            viewModel.TatalBalnce = _logService.GetCurrentBalance(groupId);
            viewModel.IsGroupOwner = true;
            return View(viewModel);
        }


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

            _logService.AddLog(viewModel, id, 1);
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
                return false;
            }

            return true;
        }
    }
}
