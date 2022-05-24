namespace SMF.SourceGenerator.Core.Helpers;

using Microsoft.CodeAnalysis.Diagnostics;
using SMF.SourceGenerator.Core;

public record ConfigSMFAndGlobalOptions(ConfigSMF? ConfigSMF, AnalyzerConfigOptionsProvider OptionsProvider)
{
    private string? _rootNamespace;
    private string? _generatorProjectPath;

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

    /// <summary>
    /// Gets the generator project path.
    /// </summary>
    public string? GeneratorProjectPath
    {
        get
        {
            if (_generatorProjectPath is not null) return _generatorProjectPath;
            OptionsProvider.GlobalOptions.TryGetValue("build_property.projectdir", out _generatorProjectPath);
            return _generatorProjectPath;
        }
    }
}