using System;
using System.Collections.Generic;
using System.Text;

using Atum.Domain.Basis;

namespace Atum.Domain.Business
{
    [Serializable]
    public class ZipGeoData : DomainObject
    {
        string _stateAbbrev;
        string _zip;
        object _state;//State with Neighbors
        double _latitude;
        double _longitude;

        public string StateAbbrev { get { return _stateAbbrev; } set { _stateAbbrev = value; } }
        public string Zip { get { return _zip; } set { _zip = value; } }
        public object State { get { return _state; } set { _state = value; } }//State with Neighbors
        public double Latitude { get { return _latitude; } set { _latitude = value; } }
        public double Longitude { get { return _longitude; } set { _longitude = value; } }

        protected override void SetId(long Id)
        {
            this._id = Id;
        }
    }
}
