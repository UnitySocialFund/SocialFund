﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModel;

namespace SocialFund.Models.Account
{
    public class UserDetailsViewModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}