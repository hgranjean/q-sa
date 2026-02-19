using System.Collections.Concurrent;
using Qsa.Application.Common.Interfaces;
using Qsa.Domain.Surveys;

namespace Qsa.Infrastructure.Surveys;

public sealed class InMemorySurveyResponseStore : ISurveyResponseStore
{
    private readonly ConcurrentDictionary<(Guid SurveyId, string UserId, Guid ItemId), SurveyResponse> _store = new();

    public Task<IReadOnlyList<SurveyResponse>> GetResponsesAsync(Guid surveyId, string userId, CancellationToken cancellationToken = default)
    {
        var list = _store.Values
            .Where(r => r.SurveyId == surveyId && r.SurveyorUserId == userId)
            .ToList();
        return Task.FromResult<IReadOnlyList<SurveyResponse>>(list);
    }

    public Task UpsertResponseAsync(SurveyResponse response, CancellationToken cancellationToken = default)
    {
        var key = (response.SurveyId, response.SurveyorUserId, response.ItemId);
        _store[key] = response;
        return Task.CompletedTask;
    }
}
