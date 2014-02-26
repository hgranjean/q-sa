using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atum.Database.Surveillance.Models
{
    [Table("AspNetUsers")]
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserClaims = new List<AspNetUserClaim>();
            this.AspNetUserLogins = new List<AspNetUserLogin>();
            this.AspNetRoles = new List<AspNetRole>();
            this.Hospitals = new HashSet<Hospital>();
        }

        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Discriminator { get; set; }
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
        public virtual ICollection<Hospital> Hospitals { get; set; }
    }
}
