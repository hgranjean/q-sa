using Atum.Domain.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.Common
{
    [Serializable]
    public class Environment : DomainObject
    {
        public Building Building { get; set; }
        public Floor Floor { get; set; }
        public Area Area { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
