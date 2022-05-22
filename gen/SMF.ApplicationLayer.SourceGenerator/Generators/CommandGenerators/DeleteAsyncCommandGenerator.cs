namespace Application.Commands;

using SMF.SourceGenerator.Core;

/// <summary>
/// The model entity configuration generator.
/// </summary>

[Generator]

internal class DeleteAsyncCommands : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(RegisteredModelCTs, AddModelEntityConfiguration);
    }

    /// <summary>
    /// Adds the model entity configuration.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s."OrderEntityTypeConfiguration "</param>                                                                                                    
    private void AddModelEntityConfiguration(SourceProductionContext c, ModelCT s)
    {
        if (s.ConfigSMFAndGlobalOptions.RootNamespace is null) return;
        if (s.ContainingModuleName is null) return;
        //Debugger.Launch();

        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate fileScopedNamespace = new($"{s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Application.{s.ContainingModuleName}.Commands");
        ClassTypeTemplate classTypeTemplate = new($"Delete{s.IdentifierNameWithoutPostFix}Command")
        {
            Modifiers = "public partial",
            Interfaces = new() { $"MediatR.IRequest<int>" },
        };

        classTypeTemplate.Members.Add(new AutoPropertyTemplate("int", "Id"));

        ClassTypeTemplate handlerClass = new(classTypeTemplate.IdentifierName + "Handler")
        {

            Interfaces = new()
            {
               $"MediatR.IRequestHandler<{classTypeTemplate.IdentifierName},int>"
            }
        };


        handlerClass.Members.Add(new TypeFieldTemplate(s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Application.Interfaces.IUnitOfWork", "_uow")
        {

            Modifiers = "private readonly"
        });

        handlerClass.Members.Add(new ConstructorTemplate(handlerClass.IdentifierName)
        {

            Parameters = new() { (s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Application.Interfaces.IUnitOfWork", "uow") },
            Body = (w, _) => { w.WriteLine("_uow = uow;"); }

        });

        handlerClass.Members.Add(new TypeMethodTemplate($"Task<int>", "Handle")
        {

            Modifiers = "public async",
            Parameters = new() { (classTypeTemplate.IdentifierName, "command"), ("System.Threading.CancellationToken", "cancellationToken") },
            Body = (w, p, gp, _) =>
            {
                var objName = s.IdentifierNameWithoutPostFix.FirstCharToLowerCase();
                w.WriteLine($" await _uow.{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Repository.DeleteAsync(command.Id);");
                w.WriteLine($"return await Task.FromResult(command.Id);");
            }
        });
        classTypeTemplate.Members.Add(handlerClass);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource($"Delete{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Command", fileScopedNamespace.CreateTemplate().GetTemplate());
    }
}
