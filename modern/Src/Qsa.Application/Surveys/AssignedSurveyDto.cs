namespace Qsa.Application.Surveys;

public sealed record AssignedSurveyDto(
    string Id,
    string Title,
    string DueDate,
    string Status,
    string Priority,
    string? LocationName,
    string AssignedAt);
