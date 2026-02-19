namespace Qsa.Application.Surveys;

public sealed class ChecklistValidationException : Exception
{
    public IReadOnlyList<Guid> MissingRequiredItemIds { get; }

    public ChecklistValidationException(string message, IReadOnlyList<Guid> missingRequiredItemIds)
        : base(message)
    {
        MissingRequiredItemIds = missingRequiredItemIds;
    }
}
