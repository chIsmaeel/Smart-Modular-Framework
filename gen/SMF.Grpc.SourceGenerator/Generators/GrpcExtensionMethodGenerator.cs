namespace Grpc;

using System.Collections.Immutable;

/// <summary>
/// The model generator.
/// </summary>
[Generator]
internal class APIExtensionMethods : CommonIncrementalGenerator
{    /// <summary>
     /// Executes the.
     /// </summary>
     /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(RegisteredModelCTs.Collect(), AddMigrationExtensionMethods);
    }

    /// <summary>
    /// Adds the infrastructure extension methods.
    /// </summary>               
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddMigrationExtensionMethods(SourceProductionContext c, ImmutableArray<ModelCT> models)
    {
        var s = models.FirstOrDefault()?.ConfigSMFAndGlobalOptions.ConfigSMF;
        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate fileScopedNamespace = new(s!.SOLUTION_NAME! + ".Grpc");
        ClassTypeTemplate classTypeTemplate = new("ExtensionMethods")
        {
            Modifiers = "public static",
            UsingNamespaces = new() {
                "Microsoft.AspNetCore.Builder",
                $"{s.SOLUTION_NAME}.Grpc.Services",
            "Microsoft.Extensions.DependencyInjection",
            "Microsoft.EntityFrameworkCore"
            }
        };


        classTypeTemplate.Members.Add(new TypeMethodTemplate("void", "AddSMFMigrations")
        {

            Modifiers = "public static",
            Parameters = new() { ("this WebApplication", "app") },
            Body = (writer, parameters, _, fileds) =>
            {

                writer.WriteLine(@$"
using (var scope = app.Services.CreateScope())
    (scope.ServiceProvider.GetRequiredService<{s.SOLUTION_NAME}.Application.Interfaces.ISMFDbContext>() as DbContext).Database.Migrate();
");
            }
        });

        classTypeTemplate.Members.Add(new TypeMethodTemplate("void", "MapSMFGrpcServices")
        {
            Modifiers = "public static",
            Parameters = new() { ("this WebApplication", "app") },
            Body = (writer, parameters, _, fileds) =>
            {
                foreach (var m in models.GroupBy(_ => _.ModuleNameWithoutPostFix))
                {
                    writer.WriteLine($@"app.MapGrpcService<{m.Key}Service>();");
                }
            }
        });
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource("ExtensionMethods", "/*\n\n" + fileScopedNamespace.CreateTemplate().GetTemplate() + "\n\n*/");
    }

}
