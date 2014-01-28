using System;
using System.Collections.Generic;
using System.Text;

using Atum.Domain.Common;
using Atum.Domain.Specification;
using Atum.Utility.Calculator;
using Atum.Utility.Constants;

namespace Atum.Domain.Business.Specification
{
    public class DistanceToZipSpec : ISpecification
    {
        SphericalDistanceCalculator _calc;
        LongLatPoint _llpTarget;
        double _maxDistance;
        double _radius;
        string _unit;

        public DistanceToZipSpec(string zip, double maxDistance ,PhysicalConstants.EarthRadius r)
        {
            _llpTarget = getLLPForZip(zip.Trim());
            _radius = (double)r;
            _unit = r.ToString();
            _maxDistance = maxDistance;

            _calc = new SphericalDistanceCalculator(_radius);

        }

        #region ISpecification Members

        public bool IsStatisfiedBy(Atum.Domain.Basis.DomainObject anAddress)
        {

            bool retVal = false;
            try
            {
                Address address = (Address)anAddress;
                LongLatPoint sourceZip = getLLPForZip(address.Zip.Trim());
                retVal = (_calc.GetDistance(sourceZip, _llpTarget) < _maxDistance);
            }
            catch
            {
            }
            return retVal;
        }

        #endregion

        private LongLatPoint getLLPForZip(string targetZip)
        {
            try
            {
                return Atum.Repository.Business.LongLatServer.Instance.GetLongLatPointForZip(targetZip);
            }
            catch (Expression e)
            {
                throw e;
            }
            
        }
    }
}
