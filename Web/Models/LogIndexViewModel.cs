using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class LogIndexViewModel
    {
        public LogIndexViewModel()
        {
            Logs = new List<Log>();
            Group = new Group();
        }

        public decimal TatalBalnce { get; set; }

        public List<Log> Logs { get; set; }

        public Group Group { get; set; }

        public bool IsGroupOwner { get; set; }
    }
}