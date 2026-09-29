var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => new
{
    name = "TheSingularityWorkshop.FSM_REST",
    role = "transport-host",
    status = "alpha",
    description = "Hosts an experimental HTTP surface for exercising the FSM_REST transport boundary."
});

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
