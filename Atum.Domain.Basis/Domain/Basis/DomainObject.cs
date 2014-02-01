using System;

namespace Atum.Domain.Basis
{
	/// <summary>
	/// Summary description for DomainObject.
	/// </summary>
	public abstract class DomainObject
	{
		protected long _id;

        //public DomainObject()
        //{
        //}
		public long ID { get{return _id;} }
        protected abstract void SetId(long id);
	}
}
