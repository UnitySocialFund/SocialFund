using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Common.Validation;

namespace SocialFund.Models.Contacts
{
    public class FeedbackModel
    {
        [Required]
        [ValidateStringLength(2, 50, false)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [ValidateStringLength(1, 250, false)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [ValidateStringLength(1, 2000, false)]
        [DisplayName("Text")]
        public string Text { get; set; }
    }
}