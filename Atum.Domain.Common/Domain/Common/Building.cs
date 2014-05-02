using Atum.Domain.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.Common
{
    [Serializable]
    public class Building : DomainObject
    {
        public Building()
        {
        }

        public Building(string name, int Id)
        {
            this.SetId(Id);
            this.Name = name;
        }

        protected override void SetId(long id)
        {
            ID = id;
        }

        public string Name { get; set; }

    }
}
