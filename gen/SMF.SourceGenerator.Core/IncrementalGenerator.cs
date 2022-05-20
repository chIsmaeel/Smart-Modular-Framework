namespace SMF.SourceGenerator.Core;

using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// The incremetnal generator.
/// </summary>
public abstract class IncrementalGenerator : IIncrementalGenerator
{

    /// <summary>
    /// Initializes the.
    /// </summary>
    /// <param name="context">The context.</param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        CreateSyntaxNode(context.SyntaxProvider);
        AdditionalTextsProvider(context.AdditionalTextsProvider);
        AnalyzerConfigOptionsProvider(context.AnalyzerConfigOptionsProvider);
        PostInitialization(context);
        Execute(context);
    }

    /// <summary>
    /// Posts the initialization.
    /// </summary>
    /// <param name="context">The context.</param>
    protected virtual void PostInitialization(IncrementalGeneratorInitializationContext context)
    { }

    /// <summary>
    /// Analyzers the config options provider.
    /// </summary>
    /// <param name="analyzerConfigOptionsProvider">The analyzer config options provider.</param>
    protected abstract void AnalyzerConfigOptionsProvider(IncrementalValueProvider<AnalyzerConfigOptionsProvider> analyzerConfigOptionsProvider);

    /// <summary>
    /// Additionals the texts provider.
    /// </summary>
    /// <param name="additionalTextsProvider">The additional texts provider.</param>
    protected abstract void AdditionalTextsProvider(IncrementalValuesProvider<AdditionalText> additionalTextsProvider);

    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected abstract void Execute(IncrementalGeneratorInitializationContext context);

    /// <summary>
    /// Creates the syntax node.
    /// </summary>
    /// <param name="syntaxProvider">The syntax provider.</param>
    protected virtual void CreateSyntaxNode(SyntaxValueProvider syntaxProvider) { }
}

//#if DEBUG
//                if (!System.Diagnostics.Debugger.IsAttached)
//                    System.Diagnostics.Debugger.Launch();
//#endif
