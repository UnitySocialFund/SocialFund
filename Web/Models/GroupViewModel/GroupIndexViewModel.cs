using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialFund.Models.GroupViewModel
{
    public class GroupIndexViewModel
    {
        public GroupIndexViewModel()
        {
            this.MyGroups = new List<Group>();
            this.InvitedGroup = new List<Group>();
        }

        public List<Group> MyGroups { get; set; }

        public List<Group> InvitedGroup { get; set; }
    }
}