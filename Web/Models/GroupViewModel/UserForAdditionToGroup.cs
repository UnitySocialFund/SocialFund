using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModel;
using PagedList;

namespace SocialFund.Models.GroupViewModel
{
    public class UserForAdditionToGroup
    {
        public UserForAdditionToGroup(int groupId)
        {
            this.GroupId = groupId;
        }

        public IPagedList<User> UsersPaged { get; set; } 
        public int GroupId { get; set; }
    }
}