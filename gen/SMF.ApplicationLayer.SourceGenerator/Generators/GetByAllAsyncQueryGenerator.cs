namespace SMF.EntityFramework.SourceGenerator.Generators;

using Humanizer;

/// <summary>
/// The model entity configuration generator.
/// </summary>

[Generator]

internal class GetByAllAsyncQueryGenerator : CommonIncrementalGenerator
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
        FileScopedNamespaceTemplate fileScopedNamespace = new($"{s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Application.{s.ContainingModuleName}.Queries");
        ClassTypeTemplate classTypeTemplate = new($"GetAll{s.IdentifierNameWithoutPostFix.Pluralize()}Query")
        {
            Modifiers = "public partial",
            Interfaces = new() { $"MediatR.IRequest<IEnumerable<{s.NewQualifiedName}>>" },
        };

        ClassTypeTemplate handlerClass = new(classTypeTemplate.IdentifierName + "Handler")
        {
            IsSubMemberofOtherType = true,
            Interfaces = new()
            {
               $"MediatR.IRequestHandler<{classTypeTemplate.IdentifierName},IEnumerable<{s.NewQualifiedName}>>"
            }
        };

        handlerClass.Members.Add(new TypeFieldTemplate(s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Domain.UnitOfWork", "_uow")
        {
            IsSubMemberofOtherType = true,
            Modifiers = "private readonly"
        });

        handlerClass.Members.Add(new ConstructorTemplate(handlerClass.IdentifierName)
        {
            IsSubMemberofOtherType = true,
            Parameters = new() { (s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Domain.UnitOfWork", "uow") },
            Body = (w, _) => { w.WriteLine("_uow = uow;"); }

        });

        handlerClass.Members.Add(new TypeMethodTemplate($"Task<IEnumerable<{s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Domain.{s.ContainingModuleName}.Models.{s.IdentifierNameWithoutPostFix}>>", "Handle")
        {
            IsSubMemberofOtherType = true,
            Modifiers = "public async",
            Parameters = new() { (classTypeTemplate.IdentifierName, "query"), ("System.Threading.CancellationToken", "cancellationToken") },
            Body = (w, p, gp, _) =>
            {
                w.WriteLine($"var response = await _uow.{s.IdentifierNameWithoutPostFix}Repository.GetAllAsync();");
                w.WriteLine($"if(response is null) return null;");
                w.WriteLine("return response;");
            }
        });
        classTypeTemplate.Members.Add(handlerClass);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource(fileScopedNamespace);
    }

}
