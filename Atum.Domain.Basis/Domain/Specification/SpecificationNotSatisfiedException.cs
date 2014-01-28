using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Specification
{
    public sealed class SpecificationNotSatisfiedException : Exception
    {
        public SpecificationNotSatisfiedException()
            : base("Specification Not Satisfied")
        {
        }

    }
}
