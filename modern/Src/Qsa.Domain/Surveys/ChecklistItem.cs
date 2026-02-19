namespace Qsa.Domain.Surveys;

public class ChecklistItem
{
    public required Guid Id { get; init; }
    public required string Text { get; init; }
    public required bool IsRequired { get; init; }
    public required int SortOrder { get; init; }
}
