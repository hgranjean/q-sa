using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Atum.Domain.Basis;
using Atum.Domain.Business;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Person.
	/// </summary>
	[Table("Persons")]
    public partial class Person
	{
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string JobTitle { get; set; }
		public Address Address { get; set; }
        public DateTime DateOfBirth { get; set;}
        public virtual Hospital Hospital { get; set; }
        public string HospitalId { get; set; }

        public string FullName
        {
            get { return String.Format("{0} {1} {2}", this.FirstName, this.MiddleName, this.LastName); }
        }

        public Department Department { get; set; }

        public Person()
        {
        }
    }
}
