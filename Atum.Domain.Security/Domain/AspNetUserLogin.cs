using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atum.Domain.Security.Domain
{
    [Table("AspNetUserLogins")]
    public partial class AspNetUserLogin
    {
        [Key]
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
