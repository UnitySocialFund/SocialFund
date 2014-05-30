using DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialFund.Models.LogViewModels
{
    public class AddCoinsViewModel
    {
        public AddCoinsViewModel()
        {
            this.Log = new Log();
            this.Users = new List<User>();
        }

        public Log Log { get; set; }

        public List<User> Users { get; set; }

        [Required(ErrorMessage = "Please choese user")]
        public string UserName { get; set; }

    }
}