using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialFund.Controllers
{
    public class LogController : Controller
    {
        private readonly LogService _logService;

        public LogController()
        {
            _logService = new LogService();
        }

        public ActionResult Index(int groupId)
        {
            var logs = _logService.GetGroupHistory(groupId);
            return View(logs);
        }

    }
}
