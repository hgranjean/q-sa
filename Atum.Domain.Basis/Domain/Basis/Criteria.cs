using System;

namespace Atum.Domain.Basis
{
	/// <summary>
	/// Summary description for Criteria.
	/// </summary>
	public class Criteria
	{
		public Criteria()
		{
		}

        public void And(Criterion crit)
        { }
        public void And(Criteria crits)
        { }

        public void Or(Criterion crit)
        { }
        public void Or(Criteria crits)
        { }


	}
}
