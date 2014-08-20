using System;

namespace Atum.Utility.Calculator
{ 
    /// <summary> 
    /// Summary description for SphericalDistanceCalculator. 
    /// </summary> 
    public class SphericalDistanceCalculator 
    { 
        double _radius; 


        public SphericalDistanceCalculator(double radius) 
        { 
                _radius = radius; 
        } 

        /// <summary>
        /// Returns the Distance between two points(e.g. Longitude & Latitude) on a Sphere
        /// </summary>
        /// <param name="p1">Point representing Source on Sphere</param>
        /// <param name="p2">Point representing Target on Sphere</param>
        /// <param name="radius">Radius of Sphere</param>
        /// <returns>Distance in the unit of supplied Radius</returns>
        public double GetDistance(LongLatPoint p1, LongLatPoint p2) 
        { 
            double retVal; 
            retVal = Math.Sqrt(sinSquaredDeltaPhiOverTwo(p1,p2) + 
            Math.Cos(p1.Phi.RadianValue)*Math.Cos(p2.Phi.RadianValue)*sinSquaredDeltaLamOverTwo(p1,p2));
            retVal = 2 * Math.Asin(retVal) * _radius; 
            return retVal; 
        }
        //3963.0 * arccos[sin(lat1) *  sin(lat2) + cos(lat1) * cos(lat2) * cos(lon2 - lon1)]


        private double sinSquaredDeltaPhiOverTwo(LongLatPoint p1, LongLatPoint p2) 
        { 
                double a = Math.Sin((p2.Phi.RadianValue - p1.Phi.RadianValue)/2); 
                return Math.Pow(a,2); 
        } 

        private double sinSquaredDeltaLamOverTwo(LongLatPoint p1, LongLatPoint p2) 
        { 
            double a = Math.Sin((p2.Lambda.RadianValue - p1.Lambda.RadianValue)/2); 
                return Math.Pow(a,2); 
        } 

    } 

} 

