using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atum.Domain.Security.Domain
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
