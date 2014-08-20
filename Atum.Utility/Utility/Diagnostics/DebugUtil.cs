using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Utility.Diagnostics
{
    /// <summary>
    /// Conditional wrapper around Debug class. Only applies to Debug builds
    /// </summary>
    public class DebugUtil
    {
        [Conditional("DEBUG")]
        public static void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
        [Conditional("DEBUG")]
        public static void WriteLine(string message, params object[] args)
        {
            Debug.WriteLine(message, args);
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message)
        {
            Debug.Assert(condition, message);
        }
    }
}
