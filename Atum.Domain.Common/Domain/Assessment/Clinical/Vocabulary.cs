using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Clinical
{
	/// <summary>
	/// Summary description for Vocabulary.
	/// Maps to ICD - Specialized for Site/Practice
	/// </summary>
	public class Vocabulary : DomainObject
	{
		public Vocabulary()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        protected override void setId(long id)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
