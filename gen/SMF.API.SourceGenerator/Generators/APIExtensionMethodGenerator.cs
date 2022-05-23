namespace API;

using SMF.SourceGenerator.Abstractions;
using SMF.SourceGenerator.Core;

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
        context.RegisterSourceOutput(ConfigSMF, AddInfrastructureExtensionMethods);
    }

    /// <summary>
    /// Adds the infrastructure extension methods.
    /// </summary>               
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddInfrastructureExtensionMethods(SourceProductionContext c, ConfigSMF? s)
    {
        var configSMF = s;

        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate fileScopedNamespace = new(configSMF!.SOLUTION_NAME! + ".Infrastructure");
        ClassTypeTemplate classTypeTemplate = new("ExtensionMethods")
        {
            Modifiers = "public static",
            UsingNamespaces = new() { "Microsoft.EntityFrameworkCore", "Microsoft.Extensions.DependencyInjection" }
        };

        classTypeTemplate.Members.Add(new TypeMethodTemplate("void", "AddSMFInfrastructureServices")
        {
            Modifiers = "public static",
            Parameters = new() { ("this Microsoft.Extensions.DependencyInjection.IServiceCollection", "services") },
            Body = (writer, parameters, _, fileds) =>
            {
                writer.WriteLine($@"services.AddDbContext<Application.Interfaces.ISMFDbContext, Data.SMFDbContext>(o => o.UseSqlServer(@""Data Source = {configSMF.DB_DATA_SOURCE}; Initial Catalog = {configSMF.DB_NAME}; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False""));");
                writer.WriteLine();
                foreach (var moduleCT in s)
                    writer.WriteLine($@" services.AddScoped<{QualifiedNames.GetRegisterModelRepositoryInterface(configSMF, moduleCT)},{QualifiedNames.GetRegisterModelRepository(configSMF, moduleCT)}> ();");
                writer.WriteLine();
                writer.WriteLine($@"services.AddScoped<{QualifiedNames.GetIUnitOfWork(configSMF)}, {QualifiedNames.GetUnitOfWork(configSMF)}>();");
            }
        });


        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource(fileScopedNamespace);
    }

}
