using System.Threading.Tasks;
using DataModel;
using DataModel.BlogModel;
using Services;
using SocialFund.Models.GroupViewModel;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace SocialFund.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly GroupService _groupService;
        private readonly LogService _logService;
        private readonly BlogService _BlogService;

        public GroupController()
        {
            _groupService = new GroupService();
            _logService = new LogService();
            _BlogService = new BlogService();
        }

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
        public ActionResult CreateGroup(Group viewModel, bool isBlogCreate)
        {
            var groupId = _groupService.CreateGroup(viewModel, User.Identity.Name);

            if (isBlogCreate)
            {
                var group = _groupService.GetGroup(groupId);
                _BlogService.CreateBlog(new Blog() { Title = group.Name, GroupId = groupId }, groupId);
            }

            return this.RedirectToAction("GroupRoom", new { id = groupId });
        }

        public ActionResult ShowGroupDetails(int groupId, int page = 1)
        {
            var vm = new GroupDetailsViewModel();

            vm.Group = _groupService.GetGroup(groupId);
            vm.GroupUsers = _groupService.GetUsersForGroup(groupId).ToPagedList(page, 10);

            if (_logService.GetUserId(User.Identity.Name) == vm.Group.OwnerId)
            {
                vm.currentUserIsOwner = true;
            }

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

            return RedirectToAction("GroupRoom", new { id = groupId });
        }

        public ActionResult GroupRoom(int id, int page = 1)
        {
            var vm = new GroupDetailsViewModel()
            {
                Group = _groupService.GetGroup(id),
                GroupUsers = _groupService.GetUsersForGroup(id).ToPagedList(page, 10),
                LastLogs = _logService.GetGroupHistory(id),
                TatalBalnce = _logService.GetCurrentBalance(id),
            };

            var userId = _logService.GetUserId(User.Identity.Name);

            vm.Blog = _BlogService.GetBlog(vm.Group.BlogId);
            if (vm.Blog != null)
            {
                vm.Blog.Posts = vm.Blog.Posts.Where(x => !x.IsDone).ToList();

                Parallel.ForEach(vm.Blog.Posts, (i) => IsVoted(i, userId));
                Parallel.ForEach(vm.Blog.Posts, (i) => CountOfNotTakeATest(i, vm.GroupUsers.Count));

                vm.Blog.Posts = vm.Blog.Posts.OrderByDescending(x => x.ApprovedList.Count).ToList();
            }

            if (userId == vm.Group.OwnerId)
            {
                vm.currentUserIsOwner = true;
            }

            vm.currentUserIsMember = _logService.IsUserMember(id, userId);

            return this.View(vm);
        }

        public ActionResult ChangeGroupName(int id, string newName)
        {
            var group = _groupService.GetGroup(id);
            group.Name = newName;
            _groupService.EditGroupDetails(group);

            return this.RedirectToAction("GroupRoom", new { id });

        }

        public ActionResult SendMails(string title, string message, int id = 1)
        {
            _groupService.Spam(id, title, message);

            return this.RedirectToAction("GroupRoom", new { id });
        }

        private void CountOfNotTakeATest(Post post, int userCount)
        {
            post.NotTakeATest = userCount - post.NotApprovedList.Count - post.ApprovedList.Count;
        }

        private void IsVoted(Post post, int userId)
        {
            if (post.ApprovedList.Contains(userId) || post.NotApprovedList.Contains(userId))
            {
                post.IsVoted = true;
            }
            else
            {
                post.IsVoted = false;
            }
        }
    }
}
