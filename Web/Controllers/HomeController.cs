using System.Collections.Generic;
using DataModel.BlogModel;
using Services;
using System.Web.Mvc;
using SocialFund.Models.GroupViewModel;
using System;
using System.Collections.ObjectModel;

namespace SocialFund.Controllers
{
    public class HomeController : Controller
    {
        #region Controller methods

        public ActionResult Index(string message)
        {
            // Just for demo data
            var group = new GroupService().GetGroup(1);
            if (group.BlogId == null && User.Identity.IsAuthenticated)
            {
                var blog = new Blog()
                {
                    Title = group.Name,
                    GroupId = group.Id,
                    Posts = new List<Post>()
                    {
                        new Post()
                        {
                        Title = "I need a car!",
                        Description = "I want to buy a car! And I need the money.",
                        Author = User.Identity.Name,
                        ApprovedList = new Collection<int>(),
                        NotApprovedList = new Collection<int>(),
                        NotTakeATest = new GroupService().GetUsersForGroup(group.Id).Count,
                        CommentCount = 3,
                        Comments = new List<Comment>()
                        {
                            new Comment()
                            {
                                Author = User.Identity.Name,
                                Content = "No problem!"
                            },

                            new Comment()
                            {
                                Author = "User10",
                                Content = "Are you crazy?",
                                Comments = new List<Comment>()
                                {
                                    new Comment()
                                    {
                                        Author = "User16",
                                        Content = "No!",
                                    }
                                }
                            }
                        }
                        },
                        new Post()
                        {
                        Title = "A wont to know where is my money!",
                        Description = "Just text for test.",
                        Author = "admin",
                        ApprovedList = new Collection<int>(),
                        NotApprovedList = new Collection<int>(),
                        NotTakeATest = new GroupService().GetUsersForGroup(group.Id).Count
                        }
                    }
                };


                new BlogService().CreateBlog(blog, group.Id);    
            }



            ViewData["Message"] = "Welcome to Social Fund!";    
            if (!String.IsNullOrEmpty(message))
            {
                ViewData["Message"] = message;
            }

            return View();
        }

        public PartialViewResult GroupList()
        {
            var _groupService = new GroupService();
            var _logService = new LogService();
            var groups = _groupService.GetGroupForUser(User.Identity.Name);
            int curentUserId = _logService.GetUserId(User.Identity.Name);
            var vm = new GroupListViewModel();
            
            foreach (var group in groups)
            {
                if (group.OwnerId == curentUserId)
                {
                    vm.MyGroups.Add(group);
                }
                else
                {
                    vm.InvitedGroups.Add(group);
                }
            }
            return this.PartialView("GroupList", vm);
        }

        #endregion
    }
}
