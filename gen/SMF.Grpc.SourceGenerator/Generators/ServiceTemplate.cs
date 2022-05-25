﻿namespace Grpc.Services;

using Humanizer;
using System.Text;

internal record ServiceTemplate()
{
    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <returns>A string.</returns>
    public static string GetTemplate(ModuleWithRegisteredModelCTs s)
    {
        return
$$"""                                           
/*


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
        {{GetCRUDMethods(s)}}
  
}

*/
""";

    }

    /// <summary>
    /// Gets the c r u d methods.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>A string.</returns>
    public static string GetCRUDMethods(ModuleWithRegisteredModelCTs s)
    {
        var sb = new StringBuilder();
        foreach (var ModelCT in s.RegisteredModelCTs!)
        {
            sb.AppendLine(
$$"""
        public override async Task GetAll{{ModelCT!.IdentifierNameWithoutPostFix.Pluralize()}}(Void request, global::Grpc.Core.IServerStreamWriter<{{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}> responseStream, global::Grpc.Core.ServerCallContext context)
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
        if(response is null) return null;
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
           {{PropertiesMapping(ModelCT, "request")}}
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
""");
        }
        return sb.ToString();
    }

    /// <summary>
    /// Properties the mapping.
    /// </summary>
    /// <param name="objName">The obj name.</param>
    /// <param name="includeID">If true, include i d.</param>
    /// <returns>A string.</returns>
    public static string PropertiesMapping(ModelCT modelCT, string objName)
    {
        var sb = new StringBuilder();
        var tModelCT = modelCT;
        while (tModelCT is not null)
        {
            foreach (var p in tModelCT.Properties)
            {
                if (p!.IdentifierName is "CreatedOn" or "LastModifiedOn")
                    continue;
                if (p.IdentifierName.EndsWith("_FK"))
                    continue;
                if (p.Type is "SMFields.Binary" or "SMFields.Binary?")
                    sb.AppendLine($"{p!.IdentifierName} = Google.Protobuf.ByteString.CopyFrom({objName}.{p.IdentifierName}),");
                else if (p.RelationshipWith is not null)
                {
                    sb.AppendLine(
$$"""
    {{p!.IdentifierName}} = new {{(p.RelationshipWith.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix}}_{{(p.RelationshipWith.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}}()
            {
                CreatedOn = cd.ToTimestamp(),
                LastModifiedOn = md.ToTimestamp(),
                Id = e.Id,
               {{PropertiesMapping((p.RelationshipWith.WithRelationship.ClassType as ModelCT)!, $"e.{p!.IdentifierName}")}}
            },
""");
                }
                else
                    sb.AppendLine($"{p!.IdentifierName} = {objName}.{p.IdentifierName},");
            }

            tModelCT = tModelCT.ParentClassType as ModelCT;
        }

        return sb.ToString();
    }
}