var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => new
{
    name = "TheSingularityWorkshop.FSM_REST",
    role = "transport-host",
    status = "alpha",
    reflection = "OpenAPI 3.x"
});

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
