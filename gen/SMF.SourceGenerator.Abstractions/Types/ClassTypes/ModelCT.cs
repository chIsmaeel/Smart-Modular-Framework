namespace SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;

using Microsoft.CodeAnalysis.CSharp;
using SMF.ORM.Models;
using SMF.SourceGenerator.Core.Types.TypeMembers;


using System.Collections.Immutable;
using System.Text;

/// <summary>
/// Module CTs
/// </summary>
/// <param name="ClassDSs"></param>
/// <param name="CancellationToken"></param>
public partial record ModelCT(IEnumerable<ClassDeclarationSyntax> ClassDSs, ConfigSMFAndGlobalOptions ConfigSMFAndGlobalOptions, CancellationToken CancellationToken) : ClassType(ClassDSs, ConfigSMFAndGlobalOptions, CancellationToken)
{
    private IEnumerable<ModuleCT>? _registeringModuleCTs;
    private TypeConstructor? _defaultConstructorDSs;
    private string? _stringParentType;
    private bool _hasParameterlessConstructor = false;
    private string? _qualifiedParentName;
    private string? _identifierNameWithoutPostFix;
    private string? _qualifiedNameWithoutPostFix;
    private readonly List<(string fieldName, Order order)>? _orderBy = new();
    private string? _newContainingNamespace;
    private string? _orderByString;
    private string? _newQualifiedName;
    private string? _moduleNameWithoutPostFix;

    /// <summary>
    /// Gets or sets the registering module c ts.
    /// </summary>                                                    
    public IEnumerable<ModuleCT> GetRegisteringModuleCTs((IEnumerable<ModuleCT> moduleCTs, IEnumerable<ModelCT> modelCTs) moduleCTsWithModelCTs)
    {
        if (_registeringModuleCTs is not null) return _registeringModuleCTs;
        var registerModuleCTs = ImmutableArray.CreateBuilder<ModuleCT?>();

        foreach (var classDeclaration in moduleCTsWithModelCTs.moduleCTs.SelectMany(_ => _.ClassDSs))
        {
            var registeredModelFDS = classDeclaration?.ChildNodes()?.OfType<FieldDeclarationSyntax>()?
                 .FirstOrDefault(_ => _.Declaration.Variables.FirstOrDefault()!.ToString() == "_registerModel");
            var genericNameSyntax = (GenericNameSyntax?)registeredModelFDS?.DescendantNodes()?
                .FirstOrDefault(_ => _.IsKind(SyntaxKind.GenericName));
            // Report Diagnostic if the registering Module is not found.
            foreach (var modelName in genericNameSyntax?.TypeArgumentList?.Arguments.Select(_ => _.ToString())!)
            {
                var modelQualifiedName = classDeclaration?.GetAllPossibleQualifiedNames(modelName).FirstOrDefault(_ => _ == QualifiedName);
                if (modelQualifiedName is not null)
                {
                    var foundModuleCT = moduleCTsWithModelCTs.moduleCTs.FirstOrDefault(_ => _.QualifiedName == modelQualifiedName);
                    if (foundModuleCT is not null)
                        registerModuleCTs.Add(foundModuleCT);
                    continue;
                }
            }
        }
        _registeringModuleCTs = registerModuleCTs.ToImmutable();
        return _registeringModuleCTs;

    }

    /// <summary>
    /// Gets the inherit type field.                              
    /// </summary>
    public virtual TypeConstructor? ParameterlessConstructor
    {
        get
        {

            if (_defaultConstructorDSs is not null && _hasParameterlessConstructor) return _defaultConstructorDSs;
            _defaultConstructorDSs = Constructors.FirstOrDefault(_ => _!.IsParameterlessConstructor);
            _hasParameterlessConstructor = true;
            return _defaultConstructorDSs;
        }
    }


    /// <summary>
    /// Gets the parent type.                   
    /// </summary>
    public override string? StringParentType
    {
        get
        {
            if (_stringParentType is not null) return _stringParentType;
            if (ParameterlessConstructor?.SimpleAssignmentStatments is IEnumerable<AssignmentExpressionSyntax> simpleMemeberAccessExpression && simpleMemeberAccessExpression.Count() > 0)
            {
                var expression = simpleMemeberAccessExpression.FirstOrDefault(_ => _.Left.ToString() is "InheritModel" or "this.InheritModel");
                if (expression is not null && expression.Right.ToString().Split('.') is var pType && pType?.Length == 2 && pType[1].EndsWith("Model"))
                {
                    _stringParentType = pType.LastOrDefault();
                    if (_stringParentType != "" && _stringParentType.Contains('_'))
                    {
                        return _stringParentType = _stringParentType?.Substring(_stringParentType.LastIndexOf('_') + 1);
                    }
                }
            }
            return _stringParentType = "ModelBase";
        }
    }

    /// <summary>
    /// Gets the order by.
    /// </summary>
    public List<(string fieldName, Order order)>? OrderBy
    {
        get
        {
            if (_orderBy?.Count > 0) return _orderBy;
            if (ParameterlessConstructor?.SimpleAssignmentStatments is IEnumerable<AssignmentExpressionSyntax> simpleMemeberAccessExpression && simpleMemeberAccessExpression.Count() > 0)
            {
                var expression = simpleMemeberAccessExpression.FirstOrDefault(_ => _.Left.ToString() is "OrderBy" or "this.OrderBy");
                if (expression is not null && expression.Right.IsKind(SyntaxKind.ImplicitObjectCreationExpression))
                {
                    var objectCreationExpression = expression.DescendantNodes().OfType<ImplicitObjectCreationExpressionSyntax>().FirstOrDefault();
                    if (objectCreationExpression.ArgumentList.Arguments.Count() > 0)
                    {
                        foreach (var arg in objectCreationExpression.ArgumentList.Arguments)
                        {
                            if (arg.Expression is TupleExpressionSyntax tupleExpressionSyntax && tupleExpressionSyntax.Arguments.Count == 2)
                            {
                                var fieldName = tupleExpressionSyntax.Arguments[0];
                                var orderBy = tupleExpressionSyntax.Arguments[1].ToString();
                                _orderBy!.Add((fieldName.ToString(), orderBy == "Order.Ascending" ? Order.Ascending : Order.Descending));
                            }
                        }
                        return _orderBy;
                    }
                }

            }
            _orderBy!.Add(("Id", Order.Ascending));
            return _orderBy;
        }
    }

    /// <summary>
    /// Gets the order by string.
    /// </summary>
    public string OrderByString
    {
        get
        {
            if (_orderByString is not null) return _orderByString;
            StringBuilder sb = new();
            foreach (var (fieldName, order) in OrderBy!)
            {
                if (order == Order.Ascending)
                    sb.Append($".OrderBy(_=>_.{fieldName})");
                else
                    sb.Append($".OrderByDescending(_=>_.{fieldName})");
            }
            return _orderByString = sb.ToString();
        }
    }

    /// <summary>
    /// Gets the identifier name without model post fix.
    /// </summary>
    public string IdentifierNameWithoutPostFix
    {
        get
        {
            if (_identifierNameWithoutPostFix is not null) return _identifierNameWithoutPostFix;
            return _identifierNameWithoutPostFix = IdentifierName.Substring(0, IdentifierName.Length - "Model".Length);
        }
    }



    /// <summary>
    /// Gets the qualified name without post fix.
    /// </summary>
    public string? QualifiedNameWithoutPostFix
    {
        get
        {
            if (_qualifiedNameWithoutPostFix is not null) return _qualifiedNameWithoutPostFix;
            return _qualifiedNameWithoutPostFix = QualifiedName!.Substring(0, QualifiedName.Length - "Model".Length);
        }
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
            _containingModuleName = ContainingNamespace.Substring(ConfigSMFAndGlobalOptions.RootNamespace!.Length + 1, ContainingNamespace.IndexOf("Addon.") - ConfigSMFAndGlobalOptions.RootNamespace!.Length + 4);
            if (_containingModuleName is null) return null;
            return _containingModuleName;
        }

    }

    /// <summary>
    /// Gets the module name without post fix.
    /// </summary>
    public string ModuleNameWithoutPostFix
    {
        get
        {
            if (_moduleNameWithoutPostFix is not null) return _moduleNameWithoutPostFix;
            return _moduleNameWithoutPostFix = ContainingModuleName!.Substring(0, ContainingModuleName.Length - "Addon".Length);
        }
    }

    /// <summary>
    /// Gets the qualified parent name.
    /// </summary>
    public string QualifiedParentName
    {
        get
        {
            if (_qualifiedParentName is not null) return _qualifiedParentName;
            if (ParameterlessConstructor?.SimpleAssignmentStatments is IEnumerable<AssignmentExpressionSyntax> simpleMemeberAccessExpression && simpleMemeberAccessExpression.Count() > 0)
            {
                var expression = simpleMemeberAccessExpression.FirstOrDefault(_ => _.Left.ToString() is "InheritModel" or "this.InheritModel");
                if (expression is not null && expression.Right.ToString().Split('.') is var pType && pType?.Length == 2 && pType[1].EndsWith("Model"))
                {
                    var parentType = pType.LastOrDefault();
                    if (parentType != "" && parentType.Contains('_'))
                    {
                        return _qualifiedParentName = ConfigSMFAndGlobalOptions?.RootNamespace + "." + parentType?.Substring(0, parentType.IndexOf('_'))! + "Addon.Models." + parentType?.Substring(parentType.LastIndexOf('_') + 1)!;
                    }
                }
            }
            return "SMF.ORM.Models.ModelBase";
        }
    }

    /// <summary>
    /// Gets the new containing namespace.
    /// </summary>
    public override string NewContainingNamespace
    {
        get
        {
            if (_newContainingNamespace is not null) return _newContainingNamespace;
            _newContainingNamespace = $"{ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Domain.{ContainingModuleName}.Entities";
            return _newContainingNamespace;
        }
    }

    /// <summary>
    /// Gets the new qualified name.
    /// </summary>
    public override string NewQualifiedName
    {
        get
        {
            if (_newQualifiedName is not null) return _newQualifiedName;
            return _newQualifiedName = $"{ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Domain.{ContainingModuleName}.Entities.{IdentifierNameWithoutPostFix}";
        }
    }
}