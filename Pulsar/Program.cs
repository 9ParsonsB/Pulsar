using Lamar.Microsoft.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLamar();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddDbContext<PulsarContext>();
builder.Services.Configure<PulsarConfiguration>(builder.Configuration.GetSection(nameof(Pulsar)));

var app = builder.Build();

await app.RunAsync();

public class PulsarConfiguration
{
    public string JournalDirectory { get; set; }
}