namespace Qsa.Domain.Surveys;

public class SurveyChecklist
{
    public required Guid SurveyId { get; init; }
    public required IReadOnlyList<ChecklistItem> Items { get; init; }
}
