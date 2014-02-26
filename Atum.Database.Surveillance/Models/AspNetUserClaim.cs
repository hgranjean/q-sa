using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atum.Database.Surveillance.Models
{
    [Table("AspNetUserClaims")]
    public partial class AspNetUserClaim
    {
        [Key]
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string User_Id { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
