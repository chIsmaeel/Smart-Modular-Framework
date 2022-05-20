namespace SMF.SourceGenerator.Core.Types.TypeMembers;

public partial record TypeConstructor(ConstructorDeclarationSyntax ConstructorDS)
{
    private IMethodSymbol? _symbol;
    private IEnumerable<(string, string)>? _parameters;
    private string _modifiers = string.Empty;
    private bool _hasNoParameters = false;
    private IEnumerable<StatementSyntax>? _memberNodes;
    private IEnumerable<AssignmentExpressionSyntax>? _simpleAssignmentStatments;

    /// <summary>
    /// Gets the symbol.
    /// </summary>
    public IMethodSymbol? GetSymbol(Compilation compilation)
    {
        return _symbol ??= Symbols.GetDeclaredConstructorSymbol(ConstructorDS!, compilation);
    }

    /// <summary>                                                        
    /// Gets the parameters.
    /// </summary>
    public IEnumerable<(string, string)>? Parameters
    {
        get
        {
            if (_parameters is not null && _hasNoParameters) return _parameters;
            _parameters = ConstructorDS.ParameterList.Parameters.Select(_ => (_.Type!.ToString(), _.Identifier.ValueText.ToString()));
            if (_parameters is null)
                _hasNoParameters = true;
            return _parameters;
        }
    }

    /// <summary>
    /// Gets a value indicating whether parameterless is constructor.
    /// </summary>
    public bool IsParameterlessConstructor => Parameters?.Count() == 0;

    /// <summary>
    /// Gets the member nodes.
    /// </summary>
    public IEnumerable<StatementSyntax>? MemberNodes
    {
        get
        {
            if (_memberNodes is not null) return _memberNodes;
            _memberNodes = ConstructorDS.Body?.Statements;
            return _memberNodes;
        }
    }

    /// <summary>
    /// Gets the simple assignment statments.
    /// </summary>
    public IEnumerable<AssignmentExpressionSyntax>? SimpleAssignmentStatments
    {
        get
        {
            if (_simpleAssignmentStatments is not null) return _simpleAssignmentStatments;

            _simpleAssignmentStatments = MemberNodes.Where(_ => _ is ExpressionStatementSyntax).SelectMany(_ => _.ChildNodes().OfType<AssignmentExpressionSyntax>());
            return _simpleAssignmentStatments;
        }
    }

    /// <summary>
    /// Gets the modifiers.
    /// </summary>
    public string Modifiers
    {
        get
        {
            if (_modifiers is not null) return _modifiers;
            _modifiers = ConstructorDS.Modifiers.ToString();
            return _modifiers;
        }
    }
}