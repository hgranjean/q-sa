using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain.Base
{
    ///
    /// Used for authoring
    public class AuthoringSettings
    {
        public String UmlModelFileName { get; set; }
        public String GenerationOutputFolder { get; set; }

        public AuthoringSettings()
        {
        }
    }
}
