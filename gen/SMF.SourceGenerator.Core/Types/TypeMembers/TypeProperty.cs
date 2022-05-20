namespace SMF.SourceGenerator.Core.Types.TypeMembers;

using Microsoft.CodeAnalysis.CSharp;

/// <summary>
/// The property Class Type.
/// </summary>
/// <param name="PDS">The Property Declaration Syntax.</param>
/// <param name="Compilation">The Compilation.</param>
public partial record TypeProperty(PropertyDeclarationSyntax PDS, ClassType ClassType, bool HasRelationship = false)
{
    private IPropertySymbol? _symbol;
    private bool _hasObjectInitialization = false;
    private IEnumerable<(string, string)>? _assignmentExpressionsIdentiferAndValue;
    private string _type = string.Empty;
    private string? _identiferName;
    private string? _comment;

    /// <summary>                              
    /// Gets the symbol.
    /// </summary>
    public IPropertySymbol? GetSymbol(Compilation compilation)
    {
        return _symbol ??= Symbols.GetDeclaredPropertySymbol(PDS!, compilation);
    }

    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    public string IdentifierName => _identiferName ??= PDS.Identifier.ValueText;

    /// <summary>
    /// Gets the comment.
    /// </summary>
    public string Comment
    {
        get
        {
            if (_comment is not null) return _comment;
            return _comment = CommentTemplate.GetCommentToken(PDS.GetLeadingTrivia().ToString())!;
        }
    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    public string Type
    {
        get
        {
            if (_type != string.Empty) return _type;
            return _type = PDS.Type.ToString();
        }
    }

    /// <summary>
    /// Gets the object initialization.
    /// </summary>
    public IEnumerable<(string Identifer, string Value)>? AssignmentExpressionsIdentiferAndValue
    {
        get
        {
            if (_assignmentExpressionsIdentiferAndValue is not null && _hasObjectInitialization) return _assignmentExpressionsIdentiferAndValue;
            _hasObjectInitialization = true;
            var tempList = new List<(string, string)>();
            if (PDS.DescendantNodes().Any(_ => _.IsKind(SyntaxKind.ObjectInitializerExpression)))
            {

                PDS.DescendantNodes()?.OfType<AssignmentExpressionSyntax>()?.ForEach(_ =>
                {
                    var left = _.Left.ToString();
                    var right = _.Right.ToString();
                    tempList.Add((left, right));
                });
                return _assignmentExpressionsIdentiferAndValue = tempList;
            }
            return _assignmentExpressionsIdentiferAndValue = null;
        }
    }
}