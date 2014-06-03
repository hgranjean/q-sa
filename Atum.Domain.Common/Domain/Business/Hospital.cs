using System.ComponentModel.DataAnnotations.Schema;
using Atum.Domain.Basis;

namespace Atum.Domain.Business
{
    [Table("Hospitals")]
    public partial class Hospital
    {
        public Hospital()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string DomainName { get; set; }
        public string Industry { get; set; }
    }
}
