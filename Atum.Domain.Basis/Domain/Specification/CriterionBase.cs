using System;
using System.Xml;
using System.Xml.XPath;

namespace Atum.Domain.Specification
{
	public enum Ops
	{
		EqualTo,
		GreaterThan,
		LessThan,
		StartsWith,
		EndsWith,
		LessThanOrEqualTo,
		GreaterThanOrEqualTo,
		Contains,
		IsNull,
		IsNotNull,
		NotEqualTo
	}

	public enum TypeCasting
	{
		Money,
		Int,
		Float, 
		Varchar
	}

	public enum CritQuantifier
	{
		OR, AND, NONE
	}
	
	/// <summary>
	/// Summary description for Criterion.
	/// </summary>
	
	public abstract class CriterionBase
	{
		protected XmlDocument _XMLCrit;
		protected CritQuantifier _innerQuantifier;
		protected string _innerQuantString;

		protected const string QUANT_OR = "OR";
		protected const string QUANT_AND = "AND";

		protected CriterionBase()
		{
			initXml();
		}
//		protected CriterionBase(string PropName, Ops op, string PropValue): this(PropName,op,PropValue,false,Quantifier.AND){}
//		protected CriterionBase(string PropName, Ops op, string PropValue, bool IsLiteral): this(PropName,op,PropValue,IsLiteral,Quantifier.AND){}
//		protected CriterionBase(string PropName, Ops op, string PropValue, bool IsLiteral,Quantifier quant)
//		{
//			initXml();
//			SetCriteria(PropName,op,PropValue,IsLiteral,quant);
//		}

		public CritQuantifier InnerQuantifier
		{
			get
			{
				return _innerQuantifier;
			}
			set
			{
				_innerQuantifier = value;
				_innerQuantString = GetQuantifer(_innerQuantifier);
			}
		}

		public abstract CritQuantifier OuterQuantifier{get;set;}

		public abstract string SQLClause{get;}
	
		public string xml
		{
			get{return this._XMLCrit.OuterXml;}
		}
		
		private string initXml()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(@"<Search ObjectName='Transaction'>");
			sb.Append(@"<Constraints Quantifier=''>");
			sb.Append(@"<Constraint PropName='' PropValue='' Operator='' IsLiteral=''/>");
			sb.Append(@"</Constraints>");
			sb.Append(@"</Search>");

			return sb.ToString();
		}
		private void CriteriaInitialize()
		{
			_XMLCrit = new XmlDocument();
			_XMLCrit.LoadXml(initXml());
			//_innerQuantString = QUANT_AND;

			//if(Quantifier==CritQuantifier.OR){_innerQuantString = QUANT_OR;}
		}

		private string getOperatorString(Ops op)
		{
			string sOperator = "";
			switch(op)
			{
				case Ops.EqualTo:
					sOperator = "=";
					break;
				case Ops.GreaterThan:
					sOperator = "lt";
					break;
				case Ops.LessThan:
					sOperator = "gt";
					break;
				case Ops.NotEqualTo:
					sOperator = "ne";
					break;
				case Ops.StartsWith:
					sOperator = "sw";
					break;
				case Ops.EndsWith:
					sOperator = "ew";
					break;
				case Ops.Contains:
					sOperator = "cn";
					break;
				case Ops.GreaterThanOrEqualTo:
					sOperator = "ge";
					break;
				case Ops.LessThanOrEqualTo:
					sOperator = "le";
					break;
				case Ops.IsNull:
					sOperator = "null";
					break;
				case Ops.IsNotNull:
					sOperator = "notnull";
					break;
			}
    
			return sOperator;
		}

		private void insertCriteria(string propName, Ops op, string propValue, bool isLiteral)
		{
			XmlNode ConstNode;
			XmlNode MyNewNode;
			XmlNode Root;
			XmlNode CurrNode;
			XmlNamedNodeMap NmdNodeMap;

			Root = _XMLCrit.DocumentElement;
			ConstNode = _XMLCrit.SelectSingleNode("/Search/Constraints");
        
			CurrNode = ConstNode.ChildNodes.Item(0);
			MyNewNode = CurrNode.CloneNode(false);
			NmdNodeMap = MyNewNode.Attributes;

			foreach(XmlAttribute att in NmdNodeMap)
			{
				switch(att.Name)
				{
					case "PropName":
						att.Value = @propName;
						break;
					case  "PropValue":
						att.Value = @propValue;
						break;
					case "Operator":
						att.Value = @getOperatorString(op);
						break;
					case  "IsLiteral":
						att.Value = @isLiteral.ToString();
						break;
				}
			}
			
			ConstNode.AppendChild(MyNewNode);
		}

		protected string GetQuantifer(CritQuantifier quant)
		{
			string retVal = QUANT_AND;
			switch(quant)
			{
				case CritQuantifier.AND:
					retVal = QUANT_AND;
					break;
				case CritQuantifier.OR:
					retVal = QUANT_OR;
					break;
				case CritQuantifier.NONE:
					retVal = "";
					break;
			}
			return retVal;
		}

		protected void SetCriteria(string PropName, Ops op, string PropValue, bool IsLiteral, CritQuantifier quant)
		{
			if(_XMLCrit==null)
			{
				CriteriaInitialize();
			}
			InnerQuantifier = quant;

			insertCriteria(PropName, op, PropValue, IsLiteral);
		}
		protected string getConstraint(System.Xml.XmlNode ConstraintNode)
		{
			//TypeCasting ValueType = TypeCasting;
			XmlNamedNodeMap NmdNodeMap = ConstraintNode.Attributes;
			
			string sOperator = "";
			string sValue = "";
			string sConstraint = NmdNodeMap.GetNamedItem("PropName").Value;
			string OpAtt = NmdNodeMap.GetNamedItem("Operator").Value;
			string sIsLiteral = NmdNodeMap.GetNamedItem("IsLiteral").Value;


            bool blnLiteral = false;
            if ((sIsLiteral.Length > 0))
            {
                blnLiteral = bool.Parse(sIsLiteral);
            }
			
			switch(OpAtt)
			{
				case "ew":
					sOperator = " LIKE " + "'%";
					sValue = NmdNodeMap.GetNamedItem("PropValue").Value + "' ";
					break;
				case "sw":
					sOperator = " LIKE " + "'";
					sValue = NmdNodeMap.GetNamedItem("PropValue").Value + "%' ";
					break;
				case "cn":
					sOperator = " LIKE " + "'%";
					sValue = NmdNodeMap.GetNamedItem("PropValue").Value + "%' ";
					break;
				case "lt":
					sOperator = " < ";
					sValue = NmdNodeMap.GetNamedItem("PropValue").Value;
					if(!blnLiteral){sValue = "'" + sValue + "'";}
					break;
				case "gt":
					sOperator = " > ";
					sValue = NmdNodeMap.GetNamedItem("PropValue").Value;
					if(!blnLiteral){sValue = "'" + sValue + "'";}
					break;
				case "le":
					sOperator = " <= ";
					sValue = NmdNodeMap.GetNamedItem("PropValue").Value;
					if(!blnLiteral){sValue = "'" + sValue + "'";}
					break;
				case "ge":
					sOperator = " >= ";
					sValue = NmdNodeMap.GetNamedItem("PropValue").Value;
					if(!blnLiteral){sValue = "'" + sValue + "'";}
					break;
				case "ne":
					sOperator = " <> ";
					sValue = NmdNodeMap.GetNamedItem("PropValue").Value;
					if(!blnLiteral){sValue = "'" + sValue + "'";}
					break;
				case "=":
					sOperator = NmdNodeMap.GetNamedItem("Operator").Value;
					sValue = NmdNodeMap.GetNamedItem("PropValue").Value;
					if(!blnLiteral){sValue = "'" + sValue + "'";}
					//if(sValue=="True" || sValue=="False") {sValue = "'" + sValue + "'";}
					break;
				case "null":
					sOperator = " is null ";
					break;
				case "notnull":
					sOperator = " is not null ";
					break;
			}
			sConstraint = sConstraint + sOperator;
			sConstraint = sConstraint + sValue;
			return sConstraint;
		}	 
			
	}
}
