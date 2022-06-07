namespace API.DTOClasses;

using Microsoft.CodeAnalysis;
using SMF.Grpc.SourceGenerator;
using System.Text;
/// <summary>
/// The proto file generator.
/// </summary>                      

[Generator]
internal class DTOClassGenerator : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(RegisteredModelCTs, AddProtoFile);
    }

    /// <summary>
    /// Adds the proto file.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddProtoFile(SourceProductionContext c, ModelCT s)
    {

        SMFProductionContext context = new(c);
        var config = s.ConfigSMFAndGlobalOptions.ConfigSMF;
        var code = DTOClass(s);
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //            System.Diagnostics.Debugger.Launch();
        //#endif
        context.AddSource(s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix + "DTO", code);
    }


    /// <summary>
    /// Protos the template.
    /// </summary>
    /// <returns>A string.</returns>
    public static string DTOClass(ModelCT s)
    {
        return $$"""                                                         

namespace {{s.NewContainingNamespace}};
public partial class {{s.IdentifierNameWithoutPostFix}}Dto
{
   {{AddMessages(s)}}
}
""";
    }


    /// <summary>
    /// Adds the messages.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>A string.</returns>
    private static string AddMessages(ModelCT s)
    {
        var model = s;
        var messages = new StringBuilder();

        var tempModelCT = model;

        messages.AppendLine(StaticMethods.GetPropertyTemplate("int", "Id"));
        messages.AppendLine(StaticMethods.GetPropertyTemplate("System.DateTime", "CreatedOn"));
        messages.AppendLine(StaticMethods.GetPropertyTemplate("System.DateTime", "LastModifiedOn"));

        while (tempModelCT is not null)
        {
            StaticMethods.AddProperties(tempModelCT, messages);
            tempModelCT = tempModelCT.ParentClassType as ModelCT;
        }
        messages.AppendLine("}").AppendLine();

        // Create Command 


        messages.AppendLine($"public partial class Create{model.ModuleNameWithoutPostFix}_{model.IdentifierNameWithoutPostFix}Command {{");
        tempModelCT = model;
        while (tempModelCT is not null)
        {
            StaticMethods.CreateCommandProperties(tempModelCT, messages);
            tempModelCT = tempModelCT.ParentClassType as ModelCT;
        }
        messages.AppendLine("}");
        messages.AppendLine();

        // Update Command

        messages.AppendLine($"public partial class Update{model.ModuleNameWithoutPostFix}_{model.IdentifierNameWithoutPostFix}Command {{");
        tempModelCT = model;
        messages.AppendLine(StaticMethods.GetPropertyTemplate("int", "Id"));

        while (tempModelCT is not null)
        {
            StaticMethods.UpdateCommandProperties(tempModelCT, messages);
            tempModelCT = tempModelCT.ParentClassType as ModelCT;
        }



        return messages.ToString();
    }


}
