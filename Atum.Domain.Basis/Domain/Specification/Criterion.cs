using System;
using System.Xml;
using System.Xml.XPath;

namespace Atum.Domain.Specification
{
	/// <summary>
	/// Summary description for Criterion.
	/// </summary>
	public class Criterion : CriterionBase
	{
		private CritQuantifier _outerQuantifier;
		protected string _outerQuantString;

		public static Criterion Instance(string PropName, Ops op, string PropValue, bool IsLiteral,CritQuantifier innerQuant, CritQuantifier outerQuant)
		{
			Criterion retVal = new Criterion();
			retVal.OuterQuantifier = outerQuant;
			retVal.SetCriteria(PropName,op,PropValue,IsLiteral,innerQuant);
			return retVal;
		}

        public static Criterion Instance(string PropName, Ops op, object PropValue, bool IsLiteral, CritQuantifier innerQuant, CritQuantifier outerQuant)
        {
            Criterion retVal = new Criterion();
            retVal.OuterQuantifier = outerQuant;
            retVal.SetCriteria(PropName, op, PropValue.ToString(), IsLiteral, innerQuant);
            return retVal;
        }
        
        public void AddConstraint(string PropName, Ops op, string PropValue, bool IsLiteral)
		{
			base.SetCriteria(PropName, op, PropValue, IsLiteral,InnerQuantifier);
		}

		public override CritQuantifier OuterQuantifier
		{
			get
			{
				return _outerQuantifier;
			}
			set
			{
				_outerQuantifier = value;
				_outerQuantString = GetQuantifer(_outerQuantifier);
			}
		}

		public override string SQLClause
		{
			get
			{
				XmlNodeList ConstraintList;

				string sClause = "";
				string sConstraintList = "";
				ConstraintList = _XMLCrit.SelectNodes("Search/Constraints/Constraint");
				
				foreach(XmlNode ListNode in ConstraintList)
				{
					if(!(sConstraintList.Length > 0))
					{
						sConstraintList = getConstraint(ListNode);
					}
					else
					{
                        string moreConstraints = getConstraint(ListNode);
                        if (moreConstraints.Length > 0)
                        {
                            moreConstraints = _innerQuantifier + moreConstraints;
                        }
                        sConstraintList = sConstraintList + " " + moreConstraints;//_innerQuantString + " " + getConstraint(ListNode);
					}
				}
					
				sClause = sClause + sConstraintList;
	    
				if(sClause.Length > 0) 
				{
					sClause = " (" + sClause + ") ";
				}
	    
				return sClause;
			}
		}		
	}

}
