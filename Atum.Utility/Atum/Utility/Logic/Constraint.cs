using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Utility.Logic
{
	/// <summary>
	/// Summary description for Constraint.
	/// </summary>
	public class Constraint
	{
        string _attributeName;
        object _attributeValue;
        Ops _operator;
        
        private Constraint(string attributeName, string attributeValue, Ops op)
		{
            _attributeName = attributeName;
            _attributeValue = attributeValue;
            _operator = op;
		}

		public static Constraint EqualTo(string attributeName, string attributeValue)
		{
            return new Constraint(attributeName, attributeValue, Ops.EqualTo);
        }
        public Constraint GreaterThan(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.GreaterThan);
        }
        public static Constraint LessThan(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.LessThan);
        }
        public static Constraint StartsWith(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.StartsWith);
        }
        public static Constraint EndsWith(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.EndsWith);
        }
        public static Constraint LessThanOrEqualTo(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.LessThanOrEqualTo);
        }
        public static Constraint GreaterThanOrEqualTo(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.GreaterThanOrEqualTo);
		}
        public static Constraint Contains(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.Contains);
        }
        public static Constraint IsNull(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.IsNull);
        }
        public static Constraint IsNotNull(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.IsNotNull);
        }
        public static Constraint NotEqualTo(string attributeName, string attributeValue)		
		{
            return new Constraint(attributeName, attributeValue, Ops.NotEqualTo);
        }

        public string AttributeName
        {
            get
            {
                return _attributeName;
            }
        }
        public object AttributeValue
        {
            get
            {
                return _attributeValue;
            }
        }
        public Ops Operator
        {
            get 
            {
                return _operator;
            }
        }
        
	}
}
