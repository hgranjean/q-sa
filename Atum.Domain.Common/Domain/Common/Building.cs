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
        private string p1;
        private int p2;

        public Building(string name, int Id)
        {
            // TODO: Complete member initialization
            this.SetId(Id);
            this.Name = name;
        }

        protected override void SetId(long id)
        {
            base._id = id;
        }

        public string Name { get; set; }

    }
}
