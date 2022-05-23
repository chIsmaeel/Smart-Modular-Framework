namespace SMF.Addons.SourceGenerator.Generators;

using SMF.Common.SourceGenerator.Abstractions;
using SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;
using System.Collections.Immutable;

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
        context.RegisterImplementationSourceOutput(RegisteredModelCTs.Collect(), AddGlobalNamespaces);
    }

    /// <summary>
    /// Adds the model static classes.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddGlobalNamespaces(SourceProductionContext c, ImmutableArray<ModelCT> s)
    {
        SMFProductionContext context = new(c);
        List<string> globalNamespaces = new()
        {
            $"{s.FirstOrDefault()?.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Application.Interfaces"
        };
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //            System.Diagnostics.Debugger.Launch();
        //#endif
        foreach (var modelCT in s)
        {
            foreach (var property in modelCT.Properties.Where(_ => _!.SMField is not null && _.SMField.Field is not null))
            {
                if ((bool)(property?.SMField?.Field?.Compute)!)
                    globalNamespaces.Add(modelCT.NewContainingNamespace);

            }
        }

        context.AddSource("GlobalNamespaces", string.Join("\n", globalNamespaces.Select(_ => $"global using {_};")));
    }
}
