using System;

namespace Atum.Utility.Calculator
{
    /// <summary> 
    /// Summary description for LongLatPoint. 
    /// </summary> 
    public class LongLatPoint
    {
        Angle _phi;
        Angle _lambda;


        public LongLatPoint(Angle phi, Angle lambda)
        {
            _phi = phi;
            _lambda = lambda;
        }
        public Angle Phi { get { return _phi; } }
        public Angle Lambda { get { return _lambda; } }
    }



}

