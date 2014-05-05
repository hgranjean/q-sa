using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    public abstract class RuleSpecReference {

	private RuleSpec _ruleSpec;

	/**
	 * Returns rule specification
	 * 
	 * @return the rule specification
	 */
	public RuleSpec getRuleSpec() {
		return _ruleSpec;
	}

	/**
	 * Sets rule specification
	 * 
	 * @param RuleSpec
	 */
	protected void setRuleSpec(RuleSpec _ruleSpec) {
		this._ruleSpec = _ruleSpec;
	}
}
}
