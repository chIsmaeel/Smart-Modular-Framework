namespace SMF.SourceGenerator.Core.Types.TypeMembers;

public partial record TypeMethod(MethodDeclarationSyntax? MDS)
{
    private IMethodSymbol? _symbol;
    private string? _identifierName;

    /// <summary>
    /// Gets the symbol.
    /// </summary>
    public IMethodSymbol? GetSymbol(Compilation compilation)
    {
        return _symbol ??= Symbols.GetDeclaredMethodSymbol(MDS!, compilation);
    }
    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    public string IdentifierName => _identifierName ??= MDS!.Identifier.Text;

}