﻿using System.Web.Routing;
using DataModel;
using Services;
using SocialFund.Models.GroupViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace SocialFund.Controllers
{
    [Authorize]
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

        public ActionResult ShowGroupDetails(int groupId, int page = 1)
        {
            var vm = new GroupDetailsViewModel();
            vm.Group = _groupService.GetGroup(groupId);
            vm.GroupUsers = _groupService.GetUsersForGroup(groupId).ToPagedList(page, 10);
            if (_logService.GetUserId(User.Identity.Name) == vm.Group.OwnerId)
                vm.currentUserIsOwner = true;
            return this.View(vm);
        }

        public ActionResult ShowUserForAddition(int groupId, string query, int page = 1)
        {
            var vm = new UserForAdditionToGroup(groupId);
            vm.Query = query;
            if (!String.IsNullOrEmpty(query))
            {
                vm.UsersPaged = _groupService.GetUserNotInGroup(groupId).Where(x => x.Name.Contains(query)).ToPagedList(page, 10);
            }
            else
            {
                vm.UsersPaged = _groupService.GetUserNotInGroup(groupId).ToPagedList(page, 10);
            }
            return this.View(vm);
        }

        public ActionResult AddUserToGroup(int groupId, int userId, int page)
        {
            _groupService.CreateGroupUser(groupId, userId);
            return this.RedirectToAction("ShowUserForAddition", new { groupId = groupId, page = page });
        }

        public ActionResult RemoveUserFromGroup(int groupId, int userId)
        {
            _groupService.DeleteUserFromGroup(groupId, userId);
            return RedirectToAction("GroupRoom", new { groupId = groupId });
        }

        public ActionResult GroupRoom(int groupId, int page = 1)
        {
            var vm = new GroupDetailsViewModel();
            vm.Group = _groupService.GetGroup(groupId);
            vm.GroupUsers = _groupService.GetUsersForGroup(groupId).ToPagedList(page, 10);
            vm.LastLogs = _logService.GetGroupHistory(groupId);
            vm.TatalBalnce = _logService.GetCurrentBalance(groupId);
            if (_logService.GetUserId(User.Identity.Name) == vm.Group.OwnerId)
                vm.currentUserIsOwner = true;
            return this.View(vm);
        }

        /// <summary>
        /// Change name of divisions or groups in current tournament.
        /// </summary>
        /// <param name="id">Division or group Id to change.</param>
        /// <param name="newName">New name to change.</param>
        /// <returns>View with details of chosen tournament.</returns>
        public ActionResult ChangeGroupName(int id, string newName)
        {
            var group = _groupService.GetGroup(id);
            group.Name = newName;
            _groupService.EditGroupDetails(group);
            return this.RedirectToAction("GroupRoom", new { groupId = id });

        }

        public ActionResult SendMails(string title, string message, int id = 1)
        {
            _groupService.Spam(id, title, message);
            return this.RedirectToAction("GroupRoom", new {groupId = id});
        }
    }
}
