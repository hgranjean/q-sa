using System;
using System.Collections;


namespace Atum.Domain.Basis
{
	/// <summary>
	/// Summary description for DomainCollection.
	/// </summary>
	public abstract class DomainCollection : CollectionBase
	{
		private object _key;
		
		public DomainCollection()
		{
		}

		protected void setKey()
		{
			_key = this.GetHashCode();
		}

		public object Key{get {return _key;}}


	}
}
