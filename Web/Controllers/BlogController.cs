using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using DataModel.BlogModel;
using Services;
using SocialFund.Models.BlogViewModel;

namespace SocialFund.Controllers
{
    public class BlogController : Controller
    {
        private readonly GroupService _groupService;
        private readonly LogService _logService;

        public BlogController()
        {
            _groupService = new GroupService();
            _logService = new LogService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Post(Guid blogId, Guid postId)
        {
            var blog = new BlogService().GetBlog(blogId);
            var userId = _logService.GetUserId(User.Identity.Name);
            var groupOwnerId = _groupService.GetGroup(blog.GroupId).OwnerId;
            PostDetailsViewModel vm = new PostDetailsViewModel()
            {
                GroupId = blog.GroupId,
                BlogId = blog.Id,
                Post = new BlogService().GetPost(blog.Id, postId),
                CurrentUserIsOwner = userId == groupOwnerId
            };

            if (vm.Post.ApprovedList.Contains(userId) || vm.Post.NotApprovedList.Contains(userId))
            {
                vm.Post.IsVoted = true;
            }

            return View(vm);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add(Guid id)
        {
            var vm = new PostCreateViewModel();
            vm.GroupId = Convert.ToInt32(Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1]);
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(PostCreateViewModel model, Guid id)
        {
            if (ModelState.IsValid)
            {
                var post = new Post()
                {
                    Title = model.Title,
                    ShortContent = model.ShortContent,
                    Content = model.Content,
                    Author = User.Identity.Name,
                    ApprovedList = new Collection<int>(),
                    NotApprovedList = new Collection<int>(),
                    NotTakeATest = _groupService.GetUsersForGroup(model.GroupId).Count
                };
                new BlogService().AddPost(id, post);

                return RedirectToAction("GroupRoom", "Group", new { id = model.GroupId });
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(Guid blogId, Guid postId)
        {
            var post = new BlogService().GetPost(blogId, postId);
            var vm = new PostEditViewModel()
            {
                BlogId = blogId,
                PostId = postId,
                Title = post.Title,
                ShortContent = post.ShortContent,
                Content = post.Content,
                Modified = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(PostEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var _blogService = new BlogService();
                var post = _blogService.GetPost(model.BlogId, model.PostId);

                post.Title = model.Title;
                post.ShortContent = model.ShortContent;
                post.Content = model.Content;
                post.Modified = model.Modified;

                _blogService.UpdatePost(model.BlogId, post);
                var groupId = _blogService.GetBlog(model.BlogId).GroupId;
                return RedirectToAction("GroupRoom", "Group", new { id = groupId});
            }

            return View(model);
        }

        [Authorize]
        public ActionResult Remove(Guid blogId, Guid postId)
        {
            new BlogService().RemovePost(blogId, postId);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize]
        public ActionResult InDone(Guid blogId, Guid postId)
        {
            new BlogService().InDonePost(blogId, postId);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize]
        public ActionResult FromDone(Guid blogId, Guid postId)
        {
            new BlogService().FromDonePost(blogId, postId);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddComment(CommentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    Author = User.Identity.Name,
                    Content = model.Content
                };
                new BlogService().AddComment(model.BlogId, model.PostId, comment);
            }
            return RedirectToAction("Post", "Blog", new { blogId = model.BlogId, postId = model.PostId });
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddRiplyComment(CommentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    Author = User.Identity.Name,
                    Content = model.Content
                };
                new BlogService().AddComment(model.BlogId, model.PostId, comment);
            }
            return RedirectToAction("Post", "Blog", new { blogId = model.BlogId, postId = model.PostId });
        }

        public ActionResult Approve(Guid blogId, Guid postId, bool isAprove)
        {
            var _bs = new BlogService();
            var post = _bs.GetPost(blogId, postId);
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

                _bs.UpdatePost(blogId, post);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ChangeMind(Guid blogId, Guid postId)
        {
            var _bs = new BlogService();
            var userId = _logService.GetUserId(User.Identity.Name);
            var post = _bs.GetPost(blogId, postId);
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

            _bs.UpdatePost(blogId, post);

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult HistoryDoneEvents(Guid id)
        {
            var blog = new BlogService().GetBlog(id);
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
