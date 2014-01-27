using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    public abstract class RuleSpecificationReference {

	private RuleSpecification ruleSpecification;

	/**
	 * Returns rule specification
	 * 
	 * @return the rule specification
	 */
	public RuleSpecification getRuleSpecification() {
		return ruleSpecification;
	}

	/**
	 * Sets rule specification
	 * 
	 * @param ruleSpecification
	 */
	protected void setRuleSpecification(RuleSpecification ruleSpecification) {
		this.ruleSpecification = ruleSpecification;
	}
}
}
