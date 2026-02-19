using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Qsa.Application.Auth;
using Qsa.Application.Surveys;
using Qsa.Application.Surveys.Commands;
using Qsa.Application.Surveys.Queries;
using Qsa.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quality Smart Assistant API",
        Version = "v1",
        Description = "Quality Management API for assessments, assignments, responses, and completion reporting."
    });
});

builder.Services.AddQsaInfrastructure(builder.Configuration);
builder.Services.AddScoped<AuthenticateDevUserCommandHandler>();
builder.Services.AddScoped<GetCurrentUserQueryHandler>();
builder.Services.AddScoped<ListAssignedSurveysQueryHandler>();
builder.Services.AddScoped<GetSurveyQueryHandler>();
builder.Services.AddScoped<GetSurveyChecklistQueryHandler>();
builder.Services.AddScoped<UpsertChecklistResponseCommandHandler>();
builder.Services.AddScoped<SubmitSurveyCommandHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// --- Auth slice: dev login, current user, health ---
app.MapPost("/auth/dev-login", async (DevLoginRequest request, AuthenticateDevUserCommandHandler handler, CancellationToken ct) =>
{
    var result = await handler.HandleAsync(new AuthenticateDevUserCommand(request.Email, request.Role), ct);
    if (result == null)
        return Results.NotFound(new ErrorResponse(new ErrorBody("auth_error", "Unknown user or dev auth disabled.", null)));
    return Results.Ok(new DevLoginResponse(result.Token, result.User));
})
.WithName("DevLogin")
.WithOpenApi();

app.MapGet("/me", [Authorize] async (GetCurrentUserQueryHandler handler, CancellationToken ct) =>
{
    var user = await handler.HandleAsync(new GetCurrentUserQuery(), ct);
    if (user == null)
        return Results.Unauthorized();
    return Results.Ok(user);
})
.WithName("GetCurrentUser")
.WithOpenApi();

app.MapGet("/auth/me", [Authorize] async (GetCurrentUserQueryHandler handler, CancellationToken ct) =>
{
    var user = await handler.HandleAsync(new GetCurrentUserQuery(), ct);
    if (user == null)
        return Results.Unauthorized();
    return Results.Ok(new AuthMeResponse(user.Id, user.Email, user.DisplayName, [user.Role], [], [], DateTimeOffset.UtcNow));
})
.WithName("GetAuthMe")
.WithOpenApi();

app.MapGet("/health", () => Results.Ok(new { status = "ready" }))
.WithName("Health")
.WithOpenApi();

app.MapGet("/auth/health", (IConfiguration config) =>
{
    var useDevAuth = config.GetValue<bool>("Auth:UseDevAuth");
    return Results.Ok(new AuthHealthResponse(useDevAuth, useDevAuth ? "Stub" : "None"));
})
.WithName("AuthHealth")
.WithOpenApi();

app.MapGet("/vp/ping", (HttpContext ctx) =>
{
    // In Development, allow unauthenticated ping so browser/health checks get 200
    if (app.Environment.IsDevelopment() && ctx.User.Identity?.IsAuthenticated != true)
        return Results.Ok(new { role = "VP", message = "pong" });
    if (ctx.User.Identity?.IsAuthenticated != true)
        return Results.Unauthorized();
    if (!ctx.User.IsInRole("VP"))
    {
        // In Development, return 403 with actual role so we can debug
        var roleClaim = ctx.User.FindFirst("role")?.Value ?? "(no role claim)";
        if (app.Environment.IsDevelopment())
            return Results.Json(new { error = "VP role required", actualRole = roleClaim }, statusCode: 403);
        return Results.Forbid();
    }
    return Results.Ok(new { role = "VP", message = "pong" });
})
.AllowAnonymous()
.WithName("VpPing").WithOpenApi();
app.MapGet("/manager/ping", [Authorize(Policy = "ManagerOnly")] () => Results.Ok(new { role = "Manager", message = "pong" }))
.WithName("ManagerPing").WithOpenApi();
app.MapGet("/surveyor/ping", [Authorize(Policy = "SurveyorOnly")] () => Results.Ok(new { role = "Surveyor", message = "pong" }))
.WithName("SurveyorPing").WithOpenApi();

// --- Surveys slice: assigned list + detail (Surveyor only) ---
app.MapGet("/surveys/assigned", [Authorize(Policy = "SurveyorOnly")] async (ListAssignedSurveysQueryHandler handler, CancellationToken ct) =>
{
    try
    {
        var list = await handler.HandleAsync(new ListAssignedSurveysQuery(), ct);
        return Results.Ok(list);
    }
    catch (UnauthorizedAccessException)
    {
        return Results.Forbid();
    }
})
.WithName("ListAssignedSurveys")
.WithOpenApi();

app.MapGet("/surveys/{id:guid}", [Authorize(Policy = "SurveyorOnly")] async (Guid id, GetSurveyQueryHandler handler, CancellationToken ct) =>
{
    try
    {
        var survey = await handler.HandleAsync(new GetSurveyQuery(id), ct);
        if (survey == null)
            return Results.NotFound();
        return Results.Ok(survey);
    }
    catch (UnauthorizedAccessException)
    {
        return Results.Forbid();
    }
})
.WithName("GetSurvey")
.WithOpenApi();

// --- Surveys slice 3: checklist + responses + submit (Surveyor only) ---
app.MapGet("/surveys/{id:guid}/checklist", [Authorize(Policy = "SurveyorOnly")] async (Guid id, GetSurveyChecklistQueryHandler handler, CancellationToken ct) =>
{
    try
    {
        var result = await handler.HandleAsync(new GetSurveyChecklistQuery(id), ct);
        if (result == null)
            return Results.NotFound();
        return Results.Ok(result);
    }
    catch (UnauthorizedAccessException)
    {
        return Results.Forbid();
    }
})
.WithName("GetSurveyChecklist")
.WithOpenApi();

app.MapPut("/surveys/{id:guid}/responses/{itemId:guid}", [Authorize(Policy = "SurveyorOnly")] async (Guid id, Guid itemId, UpsertChecklistResponseRequest body, UpsertChecklistResponseCommandHandler handler, CancellationToken ct) =>
{
    try
    {
        var result = await handler.HandleAsync(new UpsertChecklistResponseCommand(id, itemId, body.Value, body.Notes), ct);
        return Results.Ok(result);
    }
    catch (UnauthorizedAccessException)
    {
        return Results.Forbid();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new ErrorResponse(new ErrorBody("validation_error", ex.Message, null)));
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new ErrorResponse(new ErrorBody("invalid_operation", ex.Message, null)));
    }
})
.WithName("UpsertChecklistResponse")
.WithOpenApi();

app.MapPost("/surveys/{id:guid}/submit", [Authorize(Policy = "SurveyorOnly")] async (Guid id, SubmitSurveyCommandHandler handler, CancellationToken ct) =>
{
    try
    {
        var result = await handler.HandleAsync(new SubmitSurveyCommand(id), ct);
        return Results.Ok(result);
    }
    catch (UnauthorizedAccessException)
    {
        return Results.Forbid();
    }
    catch (ChecklistValidationException ex)
    {
        return Results.BadRequest(new ValidationErrorDto("missing_required", ex.Message, ex.MissingRequiredItemIds.Select(g => g.ToString()).ToArray()));
    }
})
.WithName("SubmitSurvey")
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
        return Results.BadRequest(new ErrorResponse(new ErrorBody("validation_error", "publishNotes is required.", ["publishNotes"])));
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

// Auth slice (API DTOs; Application.UserDto used for user in response)
public sealed record DevLoginRequest(string Email, string? Role);

public sealed record DevLoginResponse(string Token, Qsa.Application.Auth.UserDto User);

public sealed record AuthHealthResponse(bool UseDevAuth, string AuthMode);

// Surveys slice 3 (checklist)
public sealed record UpsertChecklistResponseRequest(string Value, string? Notes);
