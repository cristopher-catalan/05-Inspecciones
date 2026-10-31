namespace Inspecciones.Domain;

public sealed record InspectionSummaryDto(
    int Id,
    int SiteId,
    string SiteName,
    int TemplateId,
    string TemplateTitle,
    string Inspector,
    string State,
    DateTimeOffset StartedAt,
    DateTimeOffset? SubmittedAt,
    decimal Score,
    int TotalQuestions,
    int AnsweredQuestions,
    int FindingCount
);

public sealed record ChecklistItemDto(
    int QuestionId,
    string Prompt,
    bool Required,
    decimal Points,
    string? CurrentValue,
    string? CurrentComment,
    decimal CurrentScore
);

public sealed record FindingDetailDto(
    int Id,
    int InspectionId,
    string SiteName,
    string Description,
    string Severity,
    string Owner,
    string State,
    string? CorrectiveAction
);
