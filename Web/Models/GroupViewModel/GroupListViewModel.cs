using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModel;

namespace SocialFund.Models.GroupViewModel
{
    public class GroupListViewModel
    {
        public List<Group> MyGroups { get; set; }
        public List<Group> InvitedGroups { get; set; }

        public GroupListViewModel()
        {
            MyGroups = new List<Group>();
            InvitedGroups = new List<Group>();
        }
    }
}