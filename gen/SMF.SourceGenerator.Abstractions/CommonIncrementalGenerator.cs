namespace SMF.Common.SourceGenerator.Abstractions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;
using SMF.SourceGenerator.Core;
using SMF.SourceGenerator.Core.Helpers;
using SMF.SourceGenerator.Core.Types.TypeMembers;
using System.Collections.Immutable;
using System.Linq;

/// <summary>
/// The addons incremental generator.
/// </summary>

public abstract class CommonIncrementalGenerator : IncrementalGenerator
{
    /// <summary>
    /// Gets or sets the smf c d ss.
    /// </summary>
    protected virtual IncrementalValuesProvider<ClassDeclarationSyntax?> SMFCDSs { get; set; }

    /// <summary>
    /// Gets or sets the module c d ss.
    /// </summary>
    protected virtual IncrementalValuesProvider<ClassDeclarationSyntax?> ModuleCDSs =>
        SMFCDSs.Where(static cds => cds!.IsModuleClass());


    /// <summary>
    /// Gets or sets the model c d ss.
    /// </summary>
    protected virtual IncrementalValuesProvider<ClassDeclarationSyntax?> ModelCDSs =>
        SMFCDSs.Where(static cds => cds!.IsModelClass());

    /// <summary>
    /// Gets or sets the controller c d ss.
    /// </summary>
    protected virtual IncrementalValuesProvider<ClassDeclarationSyntax?> ControllerCDSs =>
        SMFCDSs.Where(static cds => cds!.IsControllerClass());

    /// <summary>
    /// Gets or sets the module c d s collection.
    /// </summary>
    protected virtual IncrementalValueProvider<ImmutableArray<ClassDeclarationSyntax?>> ModuleCDSCollection => ModuleCDSs.Collect();

    /// <summary>
    /// Gets or sets the controller c d s collection.
    /// </summary>
    protected virtual IncrementalValueProvider<ImmutableArray<ClassDeclarationSyntax?>> ControllerCDSCollection => ControllerCDSs.Collect();

    /// <summary>
    /// Gets or sets the model c d s collection.
    /// </summary>
    protected virtual IncrementalValueProvider<ImmutableArray<ClassDeclarationSyntax?>> ModelCDSCollection => ModelCDSs.Collect();

    /// <summary>
    /// Gets or sets the module c ts.
    /// </summary>
    protected virtual IncrementalValuesProvider<ModuleCT> ModuleCTs { get; private set; }
    /// <summary>
    /// Gets or sets the model c ts.
    /// </summary>
    protected virtual IncrementalValuesProvider<ModelCT> ModelCTs { get; private set; }

    /// <summary>
    /// Gets or sets the controller c ts.
    /// </summary>
    protected virtual IncrementalValuesProvider<ControllerCT> ControllerCTs { get; private set; }


    /// <summary>
    /// Gets or sets the module c t collection.
    /// </summary>
    protected virtual IncrementalValueProvider<ImmutableArray<ModuleCT>> ModuleCTCollection => ModuleCTs.Collect();

    /// <summary>
    /// Gets or sets the model c t collection.
    /// </summary>
    protected virtual IncrementalValueProvider<ImmutableArray<ModelCT>> ModelCTCollection => ModelCTs.Collect();

    /// <summary>
    /// Gets or sets the controller c t collection.
    /// </summary>
    protected virtual IncrementalValueProvider<ImmutableArray<ControllerCT>> ControllerCTCollection => ControllerCTs.Collect();

    /// <summary>
    /// Gets or sets the class types.
    /// </summary>
    protected virtual IncrementalValueProvider<(IEnumerable<ModuleCT> moduleCTs, IEnumerable<ModelCT> modelCTs)> ModuleCTsWithModelCTs =>
        ModuleCTCollection
             .Combine(ModelCTCollection)
             .Select(static (r, _) => ((IEnumerable<ModuleCT>)r.Left, (IEnumerable<ModelCT>)r.Right));

    /// <summary>
    /// Gets the module c ts with model c ts and controller c ts.
    /// </summary>
    protected virtual IncrementalValueProvider<(IEnumerable<ModuleCT> moduleCTs, IEnumerable<ModelCT> modelCTs, IEnumerable<ControllerCT> controllerCTs)> ModuleCTsWithModelCTsAndControllerCTs =>
    ModuleCTCollection
         .Combine(ModelCTCollection)
         .Combine(ControllerCTCollection)
         .Select(static (r, _) => ((IEnumerable<ModuleCT>)r.Left.Left, (IEnumerable<ModelCT>)r.Left.Right, (IEnumerable<ControllerCT>)r.Right));

    /// <summary>
    /// Gets or sets the config s m f.
    /// </summary>
    public IncrementalValueProvider<ConfigSMF?> ConfigSMF { get; set; }

    /// <summary>
    /// Gets the global usings.
    /// </summary>
    public IncrementalValueProvider<string> GlobalUsings => ModuleCTsWithModelCTsAndControllerCTs.Select(static (s, _) =>
        {
            List<string> globalNamespaces = new()
        {
            "SMF.ORM.Models",
            "SMF.Addons",
            "SMFields = SMF.ORM.Fields",
        };
            s.moduleCTs?.ForEach(_ => globalNamespaces.Add(_.ContainingNamespace));
            s.modelCTs?.ForEach(_ => globalNamespaces.Add(_.ContainingNamespace));
            s.controllerCTs?.ForEach(_ => globalNamespaces.Add(_.ContainingNamespace));

            var distinctGlobalNamespaces = globalNamespaces.Distinct();
            return string.Join("\n", distinctGlobalNamespaces.Select(_ => "global using " + _ + ";"));
        });

    /// <summary>
    /// Gets the registered models.
    /// </summary>
    public IncrementalValuesProvider<ModuleWithRegisteredModelCTs> ModuleWithRegisteredModelCTs => ModuleCTsWithModelCTs.SelectMany(static (s, _) =>
    {
        List<ModuleWithRegisteredModelCTs> registeredModels = new();
        foreach (var moduleCT in s.moduleCTs)
        {
            registeredModels.Add(new ModuleWithRegisteredModelCTs(moduleCT, moduleCT.GetRegisteredModelCTs((s.moduleCTs, s.modelCTs))));
        }
        return registeredModels.AsEnumerable();
    });

    /// <summary>
    /// Gets the registered modules c ts.
    /// </summary>                                        
    public IncrementalValuesProvider<ModelCT> RegisteredModelCTs => ModuleWithRegisteredModelCTs.SelectMany(static (s, _) =>
    {

        List<ModelCT> modelCTs = new();
        if (s.RegisteredModelCTs?.Count() == 0) return modelCTs;

        foreach (var registeredModelCT in s.RegisteredModelCTs!)
            modelCTs.Add(registeredModelCT!);

        foreach (var modelCT in modelCTs)
        {
            foreach (var property in modelCT.Properties.Where(_ => _!.Type is "SMFields.O2O" or "SMFields.O2M" or "SMFields.M2O" or "SMFields.M2M"))
            {
                var relationalModelName = (property!.PDS.DescendantNodes().OfType<ImplicitObjectCreationExpressionSyntax>().FirstOrDefault().ArgumentList.Arguments[0].Expression as MemberAccessExpressionSyntax)?.Name.Identifier.ValueText;
                var relationModelWithModuleName = relationalModelName?.Split('_');
                var relationModelQualifiedName = $"{modelCT.ConfigSMFAndGlobalOptions.RootNamespace}.{relationModelWithModuleName?[0]}Addon.Models.{relationModelWithModuleName?[1]}";
                var relationalModelCT = modelCTs.FirstOrDefault(_ => _.QualifiedName == relationModelQualifiedName);
                var forignkey = new TypeProperty(ClassType.CreateProperty("int", relationalModelName!.Substring(0, relationalModelName.Length - "Model".Length)), relationalModelCT!);
                var relationProperty = new TypeProperty(ClassType.CreateProperty(property.ClassType.NewQualifiedName!, (property.ClassType as ModelCT)!.IdentifierNameWithoutPostFix!), relationalModelCT);
                relationalModelCT.Properties.Add(forignkey);
                relationalModelCT.Properties.Add(relationProperty);
                property.SetRelationshipWith(new RelationshipWith(property, relationProperty, forignkey, RelationshipType.O2M));
            }
        }
        return modelCTs;
    }
    );

    /// <summary>
    /// Gets or sets the config s m f and global options.
    /// </summary>
    public IncrementalValueProvider<ConfigSMFAndGlobalOptions> ConfigSMFAndGlobalOptions { get; set; }

    /// <summary>
    /// Analyzers the config options provider.
    /// </summary>
    /// <param name="analyzerConfigOptionsProvider">The analyzer config options provider.</param>
    protected override void AnalyzerConfigOptionsProvider(IncrementalValueProvider<AnalyzerConfigOptionsProvider> analyzerConfigOptionsProvider)
    {
        ConfigSMFAndGlobalOptions = ConfigSMF.Combine(analyzerConfigOptionsProvider).Select(static (s, a) => new ConfigSMFAndGlobalOptions(s.Left, s.Right));

        ModuleCTs =
        ModuleCDSCollection
            .SelectMany(static (r, _) => r.Distinct(new CDSComparer()))
            .Combine(ModuleCDSCollection).Combine(ConfigSMFAndGlobalOptions)
            .Select(static (r, _) => new ModuleCT(r.Left.Right.GetAllPartialClasses(r.Left.Left)!, r.Right, _));


        ModelCTs = ModelCDSCollection
            .SelectMany(static (r, _) => r.Distinct(new CDSComparer()))
            .Combine(ModelCDSCollection).Combine(ConfigSMFAndGlobalOptions)
            .Select(static (r, _) => new ModelCT(r.Left.Right.GetAllPartialClasses(r.Left.Left)!, r.Right, _));



        ControllerCTs = ControllerCDSCollection
           .SelectMany(static (r, _) => r.Distinct(new CDSComparer()))
           .Combine(ControllerCDSCollection).Combine(ConfigSMFAndGlobalOptions)
           .Select(static (r, _) => new ControllerCT(r.Left.Right.GetAllPartialClasses(r.Left.Left)!, r.Right, _));


        ModelCTs = ModelCTs.Combine(ModelCTCollection).Select(static (r, _) =>
        {
            r.Left.SetParentClassType(r.Right.FirstOrDefault(_ => _.QualifiedName == r.Left.QualifiedParentName)!);
            return r.Left;
        });
    }

    /// <summary>
    /// Additionals the texts provider.
    /// </summary>
    /// <param name="additionalTextsProvider">The additional texts provider.</param>
    protected override void AdditionalTextsProvider(IncrementalValuesProvider<AdditionalText> additionalTextsProvider)
    {
        ConfigSMF = additionalTextsProvider
     .Where(static file => file.Path.EndsWith("config.smf")).Collect()
     .Select(static (r, _) =>
     {
         var configFile = r.FirstOrDefault()?.GetText()?.ToString()!;
         if (configFile is null) return null;
         return new ConfigSMF(configFile);
     })!;
    }


    /// <summary>
    /// Creates the syntax node.                               
    /// </summary>
    /// <param name="syntaxProvider">The syntax provider.</param>
    protected override void CreateSyntaxNode(SyntaxValueProvider syntaxProvider)
    {
        SMFCDSs = syntaxProvider
            .CreateSyntaxProvider(
           static (n, _) => n.IsSMFClass(),
           static (r, _) => r.Node as ClassDeclarationSyntax)
            .Where(static n => n is not null);
    }
}
