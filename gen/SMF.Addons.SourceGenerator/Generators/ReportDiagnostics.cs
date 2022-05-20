namespace SMF.Addons.SourceGenerator.Generators;

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
        //context.RegisterSourceOutput(ClassTypes, ReportDiagnoseForClassTypes);
    }

    /// <summary>
    ///// Reports the diagnostics.
    ///// </summary>
    ///// <param name="c">The c.</param>                        
    ///// <param name="classTypes">The class types.</param>
    //private static void ReportDiagnoseForClassTypes(SourceProductionContext c, ClassTypes source)
    //{
    //    var context = new SMFProductionContext(c);

    //    // Report Diagnostics Module Class Type. 
    //    foreach (var csType in source.ModuleCTs!)
    //    {
    //        csType.ReportDiagnostics(context);
    //    }

    //    // Report Diagnostics Model Class Type.
    //    foreach (var csType in source.ModelCTs!)
    //    {
    //        csType.ReportDiagnostics(context);
    //    }

    //    // Report Diagnostics Controller Class Type.
    //    foreach (var csType in source.ControllerCTs!)
    //    {
    //        csType.ReportDiagnostics(context);
    //    }
    //}
}
