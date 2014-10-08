using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialFund.Models.BlogViewModel
{
    public class CommentCreateViewModel
    {
        [StringLength(2500)]
        [Required(ErrorMessage = "*")]
        public string Content { get; set; }

        public Guid BlogId { get; set; }
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
    }
}