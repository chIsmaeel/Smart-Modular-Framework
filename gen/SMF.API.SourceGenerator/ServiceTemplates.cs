namespace SMF.API.SourceGenerator;

using Humanizer;
using System.Text;

/// <summary>
/// The service templates.
/// </summary>

internal class ServiceTemplates
{
    /// <summary>
    /// Gets the all.
    /// </summary>
    /// <param name="modelCT">The model c t.</param>
    /// <returns>A string.</returns>
    public static string GetAll(ModelCT modelCT)
    {
        return $$"""
        public async Task<IEnumerable<{{modelCT.IdentifierNameWithoutPostFix}}Dto>> GetAll{{modelCT!.IdentifierNameWithoutPostFix.Pluralize()}}()
    {
        var e = await _mediatR.Send(new GetAll{{modelCT.IdentifierNameWithoutPostFix.Pluralize()}}Query());
 List<{{modelCT.IdentifierNameWithoutPostFix}}Dto> list = new ();       
foreach (var response in e)
        {        
            var obj = new {{modelCT.IdentifierNameWithoutPostFix}}Dto();
            obj = response;
             {{AddReseveRelation(modelCT, "response")}}
list.Add(obj);
        }
            return list;
    }
""";
    }

    public static string GetById(ModelCT modelCT)
    {
        return $$"""
        public async Task<{{modelCT.IdentifierNameWithoutPostFix}}Dto> Get{{modelCT.IdentifierNameWithoutPostFix}}ById(int request)
    {
        var response = await _mediatR.Send(new Get{{modelCT.IdentifierNameWithoutPostFix}}ByIdQuery(request));
        if(response is null) return null;
        var obj = new {{modelCT.IdentifierNameWithoutPostFix}}Dto(); 
        obj = response;
         {{AddReseveRelation(modelCT, "request", true)}}
        return obj;
    }
""";
    }

    private static string AddReseveRelation(ModelCT modelCT, string obj, bool addJustId = false)
    {
        StringBuilder sb = new();
        //        foreach (var p in modelCT.Properties.Where(_ => _!.RelationshipWith is not null && _.RelationshipWith.RelationshipType == SMF.SourceGenerator.Core.Types.RelationshipType.O2O))
        //        {
        //            sb.AppendLine($$"""
        // obj.{{p.IdentifierName}} = await _mediatR.Send(new {{modelCT.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Application.{{(p.RelationshipWith!.WithRelationship.ClassType as ModelCT)!.ContainingModuleName}}.Queries.Get{{(p.RelationshipWith!.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}}ByIdQuery(response.{{p.IdentifierName}}_{{(p.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}}_FK));

        //""");                                               
        //        }
        obj = addJustId ? obj : (obj + ".Id");
        foreach (var p in modelCT.Properties.Where(_ => _!.HasRelation is not null && _.Type.StartsWith("System.Collections.Generic.List")))
        {
            sb.AppendLine($$"""
 obj.{{p.IdentifierName.Pluralize()}} = new();
  foreach (var o in await _mediatR.Send(new {{modelCT.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}}.Application.{{(p.HasRelation!.HasRelation.ClassType as ModelCT)!.ContainingModuleName}}.Queries.GetAll{{(p.HasRelation!.HasRelation.ClassType as ModelCT)!.IdentifierNameWithoutPostFix.Pluralize()}}Query(_ => _.{{p.HasRelation.HasRelation.IdentifierName}}_{{(p.HasRelation.HasRelation.ClassType as ModelCT)!.IdentifierNameWithoutPostFix.Pluralize()}}_FK == {{obj}})))
        {
            obj.{{p.IdentifierName.Pluralize()}}.Add(o);
        }                               
""");
        }
        return sb.ToString();
    }

    public static string Add(ModelCT modelCT)
    {
        return
$$"""
       public async Task<int> Add{{modelCT.IdentifierNameWithoutPostFix}}(Create{{modelCT.ModuleNameWithoutPostFix}}_{{modelCT.IdentifierNameWithoutPostFix}}Command request)
    {
        var createCommandObj = new Create{{modelCT.IdentifierNameWithoutPostFix}}Command();
        createCommandObj = request;
        var response = await _mediatR.Send(createCommandObj);
       return response;

}
""";
    }

    public static string Update(ModelCT modelCT)
    {
        return
$$"""
    public async Task<int> Update{{modelCT.IdentifierNameWithoutPostFix}}(Update{{modelCT.ModuleNameWithoutPostFix}}_{{modelCT.IdentifierNameWithoutPostFix}}Command request)
    {
        var updateCommandObj = new Update{{modelCT.IdentifierNameWithoutPostFix}}Command(); 
        updateCommandObj = request;
        var response = await _mediatR.Send(updateCommandObj);
        return response;

    }
""";
    }

    public static string Delete(ModelCT modelCT)
    {
        return
$$"""
    public async Task<int> Delete{{modelCT.IdentifierNameWithoutPostFix}}(int request)
    {
        var response = await _mediatR.Send(new Delete{{modelCT.IdentifierNameWithoutPostFix}}Command(request));

        return response;
    }
""";
    }
}


