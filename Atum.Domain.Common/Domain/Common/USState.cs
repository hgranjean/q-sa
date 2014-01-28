using System;
using System.Collections.Generic;
using System.Text;

using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
    [Serializable]public class USState : Domain.Basis.DomainObject
    {
        private long _id;
        private string _name;
        private string _abbrev;
        private USState _northernNeighbor;
        private USState _nEasternNeighbor;
        private USState _easthernNeighbor;
        private USState _sEasternNeighbor;
        private USState _southernNeighbor;
        private USState _sWesternNeighbor;
        private USState _westernNeighbor;
        private USState _nWesternNeighbor;

        public string Name { get { return _name; } set { _name = value; } }
        public string Abbrev { get { return _abbrev; } set { _abbrev = value; } }
        public USState NNeighbor { get { return _northernNeighbor; } set { _northernNeighbor = value; } }
        public USState NENeighbor { get { return _nEasternNeighbor; } set { _nEasternNeighbor = value; } }
        public USState ENeighbor { get { return _easthernNeighbor; } set { _easthernNeighbor = value; } }
        public USState SENeighbor { get { return _sEasternNeighbor; } set { _sEasternNeighbor = value; } }
        public USState SNeighbor { get { return _southernNeighbor; } set {  _southernNeighbor = value; } }
        public USState SWNeighbor { get { return _sWesternNeighbor; } set { _sWesternNeighbor = value; } }
        public USState WNeighbor { get { return _westernNeighbor; } set { _westernNeighbor = value; } }
        public USState NWNeighbor { get { return _nWesternNeighbor; } set { _nWesternNeighbor = value; } }
        public USState[] Neighbors
        {
            get 
            {
                USState[] retVal = {NNeighbor,
                                    SNeighbor,
                                    ENeighbor,
                                    WNeighbor,
                                    NENeighbor,
                                    NWNeighbor,
                                    SENeighbor,
                                    SWNeighbor};
                return retVal;
            }
        }

        public void SetId(long id)
        {
            base._id = id;
        }

        protected override void setId(long id)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override string ToString()
        {
            return this._name;
        }
    }
}
