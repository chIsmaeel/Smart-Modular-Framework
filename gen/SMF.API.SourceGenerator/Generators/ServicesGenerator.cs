namespace API.Services;

using Microsoft.CodeAnalysis;
using SMF.API.SourceGenerator;

[Generator]
internal class ServicesGenerator : CommonIncrementalGenerator
{
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(ModuleWithRegisteredModelCTs, AddGrpcServices);
    }

    private void AddGrpcServices(SourceProductionContext c, ModuleWithRegisteredModelCTs s)
    {
        SMFProductionContext context = new(c);
        context.AddSource(s.RegisteringModule.IdentifierNameWithoutPostFix + "Service", ServiceTemplate.GetTemplate(s));
    }
}
