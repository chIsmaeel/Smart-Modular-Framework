namespace SMF.EntityFramework.SourceGenerator.Generators;

using SMF.ApplicationLayer.SourceGenerator;
using SMF.SourceGenerator.Core;

/// <summary>
/// The model entity configuration generator.
/// </summary>

[Generator]

internal class CreateAsyncCommandGenerator : CommonIncrementalGenerator
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
        ClassTypeTemplate classTypeTemplate = new($"Create{s.IdentifierNameWithoutPostFix}Command")
        {
            Modifiers = "public partial",
            Interfaces = new() { $"MediatR.IRequest<int>" },
        };

        var tempModelCT = s;
        while (tempModelCT.ParentClassType is not null)
        {
            StaticMethods.AddProperties((ModelCT)tempModelCT.ParentClassType, classTypeTemplate);
            tempModelCT = (ModelCT)tempModelCT.ParentClassType;
        }
        StaticMethods.AddProperties(s, classTypeTemplate);
        ClassTypeTemplate handlerClass = new(classTypeTemplate.IdentifierName + "Handler")
        {
            IsSubMemberofOtherType = true,
            Interfaces = new()
            {
               $"MediatR.IRequestHandler<{classTypeTemplate.IdentifierName},int>"
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

        handlerClass.Members.Add(new TypeMethodTemplate($"Task<int>", "Handle")
        {
            IsSubMemberofOtherType = true,
            Modifiers = "public async",
            Parameters = new() { (classTypeTemplate.IdentifierName, "command"), ("System.Threading.CancellationToken", "cancellationToken") },
            Body = (w, p, gp, _) =>
            {
                var objName = s.IdentifierNameWithoutPostFix.FirstCharToLowerCase();
                w.WriteLine($"var {objName} = new {s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Domain.{s.ContainingModuleName}.Models.{s.IdentifierNameWithoutPostFix}(); ");
                var tempModelCT = s;
                w.WriteLine($"{objName}.CreatedOn = System.DateTime.Now;");
                while (tempModelCT.ParentClassType is not null)
                {
                    StaticMethods.AddProperties((ModelCT)tempModelCT.ParentClassType, w, objName);
                    tempModelCT = (ModelCT)tempModelCT.ParentClassType;
                }

                StaticMethods.AddProperties(s, w, objName);
                w.WriteLine($"await  _uow.{s.IdentifierNameWithoutPostFix}Repository.InsertAsync({objName});");
                w.WriteLine($"return await Task.FromResult({objName}.Id);");
            }
        });
        classTypeTemplate.Members.Add(handlerClass);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource(fileScopedNamespace);
    }
}
