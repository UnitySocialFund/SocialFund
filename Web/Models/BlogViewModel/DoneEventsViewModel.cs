using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModel.BlogModel;

namespace SocialFund.Models.BlogViewModel
{
    public class DoneEventsViewModel
    {
        public List<Post> Posts { get; set; }
        public Guid BlogId { get; set; }
        public bool CurrentUserIsOwner { get; set; }
        public DoneEventsViewModel()
        {
            Posts = new List<Post>();
        }
    }
}