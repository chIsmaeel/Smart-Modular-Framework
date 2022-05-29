namespace Grpc.Services;

using Microsoft.CodeAnalysis;
using SMF.Grpc.SourceGenerator;

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
        context.AddSourceInComment(s.RegisteringModule.IdentifierNameWithoutPostFix + "Service", ServiceTemplate.GetTemplate(s));
    }
}
