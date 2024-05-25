using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Pulsar.Features;
using Pulsar.Features.Journal;

Console.WriteLine((string?)null!);

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args, WebRootPath = "static", ContentRootPath = "WebApp", ApplicationName = "Pulsar", EnvironmentName =
#if DEBUG
        "Development"
#else
        "Production"
#endif 
    
});

var currentDirFileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

builder.Host.UseLamar((_, registry) => registry.Scan(scan =>
{
    scan.AssemblyContainingType<Program>();
    scan.WithDefaultConventions();
    scan.LookForRegistries();
}));

builder.Configuration.AddJsonFile(currentDirFileProvider,"appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile(currentDirFileProvider, $"appsettings.{builder.Environment.EnvironmentName.ToLowerInvariant()}.json", optional: true, reloadOnChange: true);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<PulsarConfiguration>(builder.Configuration.GetSection("Pulsar"));

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(new CorsPolicy()
        { Origins = { "http://172.31.0.222:5000", "http://localhost:5000" }, Headers = { "*" }, Methods = { "*" } });
});
builder.Services.AddSignalR().AddJsonProtocol(options =>
    options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddDbContext<PulsarContext>();
builder.Services.Configure<JsonOptions>(options =>
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
// builder.Services.AddOpenApiDocument(config => config.DocumentName = "v1");
builder.Services.AddHttpForwarder();
builder.Services.AddHostedService<FileWatcherService>();
builder.Services.AddHostedService<JournalProcessor>();

var app = builder.Build();
app.UseWebSockets();
// app.UseOpenApi();
// app.UseSwaggerUi();
app.UseRouting();   
app.MapReverseProxy();
app.MapControllers();
app.MapHub<EventsHub>("api/events");
app.MapFallbackToFile("index.html").AllowAnonymous();

await app.Services.GetRequiredService<PulsarContext>().Database.EnsureCreatedAsync();

await app.RunAsync();