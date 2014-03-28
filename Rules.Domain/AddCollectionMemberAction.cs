using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    public class AddCollectionMemberAction : RuleFunctionBase
    {
        public String Target { get; set; }
    }
}
