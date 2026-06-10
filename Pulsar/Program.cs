using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.FileProviders;

Console.WriteLine((string?)null!);

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "static",
    ContentRootPath = "WebApp",
    ApplicationName = "Pulsar",
    EnvironmentName =
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

builder.Configuration.AddJsonFile(currentDirFileProvider, "appsettings.json", false, true);
builder.Configuration.AddJsonFile(currentDirFileProvider,
    $"appsettings.{builder.Environment.EnvironmentName.ToLowerInvariant()}.json", true, true);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<PulsarConfiguration>(builder.Configuration.GetSection("Pulsar"));
builder.Services.Configure<Pulsar.Features.Overlay.OverlayConfiguration>(
    builder.Configuration.GetSection("Pulsar:Overlay"));

var aiConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"] ??
                         builder.Configuration["ApplicationInsights:ConnectionString"];
if (!string.IsNullOrWhiteSpace(aiConnectionString)) builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(new CorsPolicy
        { Origins = { "*" }, Headers = { "*" }, Methods = { "*" } });
});
builder.Services.AddSignalR().AddJsonProtocol(options =>
    options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddDbContext<PulsarContext>();
builder.Services.Configure<JsonOptions>(options =>
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
// builder.Services.AddOpenApiDocument(config => config.DocumentName = "v1");
builder.Services.AddHttpForwarder();
builder.Services.AddSingleton<Pulsar.Features.Overlay.IOverlayStateService, Pulsar.Features.Overlay.OverlayStateService>();
builder.Services.AddHostedService<FileWatcherService>();
builder.Services.AddHostedService<JournalProcessor>();
builder.Services.AddHostedService<Pulsar.Features.Overlay.Native.NativeOverlayService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseHttpsRedirection();
app.UseRouting();
app.UseWebSockets();
app.MapControllers();
app.MapHub<EventsHub>("api/events");
app.MapReverseProxy();
app.MapFallbackToFile("index.html").AllowAnonymous();

await app.Services.GetRequiredService<PulsarContext>().Database.EnsureCreatedAsync();

await app.RunAsync();
