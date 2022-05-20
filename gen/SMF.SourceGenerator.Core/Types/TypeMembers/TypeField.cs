namespace SMF.SourceGenerator.Core.Types.TypeMembers;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/// <summary>
/// The property Class Type.
/// </summary>
/// <param name="FDS">The Property Declaration Syntax.</param>
/// <param name="Compilation">The Compilation.</param>
public partial record TypeField(FieldDeclarationSyntax? FDS)
{
    private IFieldSymbol? _symbol;
    private string? _defaultAssignedValue;
    private IEnumerable<string>? _genericTypeArguments;
    private string? _typeName;
    private string? _identifierName;

    /// <summary>
    /// Gets the symbol.
    /// </summary>                 
    public IFieldSymbol? GetSymbol(Compilation compilation)
    {
        return _symbol ??= Symbols.GetDeclaredFieldSymbol(FDS!, compilation);
    }

    /// <summary>
    /// Gets the parent c d s.
    /// </summary>
    public ClassDeclarationSyntax? ParentCDS => FDS?.Parent as ClassDeclarationSyntax;

    /// <summary>
    /// Gets the type name.
    /// </summary>
    public string TypeName => _typeName ??= FDS!.Declaration.Type.ToString();
    /// <summary>
    /// Gets the name.
    /// </summary>
    public string IdentifierName => _identifierName ??= FDS!.Declaration.Variables[0].Identifier.Text;
    /// <summary>
    /// Gets the default assigned value.
    /// </summary>
    public string? DefaultAssignedValue
    {
        get
        {
            if (_defaultAssignedValue is not null) return _defaultAssignedValue;
            _defaultAssignedValue = FDS?.Declaration.Variables.FirstOrDefault()?.Initializer?.Value.ToString();
            return _defaultAssignedValue;
        }
    }

    /// <summary>
    /// Gets the generic type arguments.
    /// </summary>
    public IEnumerable<string>? GenericTypeArguments
    {
        get
        {
            if (_genericTypeArguments is not null) return _genericTypeArguments;
            _genericTypeArguments = (FDS?.DescendantNodes()?
                 .FirstOrDefault(_ => _.IsKind(SyntaxKind.GenericName))! as GenericNameSyntax)?
                 .TypeArgumentList?.Arguments.Select(_ => _.ToString()!);
            return _genericTypeArguments;
        }
    }
}