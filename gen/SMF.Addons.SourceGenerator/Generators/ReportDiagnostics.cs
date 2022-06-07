namespace SMF.Addons.SourceGenerator.Generators;

using SMF.SourceGenerator.Core.Types;

/// <summary>
/// The report diagnostics.
/// </summary>
[Generator]

internal class ReportDiagnostics : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //            System.Diagnostics.Debugger.Launch();
        //#endif
        context.RegisterSourceOutput(ModelCTs, ReportDiagnoseForClassTypes);
    }

    /// <summary>
    /// Reports the diagnostics.
    /// </summary>
    /// <param name="c">The c.</param>                        
    /// <param name="classTypes">The class types.</param>
    private static void ReportDiagnoseForClassTypes(SourceProductionContext c, ClassType source)
    {
        var context = new SMFProductionContext(c);

        source.ReportDiagnostics(context);
    }
}
