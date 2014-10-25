using System;
using System.Collections.Generic;
using System.Text;
using Atum.Domain.Basis;

namespace Atum.Domain.Healthcare
{
    /*Class Comments*/
    [Serializable]
    public class FacilityType : DomainObject
    {
        public FacilityType()
        {
        }
    

		//Attributes go here.
		public long Value { get; set; }
		public string Name { get; set; }
		public DateTime LastUpdatedDate { get; set; }
		public long LastUpdatedUserId { get; set; }
		
		

    }
}
