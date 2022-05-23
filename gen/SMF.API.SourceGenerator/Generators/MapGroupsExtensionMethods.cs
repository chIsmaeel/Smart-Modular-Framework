namespace API.Endpoints;

using Humanizer;
using SMF.SourceGenerator.Abstractions;

/// <summary>
/// The model generator.
/// </summary>
[Generator]
internal class MapGroupsExtensionMethods : CommonIncrementalGenerator
{    /// <summary>
     /// Executes the.
     /// </summary>
     /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(RegisteredModelCTs, AddMigrationExtensionMethods);
    }

    /// <summary>
    /// Adds the infrastructure extension methods.
    /// </summary>               
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    ///  

    private void AddMigrationExtensionMethods(SourceProductionContext c, ModelCT s)
    {
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //            System.Diagnostics.Debugger.Launch();
        //#endif
        var config = s!.ConfigSMFAndGlobalOptions.ConfigSMF!;
        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate fileScopedNamespace = new(config.SOLUTION_NAME! + ".API.EndPoints");
        ClassTypeTemplate classTypeTemplate = new($"Map{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Group")
        {
            Modifiers = "public static",
            UsingNamespaces = new() { "Microsoft.AspNetCore.Builder", "Microsoft.AspNetCore.Routing", "Microsoft.AspNetCore.Http.HttpResults", "Microsoft.AspNetCore.Http", "Microsoft.AspNetCore.Mvc", "MediatR" }
        };

        classTypeTemplate.Members.Add(new TypeMethodTemplate("Microsoft.AspNetCore.Routing.GroupRouteBuilder", $"Map{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Api")
        {
            Modifiers = "public static",
            Parameters = new() { ("this Microsoft.AspNetCore.Routing.GroupRouteBuilder", "g") },
            Body = (writer, parameters, _, fileds) =>
            {

                writer.WriteLine(@$"g.MapGet(""/"", GetAll{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix.Pluralize()});");
                writer.WriteLine(@$"g.MapGet(""/{{id}}"", Get{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}ById);");
                writer.WriteLine(@$"g.MapPost(""/"", Add{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix});");
                writer.WriteLine(@$"g.MapPut(""/{{id}}"", Update{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix});");
                writer.WriteLine(@$"g.MapDelete(""/{{id}}"", Delete{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix});");
                writer.WriteLine("return g;");
            }
        });


        classTypeTemplate.StringMembers.Add(GetAll(s));
        classTypeTemplate.StringMembers.Add(GetById(s));
        classTypeTemplate.StringMembers.Add(Add(s));
        classTypeTemplate.StringMembers.Add(Update(s));
        classTypeTemplate.StringMembers.Add(Delete(s));


        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource(fileScopedNamespace);
    }

    /// <summary>
    /// Deletes the.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>A string.</returns>
    private string Delete(ModelCT s)
    {
        return @$"
 public static async Task<Ok<int>> Delete{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}(int id, [FromServices] IMediator m)
    {{ 
       var response = await m.Send(new {QualifiedNames.GetDeleteCommand(s)}(id));
       return TypedResults.Ok(response);
    }}";
    }

    /// <summary>
    /// Updates the.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>A string.</returns>
    private string Update(ModelCT s)
    {
        return @$"
 public static async Task<Results<NoContent, NotFound>> Update{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}(int id, {QualifiedNames.GetUpdateCommand(s)} command, [FromServices] IMediator m)
    {{ 
         command.Id = id;
         var response = await m.Send(command);
         return response == id ? TypedResults.NoContent() : TypedResults.NotFound();
    }}";
    }

    /// <summary>
    /// Adds the.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>A string.</returns>
    private string Add(ModelCT s)
    {
        return @$"
 public static async Task<Created<int>> Add{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}({QualifiedNames.GetCreateCommand(s)} command, [FromServices] IMediator m)
    {{ 
       var response = await m.Send(command);
        return TypedResults.Created($""/{{response}}"", response);
    }}";
    }

    /// <summary>
    /// Gets the by id.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>A string.</returns>
    private string GetById(ModelCT s)
    {
        return @$"
 public static async Task<Results<Ok<{s.NewQualifiedName}>, NotFound>> Get{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}ById(int id, [FromServices] IMediator m)
    {{
        return await m.Send(new {QualifiedNames.GetByIdQuery(s)}(id)) is {s.NewQualifiedName} e
            ? TypedResults.Ok(e)
            : TypedResults.NotFound();
    }}
";
    }

    /// <summary>
    /// Gets the all.
    /// </summary>
    /// <returns>A string.</returns>
    private string GetAll(ModelCT s)
    {
        return $@" 
public static async Task<Ok<IEnumerable<{s.NewQualifiedName}>>> GetAll{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix.Pluralize()}([FromServices] IMediator m)
{{
  var response = await m.Send(new {QualifiedNames.GetAllQuery(s)}());
   return TypedResults.Ok(response);
}}";
    }
}
