using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Address.
	/// </summary>
	public class Address : DomainObject
	{
        private string _street1;
        private string _street2;
        private string _city;
        private string _state;
        private string _zip;

        
        public Address()
		{
		}

        public string Street1 { get { return _street1; } set { _street1 = value;} }
        public string Street2 { get { return _street2; } set { _street2 = value;} }
        public string City { get { return _city; } set { _city  = value; } }
        public string State { get { return _state; } set { _state = value; } }
        public string Zip { get { return _zip; } set { _zip = value; } }
        public double DistanceToSomeTarget;

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(Street1 + "\n");
            if(Street2!=null)
            sb.Append(Street2 + "\n");
            sb.Append(City + "\n");
            sb.Append(State + "\n");
            sb.Append(Zip + "\n");
            
            return sb.ToString();
        }

        protected override void setId(long id)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
