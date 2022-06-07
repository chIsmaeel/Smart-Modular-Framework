namespace SMF.Grpc.SourceGenerator;

using System.Text;

internal class ServiceTemplate
{
    public static string GetTemplate(ModuleWithRegisteredModelCTs s)
    {
        return
$$"""                                           
namespace {{s.RegisteringModule.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Grpc.Services;

using Google.Protobuf.WellKnownTypes;
using MediatR;
using {{s.RegisteringModule.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Application.{{s.RegisteringModule.IdentifierNameWithoutPostFix}}Addon.Commands;
using {{s.RegisteringModule.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Application.{{s.RegisteringModule.IdentifierNameWithoutPostFix}}Addon.Queries;
using {{s.RegisteringModule.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Grpc;
using System.Threading.Tasks;

internal class {{s.RegisteringModule.IdentifierNameWithoutPostFix}}Service : {{s.RegisteringModule.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Grpc.{{s.RegisteringModule.IdentifierNameWithoutPostFix}}Services.{{s.RegisteringModule.IdentifierNameWithoutPostFix}}ServicesBase
{
    private readonly IMediator _mediatR;

    public {{s.RegisteringModule.IdentifierNameWithoutPostFix}}Service(IMediator mediatR)
    {
        _mediatR = mediatR;
    }
        {{GetAll(s)}}
        {{GetById(s)}}
        {{Create(s)}}
        {{Update(s)}}
        {{Delete(s)}}
}
""";

    }


    private static object GetAll(ModuleWithRegisteredModelCTs s)
    {
        var sb = new StringBuilder();
        foreach (var modelCT in s.RegisteredModelCTs!)
            sb.AppendLine(ServiceTemplates.GetAll(modelCT!));
        return sb.ToString();
    }

    private static string GetById(ModuleWithRegisteredModelCTs s)
    {
        var sb = new StringBuilder();
        foreach (var modelCT in s.RegisteredModelCTs!)
            sb.AppendLine(ServiceTemplates.GetById(modelCT!));
        return sb.ToString();
    }

    private static string Create(ModuleWithRegisteredModelCTs s)
    {
        var sb = new StringBuilder();
        foreach (var modelCT in s.RegisteredModelCTs!)
            sb.AppendLine(ServiceTemplates.Add(modelCT!));
        return sb.ToString();
    }


    private static string Update(ModuleWithRegisteredModelCTs s)
    {
        var sb = new StringBuilder();
        foreach (var modelCT in s.RegisteredModelCTs!)
            sb.AppendLine(ServiceTemplates.Update(modelCT!));
        return sb.ToString();
    }


    private static string Delete(ModuleWithRegisteredModelCTs s)
    {
        var sb = new StringBuilder();
        foreach (var modelCT in s!.RegisteredModelCTs!)
            sb.AppendLine(ServiceTemplates.Delete(modelCT!));
        return sb.ToString();
    }
}