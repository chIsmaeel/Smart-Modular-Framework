namespace Grpc.Services;

using Humanizer;
using System.Text;

internal record ServiceTemplate()
{
    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <returns>A string.</returns>
    public static string GetTemplate(ModelCT ModelCT)
    {
        return
$$"""
/*


  namespace {{ModelCT.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Grpc.Services;

using Google.Protobuf.WellKnownTypes;
using MediatR;
using {{ModelCT.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Application.{{ModelCT.ModuleNameWithoutPostFix}}Addon.Commands;
using {{ModelCT.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Application.{{ModelCT.ModuleNameWithoutPostFix}}Addon.Queries;
using {{ModelCT.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Grpc;
using System.Threading.Tasks;

internal class {{ModelCT.IdentifierNameWithoutPostFix}}Service : {{ModelCT.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Grpc.{{ModelCT.ModuleNameWithoutPostFix}}Services.{{ModelCT.ModuleNameWithoutPostFix}}ServicesBase
{
    private readonly IMediator _mediatR;

    public {{ModelCT.IdentifierNameWithoutPostFix}}Service(IMediator mediatR)
    {
        _mediatR = mediatR;
    }

    public override async Task GetAll{{ModelCT.IdentifierNameWithoutPostFix.Pluralize()}}(Void request, global::Grpc.Core.IServerStreamWriter<{{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}> responseStream, global::Grpc.Core.ServerCallContext context)
    {
        var response = await _mediatR.Send(new GetAll{{ModelCT.IdentifierNameWithoutPostFix.Pluralize()}}Query());
        foreach (var e in response)
        {
            var cd = DateTime.SpecifyKind(e.CreatedOn, DateTimeKind.Utc);
            var md = DateTime.SpecifyKind(e.LastModifiedOn, DateTimeKind.Utc);
            var obj = new {{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}()
            {
                CreatedOn = cd.ToTimestamp(),
                LastModifiedOn = md.ToTimestamp(),
                Id = e.Id,
               {{PropertiesMapping(ModelCT, "e")}}

            };
            await responseStream.WriteAsync(obj);
        }
    }

    public override async Task<{{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}> Get{{ModelCT.IdentifierNameWithoutPostFix}}ById(RequestId request, global::Grpc.Core.ServerCallContext context)
    {
        var response = await _mediatR.Send(new Get{{ModelCT.IdentifierNameWithoutPostFix}}ByIdQuery(request.Id));

        var cd = DateTime.SpecifyKind(response.CreatedOn, DateTimeKind.Utc);
        var md = DateTime.SpecifyKind(response.LastModifiedOn, DateTimeKind.Utc);
       
        return new {{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}
        {
            CreatedOn = cd.ToTimestamp(),
            LastModifiedOn = md.ToTimestamp(),
            Id = response.Id,
           {{PropertiesMapping(ModelCT, "response")}}
        };
    }

    public override async Task<ResponseId> Add{{ModelCT.IdentifierNameWithoutPostFix}}(Create{{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}Command request, global::Grpc.Core.ServerCallContext context)
    {
        var response = await _mediatR.Send(new Create{{ModelCT.IdentifierNameWithoutPostFix}}Command()
        {
           {{PropertiesMapping(ModelCT, "request", false)}}
        });
        return new ResponseId
        {
            Id = response,
        };

    }

    public override async Task<ResponseId> Update{{ModelCT.IdentifierNameWithoutPostFix}}(Update{{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}Command request, global::Grpc.Core.ServerCallContext context)
    {
        var response = await _mediatR.Send(new Update{{ModelCT.IdentifierNameWithoutPostFix}}Command()
        {
           Id = request.Id,
           {{PropertiesMapping(ModelCT, "request")}}
        });
        return new ResponseId
        {
            Id = response,
        };

    }

    public override async Task<ResponseId> Delete{{ModelCT.IdentifierNameWithoutPostFix}}(RequestId request, global::Grpc.Core.ServerCallContext context)
    {
        var response = await _mediatR.Send(new Delete{{ModelCT.IdentifierNameWithoutPostFix}}Command(request.Id));

        return new ResponseId
        {
            Id = response,
        };

    }
}

*/
""";

    }

    /// <summary>
    /// Properties the mapping.
    /// </summary>
    /// <param name="objName">The obj name.</param>
    /// <param name="includeID">If true, include i d.</param>
    /// <returns>A string.</returns>
    public static string PropertiesMapping(ModelCT modelCT, string objName, bool includeID = true)
    {
        var sb = new StringBuilder();
        var tModelCT = modelCT;
        while (tModelCT is not null)
        {
            foreach (var p in tModelCT.Properties)
            {
                if (p!.IdentifierName is "CreatedOn" or "LastModifiedOn")
                    continue;
                sb.AppendLine($"{p!.IdentifierName} = {objName}.{p.IdentifierName},");
            }

            tModelCT = tModelCT.ParentClassType as ModelCT;
        }

        return sb.ToString();
    }
}
