using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using DataModel.BlogModel;
using PagedList;

namespace SocialFund.Models.GroupViewModel
{
    public class GroupDetailsViewModel
    {
        private List<Log> lastLogs;
        public GroupDetailsViewModel()
        {
            this.Group = new Group();
            this.lastLogs = new List<Log>();
        }

        public Blog Blog { get; set; }
        public Group Group { get; set; }
        
        public decimal TatalBalnce { get; set; }
        
        public IPagedList<User> GroupUsers { get; set; }
        public bool currentUserIsOwner { get; set; }
        public bool currentUserIsMember { get; set; }


        public List<Log> LastLogs
        {
            get { return lastLogs; }
            set {
                lastLogs = value.Count > 13 ? value.GetRange(value.Count - 13, 13) : value;
            }
        }
    }
}
