using Inspecciones.Endpoints;
using Inspecciones.Services;
using Inspecciones.UI;
using Preparation.Shared.Web;

var info = new ProjectInfo("inspections", "Inspecciones", "InspeccionesDb", 5105, "Planifica inspecciones, completa listas y atiende hallazgos.");
var builder = WebApplication.CreateBuilder(args);

// Register inspection scoring domain service
builder.Services.AddSingleton<IInspectionScoringService, InspectionScoringService>();

ProjectBootstrap.Configure(builder, info);
var app = builder.Build();
await ProjectBootstrap.InitializeAsync(app, info);
ProjectBootstrap.ConfigurePipeline(app, info);

// Map Inspection enhanced endpoints and register modern UI layout
InspectionEndpoints.Map(app);
DashboardRenderer.RegisterCustomView(info.Key, InspectionLayout.Render);
app.MapControllers();

await app.RunAsync();
