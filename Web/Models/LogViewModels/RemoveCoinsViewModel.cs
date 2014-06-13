using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataModel;

namespace SocialFund.Models.LogViewModels
{
    public class RemoveCoinsViewModel
    {
        public RemoveCoinsViewModel()
        {
            this.Log = new Log();
        }

        public Log Log { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Please choese user")]
        public string UserName { get; set; }
    }
}