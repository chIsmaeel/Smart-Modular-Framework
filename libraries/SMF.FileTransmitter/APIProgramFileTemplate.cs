namespace SMF.FileTransmitter;
internal record APIProgramFileTemplate(ConfigSMF Config, List<string> Usings, List<string> Services, List<string> Middlewares)
{

    /// <summary>
    /// Creates the template.
    /// </summary>
    /// <returns>A string.</returns>
    public string CreateTemplate()
    {
        var u = string.Join("\n", Usings.Select(x => $"using {x};"));
        var s = string.Join("\n", Services.Select(x => $"builder.Services.{x};"));
        var m = string.Join("\n", Middlewares.Select(x => $"app.{x};"));
        return GetTemplate(u, s, m);
    }
    /// <summary>           
    /// Gets the template.
    /// </summary>
    /// <param name="usings">The usings.</param>
    /// <param name="services">The services.</param>
    /// <param name="middlewares">The middlewares.</param>
    /// <returns>A string.</returns>
    public static string GetTemplate(string usings, string services, string middlewares)
    {
        return
@$"
{usings}
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
{services}

var app = builder.Build();
{middlewares}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.Run(); ";
    }
}
