using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Pulsar.Features;


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

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(new CorsPolicy()
        { Origins = { "http://localhost:5000" }, Headers = { "*" }, Methods = { "*" } });
});
builder.Services.AddSignalR().AddJsonProtocol(options =>
    options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddDbContext<PulsarContext>();
builder.Services.Configure<JsonOptions>(options =>
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHttpForwarder();
builder.Services.AddHostedService<FileWatcherService>();

var app = builder.Build();

app.UseWebSockets();
app.UseRouting();
app.MapReverseProxy();
app.MapControllers();
app.MapHub<EventsHub>("api/events");
app.MapFallbackToFile("index.html").AllowAnonymous();

await app.RunAsync();