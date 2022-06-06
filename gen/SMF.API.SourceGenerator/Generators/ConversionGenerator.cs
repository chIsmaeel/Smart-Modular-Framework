namespace API.Conversions;

using Microsoft.CodeAnalysis;
using SMF.Grpc.SourceGenerator;
using SMF.SourceGenerator.Core;

[Generator]
internal class ConversionGenerator : CommonIncrementalGenerator
{
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(RegisteredModelCTs, AddConversionClasses);
    }

    private void AddConversionClasses(SourceProductionContext c, ModelCT s)
    {
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //            System.Diagnostics.Debugger.Launch();
        //#endif
        SMFProductionContext context = new(c);
        context.AddSource(s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix, ConversionTemplate.CreateTemplate(s));

    }
}

internal class ConversionTemplate
{
    public static string CreateTemplate(ModelCT s)
    {
        var config = s.ConfigSMFAndGlobalOptions.ConfigSMF;
        return
$$"""
 namespace {{s.NewContainingNamespace}};

using {{config!.SOLUTION_NAME}}.Application.{{s.ContainingModuleName}}.Commands;
using {{config!.SOLUTION_NAME}}.Domain.{{s.ContainingModuleName}}.Entities;



public partial class {{s.IdentifierNameWithoutPostFix}}Dto
{
    public static implicit operator {{s.IdentifierNameWithoutPostFix}}({{s.IdentifierNameWithoutPostFix}}Dto {{s.ModuleNameWithoutPostFix.FirstCharToLowerCase()}}_{{s.IdentifierNameWithoutPostFix}})
    {
        if ({{s.ModuleNameWithoutPostFix.FirstCharToLowerCase()}}_{{s.IdentifierNameWithoutPostFix}} is null)
        {
            return null;
        }
         var resultObj = new {{s.IdentifierNameWithoutPostFix}}();
         resultObj.Id = {{s.ModuleNameWithoutPostFix.FirstCharToLowerCase()}}_{{s.IdentifierNameWithoutPostFix}}.Id;
         resultObj.CreatedOn = {{s.ModuleNameWithoutPostFix.FirstCharToLowerCase()}}_{{s.IdentifierNameWithoutPostFix}}.CreatedOn ;
         resultObj.LastModifiedOn = {{s.ModuleNameWithoutPostFix.FirstCharToLowerCase()}}_{{s.IdentifierNameWithoutPostFix}}.LastModifiedOn;

{{StaticMethods.AddEntityImplicitOperatorProperties(s, $"{s.ModuleNameWithoutPostFix.FirstCharToLowerCase()}_{s.IdentifierNameWithoutPostFix}")}}          
         return resultObj;
    }

    public static implicit operator {{s.IdentifierNameWithoutPostFix}}Dto({{s.IdentifierNameWithoutPostFix}} {{s.IdentifierNameWithoutPostFix.FirstCharToLowerCase()}})
    {
         if ({{s.IdentifierNameWithoutPostFix.FirstCharToLowerCase()!}} is null)
        {
            return null;
        }
        var resultObj = new {{s.IdentifierNameWithoutPostFix}}Dto(); 
        resultObj.Id = {{s.IdentifierNameWithoutPostFix.FirstCharToLowerCase()!}}.Id;
         resultObj.CreatedOn = {{s.IdentifierNameWithoutPostFix.FirstCharToLowerCase()!}}.CreatedOn is DateTime ?(DateTime) {{s.IdentifierNameWithoutPostFix.FirstCharToLowerCase()!}}.CreatedOn : default(DateTime);
         resultObj.LastModifiedOn = {{s.IdentifierNameWithoutPostFix.FirstCharToLowerCase()!}}.LastModifiedOn is DateTime ?(DateTime) {{s.IdentifierNameWithoutPostFix.FirstCharToLowerCase()!}}.LastModifiedOn : default(DateTime);

{{StaticMethods.AddOperatorProperties(s, s.IdentifierNameWithoutPostFix.FirstCharToLowerCase()!)}}
        return resultObj;
    }
}
public partial class Create{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command
{
    public static implicit operator Create{{s.IdentifierNameWithoutPostFix}}Command(Create{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command create{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command)
    {
         if (create{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command is null)
        {
            return null;
        }
         var resultObj = new Create{{s.IdentifierNameWithoutPostFix}}Command();
{{StaticMethods.ModifiedEntityImplicitOperatorProperties(s, $"create{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Command", false, addCreatedOn: true)}}          
         return resultObj;
    }

    public static implicit operator Create{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command(Create{{s.IdentifierNameWithoutPostFix}}Command create{{s.IdentifierNameWithoutPostFix}}Command)
    {
        if (create{{s.IdentifierNameWithoutPostFix}}Command is null)
        {
            return null;
        }
        var resultObj = new Create{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command(); 
{{StaticMethods.ModifiedOperatorProperties(s, $"create{s.IdentifierNameWithoutPostFix}Command", false, addCreatedOn: true)}}
        return resultObj;
    }
}
public partial class Update{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command
{
    public static implicit operator Update{{s.IdentifierNameWithoutPostFix}}Command(Update{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command update{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command)
    {
         if (update{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command is null)
        {
            return null;
        }
         var resultObj = new Update{{s.IdentifierNameWithoutPostFix}}Command();
         resultObj.Id = update{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command.Id;
        
{{StaticMethods.ModifiedEntityImplicitOperatorProperties(s, $"update{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Command", false, addLastModifiedOn: true)}}          
         return resultObj;
    }

    public static implicit operator Update{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command(Update{{s.IdentifierNameWithoutPostFix}}Command update{{s.IdentifierNameWithoutPostFix}}Command)
    {
        if (update{{s.IdentifierNameWithoutPostFix}}Command is null)
        {
            return null;
        }
        var resultObj = new Update{{s.ModuleNameWithoutPostFix}}_{{s.IdentifierNameWithoutPostFix}}Command(); 
         resultObj.Id = update{{s.IdentifierNameWithoutPostFix}}Command.Id;     
{{StaticMethods.ModifiedOperatorProperties(s, $"update{s.IdentifierNameWithoutPostFix}Command", false, addLastModifiedOn: true)}}
        return resultObj;
    } 
}
""";
    }
}
