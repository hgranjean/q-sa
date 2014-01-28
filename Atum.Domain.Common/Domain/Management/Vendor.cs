using System;
using System.Collections.Generic;
using System.Text;

using Atum.Domain.Basis;
using Atum.Domain.Common;

namespace Atum.Domain.Management
{
    public class Vendor : DomainObject
    {
        string _vendorName;
        Address _address;
        Contact _primaryContact;

        public string Name { get { return _vendorName; } set { _vendorName = value; } }
        public Address Address { get { return _address; } set { _address = value; } }
        public Contact PrimaryContact { get { return _primaryContact; } set { _primaryContact = value; } }

        protected override void setId(long id)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
