namespace Grpc.Services;

using Microsoft.CodeAnalysis;

[Generator]
internal class ProtoServicesGenerator : CommonIncrementalGenerator
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
