namespace Qsa.Domain.Surveys;

public class SurveyResponse
{
    public required Guid SurveyId { get; init; }
    public required string SurveyorUserId { get; init; }
    public required Guid ItemId { get; init; }
    public required ChecklistResponseValue Value { get; init; }
    public string? Notes { get; init; }
    public required DateTime UpdatedAt { get; init; }
}
