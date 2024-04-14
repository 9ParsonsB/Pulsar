if (args.Length > 0 && File.Exists(args[0]))
{
    var fileInfo = new FileInfo(args[0]);
    if (fileInfo.Extension is ".eop" or ".zip")
        File.Copy(fileInfo.FullName, Path.Join(AppDomain.CurrentDomain.BaseDirectory, "plugins", fileInfo.Name));
}

try
{
    WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);
    
    var app = builder.Build();
    
    SettingsManager.Load();
    
    await app.RunAsync();
}
catch (Exception ex)
{
    LoggingUtils.LogError(ex, "");
}