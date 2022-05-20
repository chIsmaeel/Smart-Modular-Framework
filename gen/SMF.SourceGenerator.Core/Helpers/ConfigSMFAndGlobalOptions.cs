namespace SMF.SourceGenerator.Core.Helpers;

using Microsoft.CodeAnalysis.Diagnostics;
using SMF.SourceGenerator.Core;

public record ConfigSMFAndGlobalOptions(ConfigSMF? ConfigSMF, AnalyzerConfigOptionsProvider OptionsProvider)
{
    private string? _rootNamespace;

    /// <summary>
    /// Gets the root namespace.
    /// </summary>
    public string? RootNamespace
    {
        get
        {
            if (_rootNamespace is not null) return _rootNamespace;
            OptionsProvider.GlobalOptions.TryGetValue("build_property.rootnamespace", out _rootNamespace);
            return _rootNamespace;
        }
    }
}