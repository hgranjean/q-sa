using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Atum.Domain.Business;
using Atum.Domain.Common;

namespace Atum.Domain.Security.Domain
{
    [Table("AspNetUsers")]
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserClaims = new List<AspNetUserClaim>();
            // this.AspNetUserLogins = new List<AspNetUserLogin>();
            // this.AspNetRoles = new List<AspNetRole>();
            // this.Hospitals = new HashSet<Hospital>();
            // this.Person = new Person();
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
        public string PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
