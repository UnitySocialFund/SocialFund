//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Group
    {
        public Group()
        {
            this.Group_User = new HashSet<Group_User>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public Nullable<System.Guid> BlogId { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<Group_User> Group_User { get; set; }
    }
}
