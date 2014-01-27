using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.Surveillance
{
    public enum QuestionType
    {
        YesNo,
        TrueFalse,
        SelectOne,
        SelectMultiple,
        YesNoConditional,
        TrueFalseConditional,
        SelectOneConditional,
        OpenText,
        OpenVariant,
        Ranking
    }
}
