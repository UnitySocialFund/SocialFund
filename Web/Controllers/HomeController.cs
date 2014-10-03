using Services;
using System.Web.Mvc;
using SocialFund.Models.GroupViewModel;

namespace SocialFund.Controllers
{
    public class HomeController : Controller
    {
        #region Controller methods

        public ActionResult Index()
        {
            
            ViewData["Message"] = "Welcome to Social Fund!";

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
