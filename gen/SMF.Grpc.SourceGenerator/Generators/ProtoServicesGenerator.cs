namespace Grpc.Services;

using Microsoft.CodeAnalysis;

[Generator]
internal class ProtoServicesGenerator : CommonIncrementalGenerator
{
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(RegisteredModelCTs, AddGrpcServices);
    }

    private void AddGrpcServices(SourceProductionContext c, ModelCT s)
    {
        SMFProductionContext context = new(c);
        context.AddSource(s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix + "Service", ServiceTemplate.GetTemplate(s));
    }
}
