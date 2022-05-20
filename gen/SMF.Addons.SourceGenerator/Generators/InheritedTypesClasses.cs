namespace SMF.Addons.SourceGenerator.Generators;

/// <summary>
/// The static classes.
/// </summary>

[Generator]
internal class InheritedTypesClasses : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        //base.Execute(context);
        //context.RegisterImplementationSourceOutput(ClassTypes, AddModuleNamesStaticClasses);
        context.RegisterImplementationSourceOutput(ModuleCTsWithModelCTs, AddModelStaticClasses);
    }

    /// <summary>
    /// Adds the model static classes.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddModelStaticClasses(SourceProductionContext c, (IEnumerable<ModuleCT> moduleCTs, IEnumerable<ModelCT> modelCTs) s)
    {
        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate namespaceTemplate = new("SMF.Addons");
        ClassTypeTemplate staticClassTemplate = new("InheritModels")
        {
            Modifiers = "public static partial",
            Comment = "All Registered Models."
        };
        foreach (var moduleCT in s.moduleCTs!)
        {
            var iii = moduleCT.InheritTypes;
            var modelName = moduleCT.IdentifierName.Substring(0, moduleCT.IdentifierName.Length - "Module".Length);
            foreach (var registeredModel in moduleCT?.GetRegisteredModelCTs(s)!)
            {
                var comment = registeredModel!.Comment == registeredModel!.IdentifierName ? registeredModel!.IdentifierName + " is registered in " + modelName + " Module." : registeredModel.Comment;
                AutoPropertyTemplate staticModuleNameProperty = new("InheritModel", modelName + "_" + registeredModel!.IdentifierName)
                {
                    Modifiers = "public static",
                    Comment = comment,
                    DefaultValue = $"new(\"{registeredModel.IdentifierName}\",\"{registeredModel.ContainingNamespace}\")"
                };
                staticClassTemplate.Members.Add(staticModuleNameProperty);
            }
        }
        namespaceTemplate.TypeTemplates.Add(staticClassTemplate);
        context.AddSource(namespaceTemplate);
    }
}
//#if DEBUG
//        if (!System.Diagnostics.Debugger.IsAttached)
//            System.Diagnostics.Debugger.Launch();
//#endif