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
        var m = string.Join("\n", Middlewares.Select(x => $"app.Use{x};"));
        return GetTemplate(u, s, m);
    }
    /// <summary>           
    /// Gets the template.
    /// </summary>
    /// <param name="usings">The usings.</param>
    /// <param name="services">The services.</param>
    /// <param name="middlewares">The middlewares.</param>
    /// <returns>A string.</returns>
    public string GetTemplate(string usings, string services, string middlewares)
    {
        return
@$"
{usings}
var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        {services}

        var app = builder.Build();
        {middlewares}
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        
{{
                app.UseSwagger();
                app.UseSwaggerUI();
}}
       
{MigrateIfVersionChange()}

app.MapGet("" / a"", async ([FromServices] IMediator context) =>
{{
            return await context.Send(new MyPointOfSale.Application.SaleAddon.Queries.GetAllSaleLinesQuery());
}});
        app.UseHttpsRedirection();
        app.Run(); ";
    }

    /// <summary>
    /// Migrates the if version change.
    /// </summary>
    /// <returns>A string? .</returns>
    public string? MigrateIfVersionChange()
    {
        var migrationDir = new DirectoryInfo(Path.Combine(Config.SOLUTION_BASE_PATH, Config.SOLUTION_NAME, "src", $"{Config.SOLUTION_NAME}Application", "Migrations"));
        if (!migrationDir.Exists) return null;
        bool foundMigrationFile = false;
        foreach (var migrationFile in migrationDir.EnumerateFiles())
            if (migrationFile.Name.EndsWith("SMF_" + Config.APP_VERSION))
            {
                foundMigrationFile = true;
                break;
            }

        if (foundMigrationFile) return null;
        return @$"
using (var scope = app.Services.CreateScope())
    (scope.ServiceProvider.GetRequiredService<{Config.SOLUTION_NAME}.Application.Interfaces.ISMFDbContext>() as DbContext).Database.Migrate();
";

    }
}
