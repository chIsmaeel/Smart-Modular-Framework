﻿namespace Infrastructure.Data;

using Humanizer;
using SMF.SourceGenerator.Abstractions;
using System.Collections.Immutable;

/// <summary>
/// The class that generates the code for the entity framework context.
/// </summary>
[Generator]
internal class SMFDbContext : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        //Debugger.Launch();
        context.RegisterSourceOutput(RegisteredModelCTs.Collect(), AddDBContext);
        context.RegisterSourceOutput(ConfigSMFAndGlobalOptions, AddDbContextFactory);

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
        FileScopedNamespaceTemplate fileScopedNamespace = new(configSMF!.SOLUTION_NAME! + ".Infrastructure.Data");

        ClassTypeTemplate classTypeTemplate = new("SMFDbContext")
        {
            Modifiers = "public partial",
            ParentType = "DbContext",
            Interfaces = new() { configSMF!.SOLUTION_NAME! + ".Application.Interfaces.ISMFDbContext" }
        };

        classTypeTemplate.Members.Add(new ConstructorTemplate(classTypeTemplate.IdentifierName)
        {
            Parameters = new() { ($"DbContextOptions<{QualifiedNames.GetSMFDbContext(configSMF)}>", "option")

            },
            BaseConstructorParameters = new[] { "option" },
        });

        foreach (var modelCT in s)
            classTypeTemplate.Members.Add(new AutoPropertyTemplate($"DbSet<{modelCT.NewQualifiedName}>", $"{modelCT.ModuleNameWithoutPostFix}_{modelCT.IdentifierNameWithoutPostFix.Pluralize()}") { SecondAccessor = "set" });

        classTypeTemplate.StringMembers.Add(
$$"""
    /// <summary>
    /// Ons the model creating.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
    }
""");
        classTypeTemplate.UsingNamespaces.AddRange(new[] { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.Metadata.Builders" });
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource(fileScopedNamespace);
    }


    /// <summary>
    /// Adds the db context factory.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private static void AddDbContextFactory(SourceProductionContext c, ConfigSMFAndGlobalOptions s)
    {
        SMFProductionContext context = new(c);
        var syntaxFactory = @$"
// </autogenerated>

// <copyright file=""SMFDbContextFactory.cs"" company=""SMF"">
// Copyright (c) Smart Modular FrameWork. All rights reserved.
// </copyright>
using Microsoft.EntityFrameworkCore;

public class SMFDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<{QualifiedNames.GetSMFDbContext(s.ConfigSMF!)}>
{{
    public {QualifiedNames.GetSMFDbContext(s.ConfigSMF!)} CreateDbContext(string[] args)                                                
    {{
        var optionsBuilder = new DbContextOptionsBuilder<{QualifiedNames.GetSMFDbContext(s.ConfigSMF!)}>();               
        optionsBuilder.UseSqlServer(@""Data Source = {s.ConfigSMF!.DB_DATA_SOURCE}; Initial Catalog = {s.ConfigSMF!.DB_NAME}; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False"");

        return new {QualifiedNames.GetSMFDbContext(s.ConfigSMF!)}(optionsBuilder.Options);
    }}
}} ";

        context.AddSource("SMFDbContextFactory", syntaxFactory);
    }
}
