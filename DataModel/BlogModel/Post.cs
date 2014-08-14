using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataModel.BlogModel
{
    public class Post : EntityBase
    {
        public Post()
        {
            this.Comments = new List<Comment>();
            ApprovedList = new List<int>();
            NotApprovedList = new List<int>();
        }
        [StringLength(150)]
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        [StringLength(100)]
        public string Author { get; set; }

        [StringLength(1500)]
        [Required(ErrorMessage = "*")]
        public string ShortContent { get; set; }

        public string Content { get; set; }

        public int NotTakeATest { get; set; }

        public int Force { get; set; }

        public bool IsDone { get; set; }

        public ICollection<int> ApprovedList { get; set; }
        public ICollection<int> NotApprovedList { get; set; }
        public bool IsVoted { get; set; }

        public ICollection<Comment> Comments { get; set; }

    }
}
