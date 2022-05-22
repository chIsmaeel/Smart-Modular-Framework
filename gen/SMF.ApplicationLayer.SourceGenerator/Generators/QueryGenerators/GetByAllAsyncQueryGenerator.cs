namespace Application.Queries;

using Humanizer;
using SMF.ApplicationLayer.SourceGenerator;
using SMF.SourceGenerator.Core.Types.TypeMembers;
using System.CodeDom.Compiler;

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

        ClassTypeTemplate handlerClass = new(classTypeTemplate.IdentifierName + "Handler")
        {

            Interfaces = new()
            {
               $"MediatR.IRequestHandler<{classTypeTemplate.IdentifierName},IEnumerable<{s.NewQualifiedName}>>"
            }
        };

        handlerClass.Members.Add(new TypeFieldTemplate(s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Infrastructure.UnitOfWork", "_uow")
        {

            Modifiers = "private readonly"
        });

        handlerClass.Members.Add(new ConstructorTemplate(handlerClass.IdentifierName)
        {

            Parameters = new() { (s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Infrastructure.UnitOfWork", "uow") },
            Body = (w, _) => { w.WriteLine("_uow = uow;"); }

        });

        var tempModelCTForMethods = s;
        while (tempModelCTForMethods is not null)
        {
            classTypeTemplate.UsingNamespaces.AddRange(tempModelCTForMethods.Usings.Where(_ => _.StartsWith(tempModelCTForMethods.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME)));
            StaticMethods.AddModelMethods(tempModelCTForMethods, handlerClass);
            tempModelCTForMethods = tempModelCTForMethods.ParentClassType as ModelCT;
        }

        handlerClass.Members.Add(new TypeMethodTemplate($"Task<IEnumerable<{s.NewQualifiedName}>?>", "Handle")
        {

            Modifiers = "public async",
            Parameters = new() { (classTypeTemplate.IdentifierName, "query"), ("System.Threading.CancellationToken", "cancellationToken") },
            Body = (w, p, gp, _) =>
            {
                w.WriteLine($"var response = await _uow.{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Repository.GetAllAsync();");
                w.WriteLine($"if(response is null) return null;");
                var hasComputedValue = false;
                var tempModelCTForComputedValues = s;
                while (tempModelCTForComputedValues is not null)
                {
                    if (tempModelCTForComputedValues.Properties.Any(property => property!.SMField!.Field is not null && property.SMField.Field.Compute))
                        hasComputedValue = true;
                    tempModelCTForComputedValues = tempModelCTForComputedValues.ParentClassType as ModelCT;
                }

                if (hasComputedValue)
                {
                    w.WriteLine($"foreach (var entity in response)");
                    w.WriteLine("{");
                    tempModelCTForComputedValues = s;
                    while (tempModelCTForComputedValues is not null)
                    {
                        AssignComputedProperties(w, tempModelCTForComputedValues.Properties.Where(_ => _!.SMField is not null)!);
                        tempModelCTForComputedValues = tempModelCTForComputedValues.ParentClassType as ModelCT;
                    }
                    w.WriteLine("}");
                }
                w.WriteLine("return await Task.FromResult(response);");
            }
        });
        classTypeTemplate.Members.Add(handlerClass);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource($"GetAll{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix.Pluralize()}Query", fileScopedNamespace.CreateTemplate().GetTemplate());
    }

    /// <summary>
    /// Assigns the computed properties.
    /// </summary>
    /// <param name="w">The w.</param>
    /// <param name="smFields">The sm fields.</param>
    private static void AssignComputedProperties(IndentedTextWriter w, IEnumerable<TypeProperty> smFields)
    {
        foreach (var property in smFields)
            if (property!.SMField!.Field is not null && property.SMField.Field.Compute)
            {
                w.WriteLine($"entity.{property!.IdentifierName} = Compute{property.IdentifierName}(_uow,entity);");
                continue;
            }
    }
}
