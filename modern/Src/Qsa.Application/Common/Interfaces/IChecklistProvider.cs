using Qsa.Domain.Surveys;

namespace Qsa.Application.Common.Interfaces;

public interface IChecklistProvider
{
    Task<SurveyChecklist?> GetChecklistAsync(Guid surveyId, CancellationToken cancellationToken = default);
}
