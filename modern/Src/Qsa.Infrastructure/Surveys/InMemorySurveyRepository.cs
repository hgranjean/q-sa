using Qsa.Application.Common.Interfaces;
using Qsa.Domain.Surveys;

namespace Qsa.Infrastructure.Surveys;

public sealed class InMemorySurveyRepository : ISurveyRepository
{
    private readonly List<Survey> _surveys = new();
    private readonly List<SurveyAssignment> _assignments = new();

    public InMemorySurveyRepository()
    {
        Seed();
    }

    public static readonly Guid E2ESurveyId = Guid.Parse("e2e00001-0000-0000-0000-000000000001");
    public const string E2ESurveyTitle = "E2E Survey A";

    private void Seed()
    {
        var baseDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var surveys = new List<Survey>
        {
            new() { Id = Guid.Parse("a1000001-0000-0000-0000-000000000001"), Title = "Facility Audit – Chicago West", LocationName = "Chicago West", DueDate = baseDate.AddDays(-2), Status = SurveyStatus.Completed, Priority = SurveyPriority.High },
            new() { Id = Guid.Parse("a1000002-0000-0000-0000-000000000002"), Title = "Infection Prevention Q1", LocationName = "Springfield Central", DueDate = baseDate.AddDays(3), Status = SurveyStatus.InProgress, Priority = SurveyPriority.High },
            new() { Id = Guid.Parse("a1000003-0000-0000-0000-000000000003"), Title = "Safety Compliance Review", LocationName = "Peoria North", DueDate = baseDate.AddDays(7), Status = SurveyStatus.NotStarted, Priority = SurveyPriority.Medium },
            new() { Id = Guid.Parse("a1000004-0000-0000-0000-000000000004"), Title = "Environmental Survey – Building A", LocationName = "Chicago East", DueDate = baseDate.AddDays(1), Status = SurveyStatus.NotStarted, Priority = SurveyPriority.Medium },
            new() { Id = Guid.Parse("a1000005-0000-0000-0000-000000000005"), Title = "Quarterly Quality Check", LocationName = "Rockford", DueDate = baseDate.AddDays(14), Status = SurveyStatus.NotStarted, Priority = SurveyPriority.Low },
            new() { Id = Guid.Parse("a1000006-0000-0000-0000-000000000006"), Title = "Facility Audit – Chicago West (Follow-up)", LocationName = "Chicago West", DueDate = baseDate.AddDays(5), Status = SurveyStatus.Submitted, Priority = SurveyPriority.High },
            new() { Id = E2ESurveyId, Title = E2ESurveyTitle, LocationName = "E2E Site", DueDate = baseDate.AddDays(30), Status = SurveyStatus.NotStarted, Priority = SurveyPriority.High },
        };
        _surveys.AddRange(surveys);

        var assignedAt = DateTime.UtcNow.AddDays(-10);
        var assignments = new List<SurveyAssignment>
            {
                new() { SurveyId = surveys[0].Id, SurveyorUserId = "usr_svy_01", AssignedAt = assignedAt },
                new() { SurveyId = surveys[1].Id, SurveyorUserId = "usr_svy_01", AssignedAt = assignedAt.AddDays(1) },
                new() { SurveyId = surveys[2].Id, SurveyorUserId = "usr_svy_01", AssignedAt = assignedAt.AddDays(2) },
                new() { SurveyId = surveys[3].Id, SurveyorUserId = "usr_svy_02", AssignedAt = assignedAt.AddDays(1) },
                new() { SurveyId = surveys[4].Id, SurveyorUserId = "usr_svy_02", AssignedAt = assignedAt.AddDays(3) },
                new() { SurveyId = surveys[5].Id, SurveyorUserId = "usr_svy_02", AssignedAt = assignedAt.AddDays(2) },
                new() { SurveyId = E2ESurveyId, SurveyorUserId = "usr_svy_01", AssignedAt = assignedAt },
            };
        _assignments.AddRange(assignments);
    }

    public Task<IReadOnlyList<(Survey Survey, DateTime AssignedAt)>> ListAssignedSurveysAsync(string surveyorUserId, CancellationToken cancellationToken = default)
    {
        var surveyIds = _assignments.Where(a => a.SurveyorUserId == surveyorUserId).Select(a => a.SurveyId).ToHashSet();
        var surveys = _surveys.Where(s => surveyIds.Contains(s.Id)).ToDictionary(s => s.Id);
        var results = _assignments
            .Where(a => a.SurveyorUserId == surveyorUserId && surveys.ContainsKey(a.SurveyId))
            .Select(a => (surveys[a.SurveyId], a.AssignedAt))
            .ToList();
        return Task.FromResult<IReadOnlyList<(Survey, DateTime)>>(results);
    }

    public Task<Survey?> GetSurveyByIdAsync(Guid surveyId, CancellationToken cancellationToken = default)
    {
        var survey = _surveys.Find(s => s.Id == surveyId);
        return Task.FromResult(survey);
    }

    public Task<bool> IsSurveyAssignedToUserAsync(Guid surveyId, string userId, CancellationToken cancellationToken = default)
    {
        var assigned = _assignments.Exists(a => a.SurveyId == surveyId && a.SurveyorUserId == userId);
        return Task.FromResult(assigned);
    }

    public Task<DateTime?> GetAssignedAtAsync(Guid surveyId, string userId, CancellationToken cancellationToken = default)
    {
        var a = _assignments.FirstOrDefault(x => x.SurveyId == surveyId && x.SurveyorUserId == userId);
        return Task.FromResult(a != null ? (DateTime?)a.AssignedAt : null);
    }

    public Task SetSurveyStatusAsync(Guid surveyId, SurveyStatus status, CancellationToken cancellationToken = default)
    {
        var survey = _surveys.Find(s => s.Id == surveyId);
        if (survey != null)
            survey.Status = status;
        return Task.CompletedTask;
    }
}
