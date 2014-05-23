using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialFund.Models.GroupViewModel
{
    public class AddUserToGroupViewModel
    {
        public AddUserToGroupViewModel()
        {
            GroupUsers = new List<User>();
            OtherUsers = new List<User>();
            Group = new Group();
        }

        public List<User> GroupUsers { get; set; }

        public List<User> OtherUsers { get; set; }

        public Group Group { get; set; }
    }
}