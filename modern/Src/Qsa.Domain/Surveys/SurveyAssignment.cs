namespace Qsa.Domain.Surveys;

public class SurveyAssignment
{
    public required Guid SurveyId { get; init; }
    public required string SurveyorUserId { get; init; }
    public required DateTime AssignedAt { get; init; }
}
