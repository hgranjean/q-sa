using System;
using System.Collections.Generic;
using System.Text;
using Atum.Domain.Basis;
using Atum.Domain.Common;

namespace Atum.Domain.Healthcare
{
    /*Class Comments*/
    [Serializable]
    public class Facility  : DomainObject
    {
        private string p1;
        private int p2;

        public Facility()
        {
        }
        public Facility(string guid)//:base(guid)
        {
        }

        public Facility(string name, int Id)
        {
           this.SetId(Id);
            this.Name = name;
        }

        protected override void SetId(long id)
        {
            ID = id;
        }

    	//Attributes go here.
		public string Name { get; set; }
		public virtual  List<ContactInfo> ContactInfos { get; set; }
		public FacilityType Type { get; set; }
		public virtual List<Document> Documents { get; set; }
        public virtual ContactInfo PrimaryContactInfo { get; set; }

        public Building Building { get; set; }

       public int NetworkId { get; set; }

       public long OwnerId { get; set; }

    }
}
