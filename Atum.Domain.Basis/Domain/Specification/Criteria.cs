using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

namespace Atum.Domain.Specification
{
	public enum SlctClause
	{
		scTop,
		scDistinct,
		scAll
	}
	/// <summary>
	/// Summary description for Criteria.
	/// </summary>
	public class Criteria : CriterionBase, IDisposable
	{
        private static Criteria _critAll = new Criteria();
        
       
        private string _Quantifier;
        private string _selectClause;
        private int _MaxNumberOfRows;
        private bool _flgTop;
        private bool _flgSelectDistinct;

//		private Hashtable _colCriteria;
		private Queue _colCriteria;

        public Criteria()
	    {
		    //_colCriteria = new Hashtable();
		    _colCriteria = new Queue();
	    }

        public static Criteria All 
        { 
            get 
            {
                _critAll.Add(Constraint.EqualTo("1","1"),false,CritQuantifier.NONE);
                return _critAll; 
            } 
        }

		public string xml
		{    
			get    
			{ 
				string retVal;
				retVal = "<Criteria>";
				foreach(CriterionBase oCriterion in _colCriteria)    
				{
					retVal += oCriterion.xml;
				}
				retVal = retVal + "</Criteria>";
				return retVal; 
			}
		}

		private void Class_Initialize()
		{
//			_colCriteria = new Hashtable();
			_colCriteria = new Queue();
			_flgSelectDistinct = false;
			_flgTop= false;
		}


        public Criterion Add(Constraint constraint, bool IsLiteral, CritQuantifier quant)
		{
			return AddCriterion(constraint, IsLiteral, quant, CritQuantifier.NONE);
		}

        public Criterion AddAnd(Constraint constraint, bool IsLiteral, CritQuantifier quant)
		{
            return AddCriterion(constraint, IsLiteral, quant, CritQuantifier.AND);
		}

        public Criterion AddOr(Constraint constraint, bool IsLiteral, CritQuantifier quant)
		{
            return AddCriterion(constraint, IsLiteral, quant, CritQuantifier.OR);
		}

        //private Criterion AddCriterion(Constraint constraint, bool IsLiteral, CritQuantifier quant, CritQuantifier newquant)
        //{
        //    Criterion criterion = Criterion.Instance(propName, op, propValue, IsLiteral, quant,newquant);

        //    _colCriteria.Enqueue(criterion);
        //    return criterion;
        //}

        private Criterion AddCriterion(Constraint constraint, bool IsLiteral, CritQuantifier quant, CritQuantifier newquant)
        {
            Criterion criterion = Criterion.Instance(constraint.AttributeName, constraint.Operator, constraint.AttributeValue, IsLiteral, quant, newquant);

            _colCriteria.Enqueue(criterion);
            return criterion;
        }
        
        public void Clear()
		{
			_colCriteria.Clear();
		}

        private void SetSelectClause()
		{
			string strTop = "";
			string strDistinct = "";
    
			if( _MaxNumberOfRows > 0)
			{
				strTop = " TOP " + _MaxNumberOfRows + " ";
			}
			if( _flgSelectDistinct)
			{
				strDistinct = " DISTINCT ";
			}
			//_selectClause = StringsHelper.RemoveExtraSpaces(strDistinct + strTop);
		}

		public string SelectClause
		{
			get{return _selectClause;}
		}

		public bool SelectDistinct
		{
			set{_flgSelectDistinct = value;}
		}

		public int MaxNumberOfRows
		{
			set
			{
				_MaxNumberOfRows = value;	
				SetSelectClause();
				_flgTop= true;
			}
		}

		public override string SQLClause
		{
			get
			{
				string retVal = "";
				string sCritClause;
                Criterion oCrit = null;

                if (_colCriteria.Count > 0)
                {
                    oCrit = (Criterion)_colCriteria.Dequeue();
                    while (oCrit != null)
                    {
                        sCritClause = oCrit.SQLClause;
                        //					if(sCritClause.Length > 0)
                        //					{        
                        //retVal = GetQuantifer(oCrit.OuterQuantifier) + retVal; 
                        retVal = retVal + " " + sCritClause;

                        if (_colCriteria.Count > 0)
                        {
                            retVal = retVal + GetQuantifer(oCrit.OuterQuantifier);
                            oCrit = (Criterion)_colCriteria.Dequeue();
                        }
                        else
                        {
                            oCrit = null;
                        }
                        //					}        
                    }
                }
				if(retVal.Length > 0){ retVal = " (" + retVal + ") ";}

				return retVal;
			}
		}

		public override CritQuantifier OuterQuantifier
		{
			get
			{
				return CritQuantifier.NONE;
			}
			set
			{
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			Clear();
			_colCriteria = null;
		}

		#endregion
	}	

}
