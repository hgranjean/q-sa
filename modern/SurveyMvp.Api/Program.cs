using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/auth/me", () =>
{
    var response = new AuthMeResponse(
        "usr_01JQ8T0B1T1R9D3YQ5K2P9V8M4",
        "manager@hospital.org",
        "Jordan Smith",
        ["Manager"],
        ["hos_01JQ8SYR8W8N7Q4R3P2C1B0A9Z"],
        ["reports.read", "responses.read"],
        DateTimeOffset.UtcNow.AddMinutes(-20));

    return Results.Ok(response);
})
.WithName("GetCurrentUser")
.WithOpenApi();

app.MapPost("/surveys", (CreateSurveyRequest request) =>
{
    var now = DateTimeOffset.UtcNow;
    var response = new SurveyCreatedResponse(
        NewId("srv"),
        "Draft",
        request.Title,
        request.Description,
        request.EffectiveFromUtc,
        request.EffectiveToUtc,
        1,
        "usr_01JQ8T0B1T1R9D3YQ5K2P9V8M4",
        now);

    return Results.Created($"/surveys/{response.Id}", response);
})
.WithName("CreateSurvey")
.WithOpenApi();

app.MapPost("/surveys/{surveyId}/publish", (string surveyId, PublishSurveyRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.PublishNotes))
    {
        return Results.BadRequest(new ErrorResponse(new ErrorBody("validation_error", "publishNotes is required.", ["publishNotes"] )));
    }

    var response = new PublishSurveyResponse(
        surveyId,
        "Published",
        DateTimeOffset.UtcNow,
        "usr_01JQ8T0B1T1R9D3YQ5K2P9V8M4",
        1);

    return Results.Ok(response);
})
.WithName("PublishSurvey")
.WithOpenApi();

app.MapPost("/assignments", (CreateAssignmentRequest request) =>
{
    var response = new AssignmentCreatedResponse(
        NewId("asn"),
        request.SurveyId,
        request.HospitalId,
        request.AssigneeUserId,
        "Assigned",
        request.DueAtUtc,
        DateTimeOffset.UtcNow);

    return Results.Created($"/assignments/{response.Id}", response);
})
.WithName("CreateAssignment")
.WithOpenApi();

app.MapPost("/responses", (CreateResponseRequest request) =>
{
    var status = request.Submit ? "Submitted" : "Draft";
    var submittedAt = request.Submit ? DateTimeOffset.UtcNow : (DateTimeOffset?)null;
    var submittedBy = request.Submit ? "usr_01JQ8W6C9X4V3B2N1M0L9K8J7H" : null;

    var response = new ResponseCreatedResponse(
        NewId("rsp"),
        request.AssignmentId,
        status,
        request.Submit ? new Score(1, 1, 100.0) : null,
        submittedAt,
        submittedBy);

    return Results.Created($"/responses/{response.Id}", response);
})
.WithName("CreateResponse")
.WithOpenApi();

app.MapGet("/reports/completion", (string from, string to, string? hospitalId) =>
{
    if (!DateOnly.TryParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate) ||
        !DateOnly.TryParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate))
    {
        return Results.BadRequest(new ErrorResponse(new ErrorBody("validation_error", "from/to must be yyyy-MM-dd.", ["from", "to"])));
    }

    var item = new CompletionItem(
        "asn_01JQ8WQ0D3F4G5H6J7K8L9M0N1",
        "srv_01JQ8V9Y7E3W2M1N0K9J8H7G6F",
        "Infection Prevention Q1",
        hospitalId ?? "hos_01JQ8SYR8W8N7Q4R3P2C1B0A9Z",
        "usr_01JQ8W6C9X4V3B2N1M0L9K8J7H",
        new DateTimeOffset(2026, 03, 20, 23, 59, 59, TimeSpan.Zero),
        "Submitted",
        new DateTimeOffset(2026, 03, 18, 15, 10, 00, TimeSpan.Zero));

    var response = new CompletionReportResponse(
        fromDate,
        toDate,
        hospitalId,
        new CompletionTotals(12, 3, 9, 1, 75.0),
        [item]);

    return Results.Ok(response);
})
.WithName("GetCompletionReport")
.WithOpenApi();

app.Run();

static string NewId(string prefix) => $"{prefix}_{Guid.NewGuid():N}";

public sealed record AuthMeResponse(
    string Id,
    string Email,
    string DisplayName,
    string[] Roles,
    string[] HospitalIds,
    string[] Permissions,
    DateTimeOffset LastLoginUtc);

public sealed record CreateSurveyRequest(
    string Title,
    string? Description,
    DateTimeOffset? EffectiveFromUtc,
    DateTimeOffset? EffectiveToUtc,
    QuestionGroupInput[] QuestionGroups);

public sealed record QuestionGroupInput(string Title, int SortOrder, QuestionInput[] Questions);

public sealed record QuestionInput(string Code, string Text, string Type, bool IsRequired, int SortOrder);

public sealed record SurveyCreatedResponse(
    string Id,
    string Status,
    string Title,
    string? Description,
    DateTimeOffset? EffectiveFromUtc,
    DateTimeOffset? EffectiveToUtc,
    int Version,
    string CreatedByUserId,
    DateTimeOffset CreatedAtUtc);

public sealed record PublishSurveyRequest(string PublishNotes);

public sealed record PublishSurveyResponse(
    string Id,
    string Status,
    DateTimeOffset PublishedAtUtc,
    string PublishedByUserId,
    int Version);

public sealed record CreateAssignmentRequest(
    string SurveyId,
    string HospitalId,
    string AssigneeUserId,
    DateTimeOffset DueAtUtc,
    string Priority);

public sealed record AssignmentCreatedResponse(
    string Id,
    string SurveyId,
    string HospitalId,
    string AssigneeUserId,
    string Status,
    DateTimeOffset DueAtUtc,
    DateTimeOffset CreatedAtUtc);

public sealed record CreateResponseRequest(string AssignmentId, AnswerInput[] Answers, bool Submit);

public sealed record AnswerInput(string QuestionCode, object? Value, string? Comment);

public sealed record ResponseCreatedResponse(
    string Id,
    string AssignmentId,
    string Status,
    Score? Score,
    DateTimeOffset? SubmittedAtUtc,
    string? SubmittedByUserId);

public sealed record Score(double Max, double Achieved, double Percent);

public sealed record CompletionReportResponse(
    DateOnly From,
    DateOnly To,
    string? HospitalId,
    CompletionTotals Totals,
    CompletionItem[] Items);

public sealed record CompletionTotals(int Assigned, int InProgress, int Submitted, int Overdue, double CompletionRatePercent);

public sealed record CompletionItem(
    string AssignmentId,
    string SurveyId,
    string SurveyTitle,
    string HospitalId,
    string AssigneeUserId,
    DateTimeOffset DueAtUtc,
    string Status,
    DateTimeOffset? SubmittedAtUtc);

public sealed record ErrorResponse(ErrorBody Error);

public sealed record ErrorBody(string Code, string Message, string[]? Details);
