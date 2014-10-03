using System;
using System.ComponentModel.DataAnnotations;

namespace DataModel.BlogModel
{
    public class EntityBase
    {
        public EntityBase()
        {
            this.Id = Guid.NewGuid();
            this.Created = DateTime.Now;
        }

        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}
