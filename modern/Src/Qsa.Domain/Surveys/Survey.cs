namespace Qsa.Domain.Surveys;

public class Survey
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string? LocationName { get; init; }
    public required DateOnly DueDate { get; init; }
    public required SurveyStatus Status { get; set; }
    public required SurveyPriority Priority { get; init; }
}
