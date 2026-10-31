using Preparation.Shared.Web;

var info = new ProjectInfo("inspections", "Inspecciones", "InspeccionesDb", 5105, "Planifica inspecciones, registra respuestas y sigue hallazgos.");
var builder = WebApplication.CreateBuilder(args);
ProjectBootstrap.Configure(builder, info);
var app = builder.Build();
await ProjectBootstrap.InitializeAsync(app, info);
ProjectBootstrap.ConfigurePipeline(app, info);
app.MapControllers();
await app.RunAsync();
