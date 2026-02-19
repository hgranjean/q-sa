namespace Qsa.Application.Surveys;

public sealed record SurveyChecklistDto(
    string SurveyId,
    string Status,
    ChecklistItemDto[] Items,
    ChecklistResponseDto[] Responses);

public sealed record ChecklistItemDto(string Id, string Text, bool IsRequired, int SortOrder);

public sealed record ChecklistResponseDto(string ItemId, string Value, string? Notes, string UpdatedAt);

public sealed record ResponseSavedDto(string ItemId, string UpdatedAt);

public sealed record SubmitResultDto(string SurveyId, string Status, string SubmittedAt);

public sealed record ValidationErrorDto(string Code, string Message, string[] MissingRequiredItemIds);
