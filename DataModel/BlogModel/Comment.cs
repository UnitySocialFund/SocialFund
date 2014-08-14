using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.BlogModel
{
    public class Comment : EntityBase
    {
        public Comment()
        {
            RiplyComments = new List<Comment>();
        }
        [StringLength(50)]
        [Required]
        public string Author { get; set; }

        [StringLength(2500)]
        [Required]
        public string Content { get; set; }

        public List<Comment> RiplyComments { get; set; }
    }
}
