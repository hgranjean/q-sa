namespace Qsa.Application.Surveys;

public sealed record SurveyDetailDto(
    string Id,
    string Title,
    string DueDate,
    string Status,
    string Priority,
    string? LocationName,
    string? AssignedAt);
