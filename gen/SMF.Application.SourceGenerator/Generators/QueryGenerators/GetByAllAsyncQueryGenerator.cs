namespace Application.Queries;

using Humanizer;

/// <summary>
/// The model entity configuration generator.
/// </summary>

[Generator]

internal class GetByAllAsyncQueries : CommonIncrementalGenerator
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

        classTypeTemplate.Members.Add(new AutoPropertyTemplate($"Func<{s.NewQualifiedName}, bool>", "Where"));

        classTypeTemplate.Members.Add(new ConstructorTemplate(classTypeTemplate.IdentifierName)
        {
            Parameters = new(){
                new($"Func<{s.NewQualifiedName}, bool>", "where = null")
            },
            Body = (w, _) => { w.WriteLine("Where= where;"); }
        });

        ClassTypeTemplate handlerClass = new(classTypeTemplate.IdentifierName + "Handler")
        {

            Interfaces = new()
            {
               $"MediatR.IRequestHandler<{classTypeTemplate.IdentifierName},IEnumerable<{s.NewQualifiedName}>>"
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

        handlerClass.Members.Add(new TypeMethodTemplate($"Task<IEnumerable<{s.NewQualifiedName}>?>", "Handle")
        {

            Modifiers = "public async",
            Parameters = new() { (classTypeTemplate.IdentifierName, "query"), ("System.Threading.CancellationToken", "cancellationToken") },
            Body = (w, p, gp, _) =>
            {
                w.WriteLine($"var response = await _uow.{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Repository.GetAllAsync(query.Where);");
                w.WriteLine($"if(response is null) return null;");
                w.WriteLine("return await Task.FromResult(response);");
            }
        });
        classTypeTemplate.Members.Add(handlerClass);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource($"GetAll{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix.Pluralize()}Query", fileScopedNamespace.CreateTemplate().GetTemplate());
    }
}
