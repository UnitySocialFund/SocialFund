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
            Users = new List<User>();
            Group = new Group();
        }

        public List<User> Users { get; set; }

        public Group Group { get; set; }
    }
}