using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Security
{
	/// <summary>
	/// Summary description for Assertion.
	/// </summary>
    [Serializable]
    public class Assertion : DomainObject
	{
        //private long _ID;
        private string _assertionName;
        private long _attributeId;
        private string _assertionAttribute;
        private string _logicalOperator;//TODO: CHANGE TO CONSTANTS
        private object _targetValue;
        private long _policyId;
		

        public Assertion()
		{
        }


        #region properties
        //private long _ID;
        public string AssertionName { get { return _assertionName; } set { _assertionName = value; } }
        public long AttributeId { get { return _attributeId; } set { _attributeId = value; } }
        public string AssertionAttribute { get { return _assertionAttribute; } set { _assertionAttribute= value; } }
        public string LogicalOperator { get { return _logicalOperator; } set { _logicalOperator = value; } }
        public object TargetValue { get { return _targetValue; } set { _targetValue = value; } }
        public long PolicyId{get{return _policyId;} set{_policyId=value;}}
        
        #endregion


    }
}
