namespace Application.Queries;
/// <summary>
/// The model entity configuration generator.
/// </summary>                                       

[Generator]

internal class GetByIdAsyncQueries : CommonIncrementalGenerator
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
        ClassTypeTemplate classTypeTemplate = new($"Get{s.IdentifierNameWithoutPostFix}ByIdQuery")
        {
            Modifiers = "public partial",
            Interfaces = new() { $"MediatR.IRequest<{s.NewQualifiedName}>" },
        };

        classTypeTemplate.Members.Add(new AutoPropertyTemplate("int", "Id") { SecondAccessor = "set" });

        classTypeTemplate.Members.Add(new ConstructorTemplate(classTypeTemplate.IdentifierName)
        {
            Parameters = new() { ("int", "id") },
            Body = (w, _) =>
            {
                w.WriteLine("Id = id;");
            }

        });

        ClassTypeTemplate handlerClass = new(classTypeTemplate.IdentifierName + "Handler")
        {

            Interfaces = new()
            {
               $"MediatR.IRequestHandler<{classTypeTemplate.IdentifierName},{s.NewQualifiedName}>"
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

        handlerClass.Members.Add(new TypeMethodTemplate($"Task<{s.NewQualifiedName}?>", "Handle")
        {

            Modifiers = "public async",
            Parameters = new() { (classTypeTemplate.IdentifierName, "query"), ("System.Threading.CancellationToken", "cancellationToken") },
            Body = (w, p, gp, _) =>
            {
                w.WriteLine($"var response = await _uow.{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Repository.GetByIdAsync(query.Id);");
                w.WriteLine($"if(response is null) return null;");

                w.WriteLine("return await Task.FromResult(response);");
            }
        });
        classTypeTemplate.Members.Add(handlerClass);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource($"Get{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}ByIdQuery", fileScopedNamespace.CreateTemplate().GetTemplate());
    }


}
