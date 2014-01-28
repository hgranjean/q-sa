using System;
using System.Collections.Generic;
using System.Text;

using Atum.Domain.Common;

namespace Atum.Domain.Management
{
    public class Provider : Domain.Basis.DomainObject
    {
        protected string _lastName;
        protected string _firstName;
        protected string _middleName;
        
        private string _certifications;
        private string _classification;

        private Address _address;
        private string _name;
        private Vendor _vendor;


        public void SetId(long Id)
        {
            base._id = Id;
        }

		public string LastName{get{return _lastName;}set{_lastName = value;}}
		public string FirstName{get{return _firstName;}set{_firstName = value;}}
		public string MiddleName{get{return _middleName;}set{_middleName = value;}}
		public string Certifications{get{return _certifications;}set{_certifications = value;}}
		public string Classification{get{return _classification;}set{_classification = value;}}
		//public string Suffix{get{return _suffix;}set{_suffix = value;}}
		//public string SSN{get{return _ssn;}set{_ssn = value;}}
        public double DistanceInfo{get{return _address.DistanceToSomeTarget;}}
		public Address Address{get{return _address;}set{_address = value;}}
        public Vendor Vendor { get { return _vendor; } set { _vendor = value; } }


        protected override void setId(long id)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
