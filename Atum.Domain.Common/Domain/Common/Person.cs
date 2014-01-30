using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Person.
	/// </summary>
	[Serializable]public class Person : DomainObject
	{
		protected string _lastName;
		protected string _firstName;
		protected string _middleName;
		protected string _ssn;
		protected string _suffix;
		protected Address _address;
        protected DateTime _dob;

		public Person()
		{
		}

		public string LastName{get{return _lastName;}set{_lastName = value;}}
		public string FirstName{get{return _firstName;}set{_firstName = value;}}
		public string MiddleName{get{return _middleName;}set{_middleName = value;}}
		public string Suffix{get{return _suffix;}set{_suffix = value;}}
		public string SSN{get{return _ssn;}set{_ssn = value;}}
		public Address Address{get{return _address;}set{_address = value;}}
        public DateTime DateOfBirth { get { return _dob; } set { _dob = value; } }


        protected override void setId(long id)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
