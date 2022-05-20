namespace SMF.SourceGenerator.Core.Types;
using System.Collections.Immutable;

/// <summary>
/// This class represents a class type.
/// </summary>
/// <param name="CDSs"></param>
public partial record ClassType(IEnumerable<ClassDeclarationSyntax> ClassDSs, ConfigSMFAndGlobalOptions ConfigSMFAndGlobalOptions, CancellationToken CancellationToken) : ICSType
{
    private INamedTypeSymbol? _symbol;
    private string? _identifierName;
    private IEnumerable<TypeField?>? _fields;
    private readonly List<TypeProperty?> _properties = new();
    private IEnumerable<TypeMethod?>? _methods;
    private IEnumerable<TypeConstructor?>? _constructors;
    private IEnumerable<string>? _inheritTypes;
    private string? _comment;
    private string? _qualifiedName;
    private string _containingNamespace = string.Empty;
    private string? _parentType;
    private IEnumerable<string>? _interfaces;
    private IEnumerable<string>? _usings;
    private string _newContainingNamespace = string.Empty;
    private string _newQualifiedName = string.Empty;

    /// <summary>
    /// Gets the identifier name.
    /// </summary>                            
    public string IdentifierName => _identifierName ??= ClassDSs?.FirstOrDefault()?.Identifier.ValueText!;

    /// <summary>
    /// Gets or sets the parent class type.
    /// </summary>
    public ClassType? ParentClassType { get; set; }

    /// <summary>
    /// Sets the parent class type.
    /// </summary>
    /// <param name="classType">The class type.</param>
    public void SetParentClassType(ClassType? classType)
    {
        if (classType is not null)
            ParentClassType = classType;
    }
    /// <summary>
    /// Gets the qualified name.                                      
    /// </summary>
    public string? QualifiedName
    {
        get => _qualifiedName ??= ClassDSs.FirstOrDefault()!.GetQualifiedName();
        set => _qualifiedName = value;
    }

    /// <summary>
    /// Gets the qualified name.                                      
    /// </summary>
    public virtual string? NewQualifiedName => _newQualifiedName ??= QualifiedName!.Replace(ConfigSMFAndGlobalOptions.RootNamespace, ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME);

    /// <summary>
    /// Gets the containing namespace.
    /// </summary>
    public string ContainingNamespace
    {
        get
        {
            if (_containingNamespace != "") return _containingNamespace;
            var namespaceName = ClassDSs.FirstOrDefault()?.GetContainingNamespace();
            if (namespaceName is null) _containingNamespace = "NoNamespace";
            else _containingNamespace = namespaceName;
            return _containingNamespace;
        }
        set => _containingNamespace = value;
    }

    /// <summary>
    /// Gets or sets the new containing namespace.
    /// </summary>
    public virtual string NewContainingNamespace
    {
        get
        {
            if (_newContainingNamespace != "") return _newContainingNamespace;
            if (ConfigSMFAndGlobalOptions is null) return ContainingNamespace;
            _newContainingNamespace = ContainingNamespace.Replace(ConfigSMFAndGlobalOptions!.RootNamespace, ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME);
            return _newContainingNamespace;
        }
    }



    /// <summary>
    /// Gets the usings.
    /// </summary>
    public IEnumerable<string> Usings
    {
        get
        {
            if (_usings is not null) return _usings;
            _usings = new List<string>();
            foreach (var cds in ClassDSs)
            {
                var result = cds.GetAllPossibleQualifiedNames();
                if (result?.Count() > 0)
                    (_usings as List<string>)!.AddRange(result!);
            }
            return _usings;
        }
    }

    /// <summary>                              
    /// Gets the symbol.
    /// </summary>        
    public INamedTypeSymbol? GetSymbol(Compilation compilation)
    {
        return _symbol ??= Symbols.GetDeclaredNamedTypeSymbol(ClassDSs.FirstOrDefault()!, compilation);
    }

    /// <summary>
    /// Gets the properties.
    /// </summary>
    public IEnumerable<TypeField?>? Fields
    {
        get
        {
            if (_fields is not null) return _fields;
            ImmutableArray<TypeField?>.Builder? fields = ImmutableArray.CreateBuilder<TypeField?>();
            foreach (ClassDeclarationSyntax? cds in ClassDSs!)
                foreach (FieldDeclarationSyntax? fds in cds!.ChildNodes().OfType<FieldDeclarationSyntax>())
                {
                    TypeField? fieldType = new(fds);
                    fields.Add(fieldType);
                }
            _fields = fields.ToImmutable();
            return _fields;
        }
    }

    /// <summary>
    /// Gets the properties.
    /// </summary>
    public List<TypeProperty?> Properties
    {
        get
        {

            if (_properties is not null) return _properties;
            foreach (ClassDeclarationSyntax? cds in ClassDSs!)
                foreach (PropertyDeclarationSyntax? pds in cds!.ChildNodes().OfType<PropertyDeclarationSyntax>())
                {
                    TypeProperty? propertyClass = new(pds, this);
                    _properties!.Add(propertyClass);
                }
            return _properties!;
        }
    }

    /// <summary>
    /// Creates the property.
    /// </summary>
    /// <param name="typeName">The type name.</param>
    /// <param name="identiferName">The identifer name.</param>
    public static PropertyDeclarationSyntax CreateProperty(string typeName, string identiferName)
    {
        return SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(typeName), identiferName)
        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
        .AddAccessorListAccessors(
         SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
         SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
    }

    /// <summary>
    /// Gets the properties.
    /// </summary>
    public IEnumerable<TypeConstructor?>? Constructors
    {
        get
        {
            if (_constructors is not null) return _constructors;
            ImmutableArray<TypeConstructor?>.Builder? constructors = ImmutableArray.CreateBuilder<TypeConstructor?>();
            foreach (ClassDeclarationSyntax? cds in ClassDSs!)
                foreach (ConstructorDeclarationSyntax? mds in cds!.ChildNodes().OfType<ConstructorDeclarationSyntax>())
                {
                    TypeConstructor? typeConstructor = new(mds);
                    constructors.Add(typeConstructor);
                }
            _constructors = constructors.ToImmutable();
            return _constructors;
        }
    }

    /// <summary>
    /// Gets the properties.
    /// </summary>
    public IEnumerable<TypeMethod?>? Methods
    {
        get
        {
            if (_methods is not null) return _methods;
            ImmutableArray<TypeMethod?>.Builder? methods = ImmutableArray.CreateBuilder<TypeMethod?>();
            foreach (ClassDeclarationSyntax? cds in ClassDSs!)
                foreach (MethodDeclarationSyntax? mds in cds!.ChildNodes().OfType<MethodDeclarationSyntax>())
                {
                    TypeMethod? typeMethod = new(mds);
                    methods.Add(typeMethod);
                }
            _methods = methods.ToImmutable();
            return _methods;
        }
    }

    /// <summary>
    /// Gets the parent type.
    /// </summary>
    public virtual string? StringParentType
    {
        get
        {
            if (_parentType is not null) return _parentType;
            if (InheritTypes is null) return null;
            _parentType = InheritTypes?.FirstOrDefault(_ => _[0] == 'I' && char.IsUpper(_[1]));
            return _parentType;
        }
    }

    /// <summary>
    /// Gets the interfaces.
    /// </summary>
    public virtual IEnumerable<string>? Interfaces
    {
        get
        {
            if (_interfaces is not null) return _interfaces;
            _interfaces = InheritTypes?.Where(_ => _[0] != 'I' && char.IsLower(_[1]));
            return _interfaces;
        }
    }

    /// <summary>
    /// Gets the base type.
    /// </summary>
    public virtual IEnumerable<string>? InheritTypes
    {
        get
        {
            if (_inheritTypes is not null) return _inheritTypes;
            foreach (var cds in ClassDSs!)
            {
                _inheritTypes = cds.BaseList?.Types.Select(_ => _.ToString());
                if (_inheritTypes is null) continue;
            }
            return _inheritTypes;
        }
    }

    /// <summary>
    /// Gets the comment.
    /// </summary>
    public string? Comment
    {
        get
        {
            if (_comment is not null) return _comment;
            _comment = CommentTemplate.GetCommentToken(ClassDSs.FirstOrDefault(_ => _!.GetLeadingTrivia().ToString().Trim() != "")?.GetLeadingTrivia().ToString().Trim());
            if (_comment is null)
                _comment = IdentifierName;
            return _comment;
        }
    }

    /// <summary>
    /// Reports the diagnostics.
    /// </summary>
    public virtual void ReportDiagnostics(SMFProductionContext context)
    {
        ReportDiagnosticIfMoreThanOneCommentAreAvailable(context);
    }

    /// <summary>
    /// Reports the diagnostic if more than one comment are available.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="partialClasses">The partial classes.</param>                                           
    private void ReportDiagnosticIfMoreThanOneCommentAreAvailable(SMFProductionContext context)
    {
        var declaredCommentCDSs = ClassDSs.Where(_ => !string.IsNullOrEmpty(_!.GetLeadingTrivia().ToString().Trim()));
        var length = declaredCommentCDSs?.Count();

        // Report if no comment are available.
        if (length == 0)
            foreach (var cds in ClassDSs!)
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.ShouldHaveCommentInAtleastOneSMFType, cds!.GetLocation(), IdentifierName));

        // Report If more than one comment is found.
        if (length > 1)
            foreach (var c in declaredCommentCDSs!)
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.ShouldNotBeCommentOnMoreThanSingleType, c!.GetLeadingTrivia().FirstOrDefault().GetLocation(), c.Identifier.ValueText));
    }
}
