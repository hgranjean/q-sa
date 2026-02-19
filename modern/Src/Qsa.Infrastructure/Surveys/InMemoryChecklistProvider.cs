using Qsa.Application.Common.Interfaces;
using Qsa.Domain.Surveys;

namespace Qsa.Infrastructure.Surveys;

public sealed class InMemoryChecklistProvider : IChecklistProvider
{
    private readonly ISurveyRepository _surveyRepository;
    private readonly Dictionary<Guid, SurveyChecklist> _checklists = new();

    public InMemoryChecklistProvider(ISurveyRepository surveyRepository)
    {
        _surveyRepository = surveyRepository;
        SeedChecklists();
    }

    private void SeedChecklists()
    {
        var surveyIds = new[]
        {
            Guid.Parse("a1000001-0000-0000-0000-000000000001"),
            Guid.Parse("a1000002-0000-0000-0000-000000000002"),
            Guid.Parse("a1000003-0000-0000-0000-000000000003"),
            Guid.Parse("a1000004-0000-0000-0000-000000000004"),
            Guid.Parse("a1000005-0000-0000-0000-000000000005"),
            Guid.Parse("a1000006-0000-0000-0000-000000000006"),
            InMemorySurveyRepository.E2ESurveyId,
        };
        foreach (var surveyId in surveyIds)
        {
            var items = SeedItemsForSurvey(surveyId);
            _checklists[surveyId] = new SurveyChecklist { SurveyId = surveyId, Items = items };
        }
    }

    private static List<ChecklistItem> SeedItemsForSurvey(Guid surveyId)
    {
        var prefix = surveyId.ToString("N")[..8];
        var list = new List<ChecklistItem>();
        var templates = new[]
        {
            ("Hand hygiene stations properly stocked", true, 1),
            ("Fire extinguishers accessible and current inspection", true, 2),
            ("Emergency exits clearly marked", true, 3),
            ("Storage areas organized", true, 4),
            ("Documentation available and up to date", true, 5),
            ("Staff training records current", true, 6),
            ("PPE available at point of use", true, 7),
            ("Waste segregation observed", false, 8),
            ("Cleaning schedule posted", false, 9),
            ("Safety data sheets accessible", true, 10),
            ("No obstructions in corridors", false, 11),
            ("Infection control signage visible", true, 12),
        };
        for (var i = 0; i < templates.Length; i++)
        {
            var (text, required, order) = templates[i];
            var id = Guid.Parse($"{prefix}-0000-0000-0000-{i + 1:D12}");
            list.Add(new ChecklistItem { Id = id, Text = text, IsRequired = required, SortOrder = order });
        }
        return list;
    }

    public Task<SurveyChecklist?> GetChecklistAsync(Guid surveyId, CancellationToken cancellationToken = default)
    {
        _checklists.TryGetValue(surveyId, out var checklist);
        return Task.FromResult(checklist);
    }
}
