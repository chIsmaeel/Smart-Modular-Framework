namespace SMF.Addons.SourceGenerator.Generators;

using SMF.Common.SourceGenerator.Abstractions;

/// <summary>
/// The global namespace generator.
/// </summary>
[Generator]
public class GlobalUsingsGenerator : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterImplementationSourceOutput(GlobalUsings, AddGlobalNamespaces);
    }

    /// <summary>
    /// Adds the model static classes.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddGlobalNamespaces(SourceProductionContext c, string s)
    {
        SMFProductionContext context = new(c);

        context.AddSource("GlobalNamespaces", "global using SMFields = SMF.ORM.Fields;\nglobal using SMF.ORM.Models;\nglobal using SMF.Addons;");
    }
}
