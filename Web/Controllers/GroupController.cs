using DataModel;
using Services;
using SocialFund.Models.GroupViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialFund.Controllers
{
    public class GroupController : Controller
    {
        private readonly GroupService _groupService;

        private readonly LogService _logService;

        public GroupController()
        {
            _groupService = new GroupService();
            _logService = new LogService();
        }
        //
        // GET: /Group/

        public ActionResult Index()
        {
            var viewModel = new GroupIndexViewModel();
            var groups = _groupService.GetGroupForUser(User.Identity.Name);
            int curentUserId = _logService.GetUserId(User.Identity.Name);
            foreach (var group in groups)
            {
                if (group.OwnerId == curentUserId)
                {
                    viewModel.MyGroups.Add(group);
                }
                else
                {
                    viewModel.InvitedGroup.Add(group);
                }
            }
            return View(viewModel);
        }

        public ActionResult CreateGroup()
        {
            var viewModel = new Group();
            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateGroup(Group viewModel)
        {
            _groupService.CreateGroup(viewModel, User.Identity.Name);
            return this.RedirectToAction("Index");
        }

        public ActionResult AddUserToGroup(int id)
        {
            var viewModel = new AddUserToGroupViewModel();

            viewModel.Users = _groupService.GetUserNotInGroup(id);
            viewModel.Group = _groupService.GetGroup(id);
            
            return this.View(viewModel);
        }

        public ActionResult AddUserToGroupPost(int groupId, int userId)
        {
            _groupService.CreateGroupUser(groupId, userId);
            return this.RedirectToAction("Index", "Log", new { groupId = groupId });
        }
    }
}
