using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModel.BlogModel;

namespace SocialFund.Models.BlogViewModel
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public Guid BlogId { get; set; }
        public int GroupId { get; set; }
        public bool CurrentUserIsOwner { get; set; }
    }
}