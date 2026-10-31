namespace Inspecciones.Services;

public interface IInspectionScoringService
{
    decimal CalculateScore(IEnumerable<(decimal Points, bool Required, string Value)> answers);
    string EvaluateSeverity(decimal questionPoints);
    string GetScoreCategory(decimal score);
}

public sealed class InspectionScoringService : IInspectionScoringService
{
    public decimal CalculateScore(IEnumerable<(decimal Points, bool Required, string Value)> answers)
    {
        var items = answers.ToList();
        var totalPoints = items.Where(x => x.Required).Sum(x => x.Points);
        if (totalPoints == 0) return 100m;

        var earned = items.Where(x => x.Required && x.Value == "Conforme").Sum(x => x.Points);
        return decimal.Round(earned / totalPoints * 100m, 2);
    }

    public string EvaluateSeverity(decimal questionPoints)
    {
        if (questionPoints >= 5) return "Alta";
        if (questionPoints >= 3) return "Media";
        return "Baja";
    }

    public string GetScoreCategory(decimal score)
    {
        if (score >= 90) return "Excelente";
        if (score >= 75) return "Aceptable";
        if (score >= 60) return "Observado";
        return "Crítico";
    }
}
