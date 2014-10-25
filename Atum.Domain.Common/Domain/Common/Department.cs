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
            this.Id = Id;
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
