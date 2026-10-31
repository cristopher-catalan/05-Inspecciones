using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Preparation.Shared.Data;

namespace Inspecciones.Endpoints;

public static class InspectionEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/inspections-mgmt").RequireAuthorization();

        group.MapGet("/overview", async (AppDbContext db, ClaimsPrincipal user) =>
        {
            var myUsername = user.Identity?.Name ?? "";
            var query = db.Inspections.AsQueryable();

            if (user.IsInRole("Inspector"))
                query = query.Where(x => x.Inspector == myUsername);

            var items = await (
                from i in query
                join s in db.Sites on i.SiteId equals s.Id
                join t in db.InspectionTemplates on i.TemplateId equals t.Id
                select new
                {
                    i.Id,
                    i.SiteId,
                    SiteName = s.Name,
                    SiteCode = s.Code,
                    i.TemplateId,
                    TemplateTitle = t.Title,
                    i.Inspector,
                    i.State,
                    i.StartedAt,
                    i.SubmittedAt,
                    i.Score
                }
            ).OrderByDescending(x => x.StartedAt).ToListAsync();

            var ids = items.Select(x => x.Id).ToList();
            var findings = await db.Findings.Where(x => ids.Contains(x.InspectionId)).ToListAsync();

            var result = items.Select(x => new
            {
                x.Id,
                x.SiteId,
                x.SiteName,
                x.SiteCode,
                x.TemplateId,
                x.TemplateTitle,
                x.Inspector,
                x.State,
                x.StartedAt,
                x.SubmittedAt,
                x.Score,
                OpenFindings = findings.Count(f => f.InspectionId == x.Id && f.State != "Cerrado")
            });

            return Results.Ok(result);
        });

        group.MapGet("/open-findings", async (AppDbContext db, ClaimsPrincipal user) =>
        {
            var findings = await (
                from f in db.Findings.Where(x => x.State != "Cerrado")
                join i in db.Inspections on f.InspectionId equals i.Id
                join s in db.Sites on i.SiteId equals s.Id
                select new
                {
                    f.Id,
                    f.InspectionId,
                    SiteName = s.Name,
                    f.Severity,
                    f.Description,
                    f.Owner,
                    f.State
                }
            ).ToListAsync();

            return Results.Ok(findings);
        });
    }
}
