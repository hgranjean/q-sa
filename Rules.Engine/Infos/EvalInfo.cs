using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Functions;
using Rules.Engine.Infos;

namespace Rules.Engine
{
    public class EvalInfo : IInfo
    {   
        public EvalInfo(String eval)
        {
            this.Eval = eval;
        }
        
        public String Eval { get; private set; }
    }
}
