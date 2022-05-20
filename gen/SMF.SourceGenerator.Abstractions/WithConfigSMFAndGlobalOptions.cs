namespace SMF.Common.SourceGenerator.Abstractions;

using Microsoft.CodeAnalysis.Diagnostics;
using SMF.SourceGenerator.Core.Types.Interfaces;

public record WithConfigSMFAndGlobalOptions<T>(T Type, ConfigSMF? ConfigSMF, AnalyzerConfigOptionsProvider OptionsProvider) : ConfigSMFAndGlobalOptions(ConfigSMF, OptionsProvider);

public record ICSTypeWithConfigSMFAndGlobalOptions<T>(T Type, ConfigSMF? ConfigSMF, AnalyzerConfigOptionsProvider OptionsProvider) : ConfigSMFAndGlobalOptions(ConfigSMF, OptionsProvider) where T : class, ICSType
{
    /// <summary>
    /// Gets the new qualified name.
    /// </summary>
    /// <returns>A string.</returns>
    public string GetNewQualifiedName()
    {
        return Type.QualifiedName!.Replace(RootNamespace, ConfigSMF!.SOLUTION_NAME);
    }

    /// <summary>
    /// Gets the new qualified name without post fix.
    /// </summary>
    /// <returns>A string.</returns>
    public string GetNewQualifiedNameWithoutPostFix()
    {
        if (Type is ModelCT)
            return (Type as ModelCT)!.QualifiedNameWithoutPostFix!.Replace(RootNamespace, ConfigSMF!.SOLUTION_NAME);
        return Type.QualifiedName!.Replace(RootNamespace, ConfigSMF!.SOLUTION_NAME);
    }

    private string? _containingModuleName;

    /// <summary>
    /// Gets the containing module name.
    /// </summary>
    public string? ContainingModuleName
    {
        get
        {
            if (_containingModuleName is not null) return _containingModuleName;
            var cNamespace = Type.ContainingNamespace;
            _containingModuleName = cNamespace.Substring(RootNamespace!.Length + 1, cNamespace.IndexOf("Addon.") - RootNamespace!.Length + 4);
            if (_containingModuleName is null) return null;
            return _containingModuleName;
        }

    }


    /// <summary>
    /// Gets the new containing namespace.
    /// </summary>
    /// <returns>A string.</returns>
    public string GetNewContainingNamespace()
    {
        return Type.ContainingNamespace!.Replace(RootNamespace, ConfigSMF!.SOLUTION_NAME);
    }
}