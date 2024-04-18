using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Pulsar.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLamar();
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(new CorsPolicy()
        { Origins = { "http://localhost:5000" }, Headers = { "*" }, Methods = { "*" } });
});
builder.Services.AddSignalR().AddJsonProtocol(options =>
    options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddDbContext<PulsarContext>();
builder.Services.Configure<PulsarConfiguration>(builder.Configuration.GetSection(nameof(Pulsar)));
builder.Services.Configure<JsonOptions>(options =>
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddSpaYarp();

var app = builder.Build();

app.UseRouting();
app.MapReverseProxy();
app.MapControllers();
app.MapDefaultControllerRoute();
app.UseWebSockets();
app.MapHub<EventsHub>("api/events");
app.UseSpaYarp();
app.MapFallbackToFile("index.html");

await app.RunAsync();