﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using DataModel.BlogModel;
using Services;
using SocialFund.Models.BlogViewModel;

namespace SocialFund.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly UserService _userService;
        private readonly GroupService _groupService;
        private readonly LogService _logService;
        private readonly BlogService _blogService;

        public BlogController()
        {
            _groupService = new GroupService();
            _userService = new UserService();
            _logService = new LogService();
            _blogService = new BlogService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int groupId)
        {
            var group = _groupService.GetGroup(groupId);
            
            _blogService.CreateBlog(new Blog() { Title = group.Name, GroupId = groupId }, groupId);

            return RedirectToAction("GroupRoom", "Group", new { id = groupId });
        }

        public ActionResult Post(Guid blogId, Guid postId)
        {
            var blog = _blogService.GetBlog(blogId);
            var userId = _logService.GetUserId(User.Identity.Name);
            var groupOwnerId = _groupService.GetGroup(blog.GroupId).OwnerId;
            
            var vm = new PostDetailsViewModel()
            {
                GroupId = blog.GroupId,
                BlogId = blog.Id,
                Post = _blogService.GetPost(blog.Id, postId),
                CurrentUserIsOwner = userId == groupOwnerId
            };

            if (vm.Post.ApprovedList.Contains(userId) || vm.Post.NotApprovedList.Contains(userId))
            {
                vm.Post.IsVoted = true;
            }

            return View(vm);
        }

        [HttpGet]
        public ActionResult Add(Guid id)
        {
            var vm = new PostCreateViewModel();

            vm.GroupId = Convert.ToInt32(Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1]);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Add(PostCreateViewModel model, Guid id)
        {
            if (ModelState.IsValid)
            {
                var blog = _blogService.GetBlog(id);
                var user = _userService.GetUser(User.Identity.Name);

                if (_groupService.IsMember(blog.GroupId, user.Id))
                {
                    var post = new Post()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Author = User.Identity.Name,
                        ApprovedList = new Collection<int>(),
                        NotApprovedList = new Collection<int>(),
                        NotTakeATest = _groupService.GetUsersForGroup(model.GroupId).Count
                    };
                    _blogService.AddPost(id, post);

                    return RedirectToAction("GroupRoom", "Group", new { id = model.GroupId });
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { message = "You do not have enought permissions." });
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult Edit(Guid blogId, Guid postId)
        {
            var post = _blogService.GetPost(blogId, postId);
            
            var vm = new PostEditViewModel()
            {
                BlogId = blogId,
                PostId = postId,
                Title = post.Title,
                Description = post.Description,
                Modified = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(PostEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = _blogService.GetPost(model.BlogId, model.PostId);

                post.Title = model.Title;
                post.Description = model.Description;
                post.Modified = model.Modified;

                _blogService.UpdatePost(model.BlogId, post);

                var groupId = _blogService.GetBlog(model.BlogId).GroupId;
                
                return RedirectToAction("GroupRoom", "Group", new { id = groupId});
            }

            return View(model);
        }

        public ActionResult Remove(Guid blogId, Guid postId)
        {
            _blogService.RemovePost(blogId, postId);

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult InDone(Guid blogId, Guid postId)
        {
            _blogService.InDonePost(blogId, postId);

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult FromDone(Guid blogId, Guid postId)
        {
            _blogService.FromDonePost(blogId, postId);

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult AddComment(CommentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    Author = User.Identity.Name,
                    Content = model.Content
                };

                _blogService.AddComment(model.BlogId, model.PostId, comment);
            }

            return RedirectToAction("Post", "Blog", new { blogId = model.BlogId, postId = model.PostId });
        }

        [HttpPost]
        public ActionResult AddReplyComment(CommentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    Author = User.Identity.Name,
                    Content = model.Content
                };

                _blogService.AddReplyComment(model.BlogId, model.PostId, model.CommentId, comment);
            }

            return RedirectToAction("Post", "Blog", new { blogId = model.BlogId, postId = model.PostId });
        }

        [HttpPost]
        public ActionResult AddRiplyComment(CommentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    Author = User.Identity.Name,
                    Content = model.Content
                };

                _blogService.AddComment(model.BlogId, model.PostId, comment);
            }

            return RedirectToAction("Post", "Blog", new { blogId = model.BlogId, postId = model.PostId });
        }

        public ActionResult Approve(Guid blogId, Guid postId, bool isAprove)
        {
            var post = _blogService.GetPost(blogId, postId);
            
            if (post.NotTakeATest != 0)
            {
                if (isAprove)
                {
                    post.ApprovedList.Add(_logService.GetUserId(User.Identity.Name));
                }
                else
                {
                    post.NotApprovedList.Add(_logService.GetUserId(User.Identity.Name));
                }
                post.NotTakeATest--;

                _blogService.UpdatePost(blogId, post);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ChangeMind(Guid blogId, Guid postId)
        {
            var userId = _logService.GetUserId(User.Identity.Name);
            var post = _blogService.GetPost(blogId, postId);
            
            if (post.ApprovedList.Contains(userId))
            {
                post.NotTakeATest++;
                post.ApprovedList.Remove(userId);

            }
            else if (post.NotApprovedList.Contains(userId))
            {
                post.NotTakeATest++;
                post.NotApprovedList.Remove(userId);
            }

            _blogService.UpdatePost(blogId, post);

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult HistoryDoneEvents(Guid id)
        {
            var blog = _blogService.GetBlog(id);
            var userId = _logService.GetUserId(User.Identity.Name);
            var groupOwnerId = _groupService.GetGroup(blog.GroupId).OwnerId;
            
            var vm = new DoneEventsViewModel()
            {
                Posts = blog.Posts.Where(x => x.IsDone).ToList(),
                CurrentUserIsOwner = userId == groupOwnerId,
                BlogId = id
            };
            
            return this.View(vm);
        }
    }
}
