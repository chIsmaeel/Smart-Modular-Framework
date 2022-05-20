namespace SMF.SourceGenerator.Core.Helpers;

/// <summary>
/// The symbols.
/// </summary>

public static class Symbols
{
    /// <summary>
    /// Gets the declared named type symbol.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <param name="compilation">The compilation.</param>
    /// <returns>An INamedTypeSymbol? .</returns>
    public static INamedTypeSymbol? GetDeclaredNamedTypeSymbol(ClassDeclarationSyntax cds, Compilation compilation)
    {
        CompilationUnitSyntax? compilationUnitSyntax = cds?.FirstAncestorOrSelf<CompilationUnitSyntax>();
        SemanticModel? compilationSemanticModel = compilation?.GetSemanticModel(compilationUnitSyntax!.SyntaxTree);
        return compilationSemanticModel?.GetDeclaredSymbol(cds!);
    }

    /// <summary>
    /// Gets the declared property symbol.
    /// </summary>
    /// <param name="pds">The pds.</param>
    /// <param name="compilation">The compilation.</param>
    /// <returns>An IPropertySymbol? .</returns>                           
    public static IFieldSymbol? GetDeclaredFieldSymbol(FieldDeclarationSyntax fds, Compilation compilation)
    {
        CompilationUnitSyntax? compilationUnitSyntax = fds.FirstAncestorOrSelf<CompilationUnitSyntax>();
        SemanticModel? compilationSemanticModel = compilation?.GetSemanticModel(compilationUnitSyntax!.SyntaxTree);
        return (IFieldSymbol?)compilationSemanticModel?.GetDeclaredSymbol(fds.Declaration.Variables.FirstOrDefault()!);
    }

    /// <summary>
    /// Gets the declared method symbol.
    /// </summary>
    /// <param name="pds">The pds.</param>
    /// <param name="compilation">The compilation.</param>
    /// <returns>An IPropertySymbol? .</returns>
    public static IPropertySymbol? GetDeclaredPropertySymbol(PropertyDeclarationSyntax pds, Compilation compilation)
    {
        CompilationUnitSyntax? compilationUnitSyntax = pds?.FirstAncestorOrSelf<CompilationUnitSyntax>();
        SemanticModel? compilationSemanticModel = compilation?.GetSemanticModel(compilationUnitSyntax!.SyntaxTree);
        return compilationSemanticModel?.GetDeclaredSymbol(pds!);
    }

    /// <summary>
    /// Gets the declared property symbol.
    /// </summary>
    /// <param name="pds">The pds.</param>
    /// <param name="compilation">The compilation.</param>
    /// <returns>An IPropertySymbol? .</returns>
    public static IMethodSymbol? GetDeclaredConstructorSymbol(ConstructorDeclarationSyntax cds, Compilation compilation)
    {
        CompilationUnitSyntax? compilationUnitSyntax = cds?.FirstAncestorOrSelf<CompilationUnitSyntax>();
        SemanticModel? compilationSemanticModel = compilation?.GetSemanticModel(compilationUnitSyntax!.SyntaxTree);
        return compilationSemanticModel?.GetDeclaredSymbol(cds!);
    }

    /// <summary>
    /// Gets the declared method symbol.
    /// </summary>
    /// <param name="mds">The mds.</param>
    /// <param name="compilation">The compilation.</param>
    /// <returns>An IMethodSymbol? .</returns>
    public static IMethodSymbol? GetDeclaredMethodSymbol(MethodDeclarationSyntax mds, Compilation compilation)
    {
        CompilationUnitSyntax? compilationUnitSyntax = mds?.FirstAncestorOrSelf<CompilationUnitSyntax>();
        SemanticModel? compilationSemanticModel = compilation?.GetSemanticModel(compilationUnitSyntax!.SyntaxTree);
        return compilationSemanticModel?.GetDeclaredSymbol(mds!);
    }
}
