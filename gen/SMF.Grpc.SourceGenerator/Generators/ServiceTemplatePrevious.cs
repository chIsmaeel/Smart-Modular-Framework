namespace Grpc.Services;


using Humanizer;
using SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;
using SMF.SourceGenerator.Core;
using SMF.SourceGenerator.Core.Types.TypeMembers;
using System.Text;

internal record ServiceTemplatePrevious()
{
    private static readonly string? _defaultObjName;
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
        var e = await _mediatR.Send(new GetAll{{ModelCT.IdentifierNameWithoutPostFix.Pluralize()}}Query());
        foreach (var response in e)
        {
{{CreateRelationObjs(ModelCT, "response")}}           
var obj = new {{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}()
            {
                 CreatedOn = DateTime.SpecifyKind(response.CreatedOn is System.DateTime ?(DateTime) response.CreatedOn : System.DateTime.Now, DateTimeKind.Utc).ToTimestamp(),
               LastModifiedOn = DateTime.SpecifyKind(response.LastModifiedOn is System.DateTime ?(DateTime) response.LastModifiedOn : System.DateTime.Now, DateTimeKind.Utc).ToTimestamp(),
            Id = response.Id,
               {{PropertiesMappingForGetAllAndGetById(ModelCT, "response")}}

            };
            await responseStream.WriteAsync(obj);
        }
    }

    public override async Task<{{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}> Get{{ModelCT.IdentifierNameWithoutPostFix}}ById(RequestId request, global::Grpc.Core.ServerCallContext context)
    {
        var response = await _mediatR.Send(new Get{{ModelCT.IdentifierNameWithoutPostFix}}ByIdQuery(request.Id));
        if(response is null) return null;
          {{CreateRelationObjs(ModelCT, "response")}}        
return new {{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}
        {
             CreatedOn = DateTime.SpecifyKind(response.CreatedOn is System.DateTime ?(DateTime) response.CreatedOn : System.DateTime.Now, DateTimeKind.Utc).ToTimestamp(),
               LastModifiedOn = DateTime.SpecifyKind(response.LastModifiedOn is System.DateTime ?(DateTime) response.LastModifiedOn : System.DateTime.Now, DateTimeKind.Utc).ToTimestamp(),
               Id = response.Id,
           {{PropertiesMappingForGetAllAndGetById(ModelCT, "response")}}
        };
    }

    public override async Task<ResponseId> Add{{ModelCT.IdentifierNameWithoutPostFix}}(Create{{ModelCT.ModuleNameWithoutPostFix}}_{{ModelCT.IdentifierNameWithoutPostFix}}Command request, global::Grpc.Core.ServerCallContext context)
    {
        var response = await _mediatR.Send(new Create{{ModelCT.IdentifierNameWithoutPostFix}}Command()
        {
           {{PropertiesMappingForAddAndUpdate(ModelCT, "request")}}
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
           {{PropertiesMappingForAddAndUpdate(ModelCT, "request")}}
        });
if(response is default(int)) return new ResponseId() { Id = 0 };
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

    private static string CreateRelationObjs(ModelCT modelCT, string objName, TypeProperty? relationalProperty = null, string? obj = null)
    {
        var sb = new StringBuilder();
        var tModelCT = modelCT;
        while (tModelCT is not null)
        {
            foreach (var p in tModelCT.Properties)
            {
                //if (obj is null)
                //    _defaultObjName = p.IdentifierName.FirstCharToLowerCase();
                //obj = obj is not null ? obj : _defaultObjName;
                if (p!.IdentifierName.EndsWith("_FK")) continue;
                if (p.Type is "SMFields.Binary" or "SMFields.Binary?")
                {
                    continue;
                }
                if (p.RelationshipWith is not null)
                {

                    if (relationalProperty == p)
                        continue;
                    string? variableName = p.IdentifierName.FirstCharToLowerCase();

                    sb.AppendLine(
$$"""
var  {{variableName}} = new {{(p.RelationshipWith.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix}}_{{(p.RelationshipWith.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}}();
if({{objName}}.{{p.IdentifierName}} is not null)
{
{{variableName}}.Id =  {{objName}}.{{p.IdentifierName}}.Id;
{{variableName}}.CreatedOn = DateTime.SpecifyKind({{objName}}.{{p.IdentifierName}}.CreatedOn is System.DateTime ? (DateTime){{objName}}.{{p.IdentifierName}}.CreatedOn : System.DateTime.Now, DateTimeKind.Utc).ToTimestamp();
{{variableName}}.LastModifiedOn =  DateTime.SpecifyKind({{objName}}.{{p.IdentifierName}}.LastModifiedOn is System.DateTime ? (DateTime){{objName}}.{{p.IdentifierName}}.LastModifiedOn : System.DateTime.Now, DateTimeKind.Utc).ToTimestamp();
{{CreateRelationObjs((p.RelationshipWith.WithRelationship.ClassType as ModelCT)!, $"{objName}.{p!.IdentifierName}", p.RelationshipWith.HasRelations, variableName)}}
{{AddIfObjNotEmpty(obj, p)}}
}
""");

                }
                else if (p.HasRelation is not null)
                {
                    if (relationalProperty == p)
                        continue;
                    string? variableName = p.IdentifierName.FirstCharToLowerCase();

                    sb.AppendLine(
$$"""                                          
var {{variableName}} = new {{(p.HasRelation.HasRelation.ClassType as ModelCT)!.ModuleNameWithoutPostFix}}_{{(p.HasRelation.HasRelation.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}}();
if({{objName}}.{{p.IdentifierName}} is not null)
{
{{variableName}}.Id =  {{objName}}.{{p.IdentifierName}}.Id;
{{variableName}}.CreatedOn = DateTime.SpecifyKind({{objName}}.{{p.IdentifierName}}.CreatedOn is System.DateTime ? (DateTime){{objName}}.{{p.IdentifierName}}.CreatedOn : System.DateTime.Now, DateTimeKind.Utc).ToTimestamp();
{{variableName}}.LastModifiedOn =  DateTime.SpecifyKind({{objName}}.{{p.IdentifierName}}.LastModifiedOn is System.DateTime ? (DateTime){{objName}}.{{p.IdentifierName}}.LastModifiedOn : System.DateTime.Now, DateTimeKind.Utc).ToTimestamp();
{{CreateRelationObjs((p.HasRelation.HasRelation.ClassType as ModelCT)!, $"{objName}.{p!.IdentifierName}", p.HasRelation.HasRelation, variableName)}}                                              
{{AddIfObjNotEmpty(obj, p)}}
}
""");
                }

                else
                    if (obj is not null)

                    sb.AppendLine($"{obj}.{p.IdentifierName} = {objName}.{p.IdentifierName} == null ? default : {objName}.{p.IdentifierName};");
            }
            tModelCT = tModelCT.ParentClassType as ModelCT;
        }

        return sb.ToString();
    }

    public static string? AddIfObjNotEmpty(string obj, TypeProperty p)
    {
        if (obj is not null)
            return $"{obj}.{p.IdentifierName.Replace("_", "")}  =  {p.IdentifierName.FirstCharToLowerCase()}; ";
        return null;
    }

    /// <summary>
    /// Have the f k_ compute_ created_ modified.
    /// </summary>
    /// <param name="p">The p.</param>
    /// <returns>A bool.</returns>
    public static bool HasFK_Compute_Created_Modified(TypeProperty p, string objName, StringBuilder sb)
    {
        if (p!.IdentifierName.EndsWith("_FK")) return true;
        if (p!.IdentifierName is "CreatedOn" or "LastModifiedOn")
            return true;
        return false;
    }

    public static string PropertiesMappingForGetAllAndGetById(ModelCT modelCT, string objName, TypeProperty? relationalProperty = null)
    {

        var sb = new StringBuilder();
        var tModelCT = modelCT;
        while (tModelCT is not null)
        {
            foreach (var p in tModelCT.Properties)
            {
                if (p.Type is "SMFields.Binary" or "SMFields.Binary?")
                {
                    sb.AppendLine($"{p!.IdentifierName.Replace("_", "")} ={objName}.{p.IdentifierName} == null?default: Google.Protobuf.ByteString.CopyFrom({objName}.{p.IdentifierName}),");
                    continue;
                }

                if (HasFK_Compute_Created_Modified(p, objName, sb))
                    continue;

                if (p.RelationshipWith is not null)
                {
                    if (relationalProperty == p)
                        continue;
                    sb.AppendLine(
$$"""
{{p.IdentifierName}} ={{p.IdentifierName.FirstCharToLowerCase()}} ==null?default:{{p.IdentifierName.FirstCharToLowerCase()}},
""");
                }
                else if (p.HasRelation is not null)
                {
                    if (relationalProperty == p)
                        continue;

                    sb.AppendLine(
$$"""
{{p.IdentifierName.Replace("_", "")}} = {{p.IdentifierName.FirstCharToLowerCase()}}==null?default:{{p.IdentifierName.FirstCharToLowerCase()}},
""");
                }

                else
                    sb.AppendLine($"{p.IdentifierName.Replace("_", "")} = {objName}.{p.IdentifierName} == null ? default : {objName}.{p.IdentifierName},");
            }

            tModelCT = tModelCT.ParentClassType as ModelCT;
        }



        return sb.ToString();
    }


    public static string AddObjectReferences(ModelCT modelCT, string objName, TypeProperty? relationalProperty = null, string? obj = null)
    {
        var sb = new StringBuilder();
        var tModelCT = modelCT;
        while (tModelCT is not null)
        {
            foreach (var p in tModelCT.Properties)
            {
                if (obj is null)
                    sb.Append("// ");

                if (p!.IdentifierName.EndsWith("_FK")) continue;
                if (p.Type is "SMFields.Binary" or "SMFields.Binary?")
                {
                    continue;
                }
                if (p.RelationshipWith is not null)
                {

                    if (relationalProperty == p)
                        continue;
                    string? variableName = p.IdentifierName.FirstCharToLowerCase();

                    sb.AppendLine(
$$"""
{{obj}}.{{p.IdentifierName.Replace("_", "")}} = {{p.IdentifierName.FirstCharToLowerCase()}};
{{AddObjectReferences((p.RelationshipWith.WithRelationship.ClassType as ModelCT)!, $"{objName}.{p!.IdentifierName}", p.RelationshipWith.HasRelations, variableName)}}
""");

                }
                else if (p.HasRelation is not null)
                {
                    if (relationalProperty == p)
                        continue;
                    string? variableName = p.IdentifierName.FirstCharToLowerCase();
                    if (obj is not null)
                        sb.AppendLine(
$$"""
{{obj}}.{{p.IdentifierName.Replace("_", "")}} = {{p.IdentifierName.FirstCharToLowerCase()}};
{{AddObjectReferences((p.HasRelation.HasRelation.ClassType as ModelCT)!, $"{objName}.{p!.IdentifierName}", p.HasRelation.HasRelation, variableName)}}                                               
""");
                }

            }
            tModelCT = tModelCT.ParentClassType as ModelCT;
        }

        return sb.ToString();
    }

    /// <summary>
    /// Properties the mapping.
    /// </summary>
    /// <param name="objName">The obj name.</param>
    /// <param name="includeID">If true, include i d.</param>
    /// <returns>A string.</returns>
    public static string PropertiesMappingForAddAndUpdate(ModelCT modelCT, string objName, TypeProperty? relationalProperty = null)
    {

        var sb = new StringBuilder();
        var tModelCT = modelCT;
        while (tModelCT is not null)
        {
            foreach (var p in tModelCT.Properties)
            {
                if (HasFK_Compute_Created_Modified(p, objName, sb))
                    continue;
                if (p.SMField is not null && p.SMField.Field is not null)
                {
                    if (p.SMField.Field.Compute)
                        continue;
                }
                if (p.RelationshipWith is not null)
                {
                    if (relationalProperty == p)
                        continue;
                    sb.AppendLine(
$$"""
                    {{p.IdentifierName}} ={{objName}}.{{p.IdentifierName.Replace("_", "")}}== null? null: new()
                            {
                                {{PropertiesMappingForAddAndUpdate((p.RelationshipWith.WithRelationship.ClassType as ModelCT)!, $"{objName}.{p!.IdentifierName.Replace("_", "")}", p.RelationshipWith.HasRelations)}}
                            },
""");
                }
                else if (p.HasRelation is not null)
                {
                    if (relationalProperty == p)
                        continue;
                    sb.AppendLine(
$$"""
{{p.IdentifierName}} = {{objName}}.{{p.IdentifierName.Replace("_", "")}} == null?null: new(){    
{{PropertiesMappingForAddAndUpdate((p.HasRelation.HasRelation.ClassType as ModelCT)!, $"{objName}.{p!.IdentifierName.Replace("_", "")}", p.HasRelation.HasRelation)}}
},
""");
                }

                else
                {
                    var type = ModelPropertyTypes.GetPropertyType(p!);
                    if (type.EndsWith("?"))
                        type = type.Remove(type.Length - 1);

                    sb.AppendLine($"{p.IdentifierName} = {objName}.{p.IdentifierName} is default({type}) ? default : {objName}.{p.IdentifierName},");
                }
            }

            tModelCT = tModelCT.ParentClassType as ModelCT;
        }

        return sb.ToString();
    }
}
