using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atum.Database.Surveillance.Models
{
    [Table("AspNetRoles")]
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            this.AspNetUsers = new List<AspNetUser>();
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
