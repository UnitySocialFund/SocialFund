using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModel.BlogModel
{
    public class Blog : EntityBase
    {
        public Blog()
        {
            this.Posts = new List<Post>();
        }

        [StringLength(150)]
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        public ICollection<Post> Posts { get; set; }

        public int GroupId { get; set; }

    }
}
