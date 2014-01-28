using System;

namespace Atum.Repository.Basis
{
	/// <summary>
	/// Summary description for Criterion.
	/// </summary>
	public class Criterion
	{
        string _attributeName;
        object _attributeValue;
        object _operator;

        private Criterion(string attributeName, string attributeValue, object op)
		{
            _attributeName = attributeName;
            _attributeValue = attributeValue;
            _operator = op;
		}


		public static Criterion EqualTo(string attributeName, string attributeValue)
		{
            return new Criterion(attributeName, attributeValue, '=');
        }
        public Criterion GreaterThan(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
        }
        public static Criterion LessThan(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
        }
        public static Criterion StartsWith(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
        }
        public static Criterion EndsWith(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
        }
        public static Criterion LessThanOrEqualTo(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
        }
        public static Criterion GreaterThanOrEqualTo(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
		}
        public static Criterion Contains(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
        }
        public static Criterion IsNull(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
        }
        public static Criterion IsNotNull(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
        }
        public static Criterion NotEqualTo(string attributeName, string attributeValue)		
		{
            return new Criterion(attributeName, attributeValue, '=');
        }

	}
}
