﻿namespace Application.Interfaces;

using Humanizer;
using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.InterfaceMemberTemplate;
using System.Collections.Immutable;

/// <summary>
/// The class that generates the code for the entity framework context.
/// </summary>
[Generator]
internal class ISMFDbContext : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        //Debugger.Launch();
        context.RegisterSourceOutput(RegisteredModelCTs.Collect(), AddDBContext);
    }

    /// <summary>
    /// Adds the d b context.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddDBContext(SourceProductionContext c, ImmutableArray<ModelCT> s)
    {
        var rootNamespace = s.FirstOrDefault()?.ConfigSMFAndGlobalOptions.RootNamespace;
        var configSMF = s.FirstOrDefault()?.ConfigSMFAndGlobalOptions.ConfigSMF;
        if (rootNamespace is null) return;
        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate fileScopedNamespace = new(configSMF!.SOLUTION_NAME! + ".Application.Interfaces");

        InterfaceTemplate interfaceIypeTemplate = new("ISMFDbContext")
        {
            Modifiers = "public partial",
        };

        foreach (var modelCT in s)
            interfaceIypeTemplate.Members.Add(new PropertyInterfaceTemplate($"DbSet<{modelCT.NewQualifiedName}>", $"{modelCT.ModuleNameWithoutPostFix}_{modelCT.IdentifierNameWithoutPostFix.Pluralize()}"));

        interfaceIypeTemplate.UsingNamespaces.AddRange(new[] { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.Metadata.Builders" });
        fileScopedNamespace.TypeTemplates.Add(interfaceIypeTemplate);
        context.AddSource(fileScopedNamespace);
    }
}