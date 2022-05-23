namespace Infrastructure;

using Microsoft.CodeAnalysis;
using SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;
using System.Collections.Immutable;
/// <summary>
///         UnitOfWorkGenerator.
/// </summary>

[Generator]
internal class UnitOfWork : CommonIncrementalGenerator
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
        var configSMF = s.FirstOrDefault()?.ConfigSMFAndGlobalOptions.ConfigSMF;
        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate fileScopedNamespace = new(configSMF!.SOLUTION_NAME! + ".Infrastructure");
        ClassTypeTemplate classTypeTemplate = new("UnitOfWork")
        {
            Interfaces = new() { $"{configSMF.SOLUTION_NAME}.Application.Interfaces.IUnitOfWork" }
        };

        classTypeTemplate.Members.Add(new AutoPropertyTemplate(configSMF.SOLUTION_NAME! + ".Application.Interfaces.ISMFDbContext", "SMFDbContext"));
        classTypeTemplate.Members.Add(new TypeFieldTemplate("bool", "disposed") { DefaultValue = "false" });

        classTypeTemplate.Members.Add(new ConstructorTemplate(classTypeTemplate.IdentifierName)
        {

            Parameters = new() { (configSMF!.SOLUTION_NAME! + ".Application.Interfaces.ISMFDbContext", "context") },
            Body = (writer, parameters) =>
            {
                writer.WriteLine("SMFDbContext = context;");

            }
        });
        foreach (var modelCT in s)
        {
            classTypeTemplate.Members.Add(new FullPropertyTemplate($"{configSMF!.SOLUTION_NAME}.Application.{modelCT.ContainingModuleName}.Repositories.Interfaces." + $"I{modelCT.IdentifierNameWithoutPostFix}Repository", $"{modelCT.ModuleNameWithoutPostFix}_{modelCT.IdentifierNameWithoutPostFix}Repository")
            {
                FirstAccessorBodyAction = (writer, propertyField, otherFields) =>
                {
                    writer.WriteLine($"return {propertyField} = new {configSMF!.SOLUTION_NAME}.Infrastructure." + modelCT.ContainingModuleName + ".Repositories." + $"{modelCT.IdentifierNameWithoutPostFix}Repository(SMFDbContext); ");
                }
            }
            );
        }

        classTypeTemplate.Members.Add(new TypeMethodTemplate("void", "Dispose")
        {
            Modifiers = "protected virtual",
            Parameters = new() { ("bool", "disposing") },
            Body = (writer, parameters, _, fileds) =>
            {
                writer.WriteLine("if (!disposed)");
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine("if (disposing)");
                writer.Indent++;
                writer.WriteLine($"(SMFDbContext as  Microsoft.EntityFrameworkCore.DbContext).Dispose();");
                writer.Indent--;
                writer.WriteLine("disposed = true;");
                writer.Indent--;
                writer.WriteLine("}");
            }
        });

        classTypeTemplate.Members.Add(new TypeMethodTemplate("void", "Dispose")
        {

            Body = (writer, parameters, _, fileds) =>
            {
                writer.WriteLine("Dispose(true);");
                writer.WriteLine("GC.SuppressFinalize(this);");
            }
        });
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource(fileScopedNamespace);
    }
}
