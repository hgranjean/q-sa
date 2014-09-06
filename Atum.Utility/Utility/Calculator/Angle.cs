using System;

namespace Atum.Utility.Calculator
{
    public enum AngleUnits
    {
        degrees, radians
    }


    /// <summary> 
    /// Summary description for Angle. 
    /// </summary> 
    public class Angle
    {
        int _degree;
        int _minute;
        int _second;
        double _degreesDec;
        double _radians;


        public Angle(int degree, int minute, int second)
        {
            _degree = degree;
            _minute = minute;
            _second = second;
            _degreesDec = degree + minute / 60 + second / (3600);
        }


        public Angle(double angleValue, AngleUnits unit)
        {
            switch (unit)
            {
                case AngleUnits.degrees:
                    AngleDegresDec(angleValue);
                    break;
                case AngleUnits.radians:
                    AngleRadians(angleValue);
                    break;
            }
        }


        public void AngleDegresDec(double degreesDec)
        {
            _degreesDec = degreesDec;
            _radians = degreesDec * System.Math.PI / 180;
        }


        public void AngleRadians(double radians)
        {
            _radians = radians;
        }


        public double RadianValue
        {
            get
            {
                return _radians;
            }
        }
    }


}