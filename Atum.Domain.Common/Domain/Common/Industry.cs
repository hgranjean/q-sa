using Atum.Domain.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.Common
{
    public class Industry : DomainObject
    {
        public static readonly List<Industry> AvailableIndustries = new List<Industry>();

        static Industry()
        {
            AvailableIndustries.Add(new Industry("Agriculture / Forestry / Fishing"));
            AvailableIndustries.Add(new Industry("Banking and Capital Markets/Securities"));
            AvailableIndustries.Add(new Industry("Consumer Goods"));
            AvailableIndustries.Add(new Industry("Banking and Capital Markets/Securities"));
            AvailableIndustries.Add(new Industry("Consumer Goods"));
            AvailableIndustries.Add(new Industry("Real Estate / Construction / Architecture / Engineering"));
            AvailableIndustries.Add(new Industry("Government – Federal"));
            AvailableIndustries.Add(new Industry("Government – Health and Human Services"));
            AvailableIndustries.Add(new Industry("Government – State/Province and Local"));
            AvailableIndustries.Add(new Industry("Healthcare – Life Sciences and Pharmaceuticals"));
            AvailableIndustries.Add(new Industry("Healthcare – Payor"));
            AvailableIndustries.Add(new Industry("Healthcare – Provider"));
            AvailableIndustries.Add(new Industry("Hospitality and Travel"));
            AvailableIndustries.Add(new Industry("Insurance"));
            AvailableIndustries.Add(new Industry("Logistics and Transportation"));
            AvailableIndustries.Add(new Industry("Manufacturing"));
            AvailableIndustries.Add(new Industry("Media and Cable"));
            AvailableIndustries.Add(new Industry("Minerals and Mining"));
            AvailableIndustries.Add(new Industry("Non-profit"));
            AvailableIndustries.Add(new Industry("Power and Utilities"));
            AvailableIndustries.Add(new Industry("Professional Services"));
            AvailableIndustries.Add(new Industry("Retail"));
            AvailableIndustries.Add(new Industry("Technology"));
            AvailableIndustries.Add(new Industry("Telecommunications"));
            AvailableIndustries.Add(new Industry("Other"));
        }

        public Industry()
        {
        }

        public Industry(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

    }
}
