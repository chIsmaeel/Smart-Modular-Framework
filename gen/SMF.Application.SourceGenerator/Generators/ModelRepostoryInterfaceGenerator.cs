namespace Application.Interfaces;

using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.InterfaceMemberTemplate;

/// <summary>
///          ModelRepostoryInterfaceGenerator
/// </summary>
[Generator]
internal class Repositories : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(ConfigSMFAndGlobalOptions, AddIRepositoryInterface);
        context.RegisterSourceOutput(RegisteredModelCTs, AddModelRepositoiesInterface);
    }

    /// <summary>
    /// Adds the i repository interface.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddIRepositoryInterface(SourceProductionContext c, ConfigSMFAndGlobalOptions s)
    {
        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate fileScopedNamespace = new(s.ConfigSMF!.SOLUTION_NAME + ".Application.Interfaces");
        InterfaceTemplate interfaceTemplate = new("IRepository")
        {
            Modifiers = "public partial",
            GenericParameters = new() { "T" }
        };
        //Debugger.Launch();

        interfaceTemplate.Members.Add(new MethodInterfaceTemplate($"Task<IEnumerable<{interfaceTemplate.GenericParameters.FirstOrDefault()}>>", "GetAllAsync")
        {
            Parameters = new() { new("Func<T, bool>", "where = null") }
        });
        interfaceTemplate.Members.Add(new MethodInterfaceTemplate($"Task<{interfaceTemplate.GenericParameters.FirstOrDefault()}>", "GetByIdAsync") { Parameters = new() { ("int", "id") } });
        interfaceTemplate.Members.Add(new MethodInterfaceTemplate($"Task", "AddAsync") { Parameters = new() { (interfaceTemplate.GenericParameters.FirstOrDefault(), "entity") } });
        interfaceTemplate.Members.Add(new MethodInterfaceTemplate($"Task", "UpdateAsync") { Parameters = new() { (interfaceTemplate.GenericParameters.FirstOrDefault(), "entity") } });
        interfaceTemplate.Members.Add(new MethodInterfaceTemplate($"Task", "DeleteAsync") { Parameters = new() { ("int", "id") } });


        fileScopedNamespace.TypeTemplates.Add(interfaceTemplate);
        context.AddSource("IRepository", fileScopedNamespace.CreateTemplate().GetTemplate());
    }

    /// <summary>
    /// Adds the model repositoies.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddModelRepositoiesInterface(SourceProductionContext c, ModelCT s)
    {
        SMFProductionContext context = new(c);
        if (s.ContainingModuleName is null) return;
        FileScopedNamespaceTemplate fileScopedNamespace = new(s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME + ".Application." + s.ContainingModuleName + ".Repositories.Interfaces");
        InterfaceTemplate interfaceTemplate = new($"I{s.IdentifierNameWithoutPostFix}Repository")
        {
            Interfaces = new() { $"{s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Application.Interfaces.IRepository<{s.NewQualifiedName}>" },
        };
        fileScopedNamespace.TypeTemplates.Add(interfaceTemplate);
        context.AddSource($"I{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}", fileScopedNamespace.CreateTemplate().GetTemplate());
    }
}
