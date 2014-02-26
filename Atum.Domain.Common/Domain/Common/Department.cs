using Atum.Domain.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.Common
{
    [Serializable]
    public class Department : DomainObject
    {
        public Department(string name, int Id)
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
