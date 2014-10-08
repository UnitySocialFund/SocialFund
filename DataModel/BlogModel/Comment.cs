using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModel.BlogModel
{
    public class Comment : EntityBase
    {
        public Comment()
        {
            Comments = new List<Comment>();
        }

        [StringLength(50)]
        [Required]
        public string Author { get; set; }

        [StringLength(2500)]
        [Required]
        public string Content { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
