
namespace Atum.Domain.Surveillance
{
    [System.Serializable]
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
