namespace Application.Interfaces;

using Microsoft.CodeAnalysis;
using SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;
using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.InterfaceMemberTemplate;
using System.Collections.Immutable;
/// <summary>
///         UnitOfWorkGenerator.
/// </summary>

[Generator]
internal class IUnitOfWork : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(RegisteredModelCTs.Collect(), AddUnitOfWork);
    }

    /// <summary>
    /// Adds the unit of work.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddUnitOfWork(SourceProductionContext c, ImmutableArray<ModelCT> s)
    {
        if (s.Length == 0) return;
        var configSMF = s.FirstOrDefault()?.ConfigSMFAndGlobalOptions.ConfigSMF;
        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate fileScopedNamespace = new(configSMF!.SOLUTION_NAME! + ".Application.Interfaces");
        InterfaceTemplate interfaceTypeTemplate = new("IUnitOfWork")
        {
            Modifiers = "public partial",
            Interfaces = new() { "System.IDisposable" }
        };

        interfaceTypeTemplate.Members.Add(new PropertyInterfaceTemplate(configSMF.SOLUTION_NAME! + ".Application.Interfaces.ISMFDbContext", "SMFDbContext"));
        foreach (var modelCT in s)
        {
            interfaceTypeTemplate.Members.Add(new PropertyInterfaceTemplate($"{configSMF!.SOLUTION_NAME}.Application.{modelCT.ContainingModuleName}.Repositories.Interfaces." + $"I{modelCT.IdentifierNameWithoutPostFix}Repository", $"{modelCT.ModuleNameWithoutPostFix}_{modelCT.IdentifierNameWithoutPostFix}Repository"));
        }
        fileScopedNamespace.TypeTemplates.Add(interfaceTypeTemplate);
        context.AddSource(fileScopedNamespace);
    }
}
