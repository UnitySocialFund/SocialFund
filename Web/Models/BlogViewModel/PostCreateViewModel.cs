﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.BlogModel;

namespace SocialFund.Models.BlogViewModel
{
    public class PostCreateViewModel
    {
        [StringLength(150)]
        [Required(ErrorMessage = "Must be filled out")]
        public string Title { get; set; }

        [StringLength(1500)]
        [Required(ErrorMessage = "Must be filled out")]
        public string ShortContent { get; set; }

        [StringLength(2500)]
        [Required(ErrorMessage = "Must be filled out")]
        public string Content { get; set; }

        public int GroupId { get; set; }
    }
}
