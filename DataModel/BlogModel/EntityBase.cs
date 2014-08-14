using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
