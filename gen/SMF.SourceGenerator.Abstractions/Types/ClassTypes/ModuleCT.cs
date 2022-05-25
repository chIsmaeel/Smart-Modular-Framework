namespace SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;

using Microsoft.CodeAnalysis.CSharp;
using SMF.SourceGenerator.Core.Diagnostics;
using SMF.SourceGenerator.Core.Types.TypeMembers;
using System.Collections.Immutable;

public partial record ModuleCT(IEnumerable<ClassDeclarationSyntax> ClassDSs, ConfigSMFAndGlobalOptions ConfigSMFAndGlobalOptions, CancellationToken CancellationToken) : ClassType(ClassDSs, ConfigSMFAndGlobalOptions, CancellationToken)
{
    private IEnumerable<ModelCT?>? _registeredModelCTs;
    private IEnumerable<string?>? _registeredModelNames;
    private TypeField? _registeredModelTypeField;
    private string? _identifierNameWithoutPostFix;

    //private readonly List<string?>? _invalidRegisteredModelCTs = new();
    /// <summary>                                                      
    /// Gets the registered model c ts.
    /// </summary>                                        
    public IEnumerable<ModelCT?>? GetRegisteredModelCTs((IEnumerable<ModuleCT> moduleCTs, IEnumerable<ModelCT> modelCTs) moduleCTsWithModelCTs)
    {
        if (_registeredModelCTs is not null) return _registeredModelCTs;
        var registerModelCTs = ImmutableArray.CreateBuilder<ModelCT>();
        if (RegisteredModelTypeField is not null)
        {
            foreach (string registeredModelName in RegisteredModelTypeField.GenericTypeArguments!)
            {
                // Continue If the registered model name has Invalid State then continue;
                // as well as if the registered model CDSs is null or empty then also continue.
                var registeringModelCT = moduleCTsWithModelCTs.modelCTs.FirstOrDefault(_ => _.QualifiedName == ContainingNamespace + "." + registeredModelName);
                if (registeringModelCT is not null)
                {
                    registerModelCTs.Add(registeringModelCT);
                    continue;
                }

                var modelQualifiedName = RegisteredModelTypeField.ParentCDS?.GetAllPossibleQualifiedNames(registeredModelName);
                if (modelQualifiedName is not null)
                {
                    var foundModelCT = moduleCTsWithModelCTs.modelCTs.FirstOrDefault(_ => _.QualifiedName == modelQualifiedName.FirstOrDefault(qn => qn == _.QualifiedName));
                    if (foundModelCT is not null)
                        registerModelCTs.Add(foundModelCT);
                    continue;
                }
            }
        }
        _registeredModelCTs = registerModelCTs.ToImmutable();
        return _registeredModelCTs;
    }

    /// <summary>
    /// Gets the identifier name without post fix.
    /// </summary>
    public string IdentifierNameWithoutPostFix => _identifierNameWithoutPostFix ??= IdentifierName.Substring(0, IdentifierName.Length - "Module".Length);



    /// <summary>
    /// Gets the registered model names.
    /// </summary>
    public IEnumerable<string?>? RegisteredModelNames
    {
        get
        {
            if (_registeredModelNames is not null) return _registeredModelNames;
            if (RegisteredModelTypeField is not null)
                _registeredModelNames = RegisteredModelTypeField.GenericTypeArguments;
            return _registeredModelNames;
        }
    }

    /// <summary>
    /// Gets the registered model type field.
    /// </summary>
    public TypeField? RegisteredModelTypeField
    {
        get
        {
            if (_registeredModelTypeField is not null) return _registeredModelTypeField;
            _registeredModelTypeField = Fields.FirstOrDefault(f => f?.IdentifierName == "_registerModel");
            return _registeredModelTypeField;
        }
    }

    /// <summary>
    /// Reports the diagnostics.
    /// </summary>
    public override void ReportDiagnostics(SMFProductionContext context)
    {
        base.ReportDiagnostics(context);
        RDIfRegisteredModelFieldHasInvalidTypeArguments(context);
    }

    /// <summary>
    /// Reports the diagnostic if exist.
    /// </summary>
    /// <param name="registeredModelName">The registered model name.</param>
    /// <param name="registeredTypeField">The registered type field.</param>
    /// <returns>A bool.</returns>
    private void RDIfRegisteredModelFieldHasInvalidTypeArguments(SMFProductionContext context)
    {
        var parentCDS = RegisteredModelTypeField?.ParentCDS;
        var typeLocations = (RegisteredModelTypeField?.FDS?.DescendantNodes()?.FirstOrDefault(_ => _.IsKind(SyntaxKind.GenericName))! as GenericNameSyntax)?.TypeArgumentList.Arguments;
        if (RegisteredModelNames is null) return;
        foreach (string? registeredModelName in RegisteredModelNames!)
        {
            if (registeredModelName is null) continue;

            var location = typeLocations!.Value.FirstOrDefault(_ => _.ToString() == registeredModelName)?.GetLocation();

            // Report If the registered type is not SMF Model.
            if (!registeredModelName.EndsWith("Model"))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.MustHaveSMFModelType, location,
                    registeredModelName, parentCDS?.Identifier.ValueText));
            }

            // Report If More Than one Models are registered for the same Module Name.
            if (RegisteredModelTypeField?.GenericTypeArguments.Where(s => s == registeredModelName).Count() > 1)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                   DiagnosticDescriptors.ShouldNotBeRegisteredMoreThanOneSMFModelType, location,
                    registeredModelName, parentCDS?.Identifier.ValueText));
            }
        }
    }
}