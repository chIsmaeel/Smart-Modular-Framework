namespace Application.Commands;

using SMF.ApplicationLayer.SourceGenerator;
using SMF.SourceGenerator.Core;

/// <summary>
/// The model entity configuration generator.
/// </summary>

[Generator]

internal class CreateAsyncCommands : CommonIncrementalGenerator
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
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //            System.Diagnostics.Debugger.Launch();
        //#endif
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
        while (tempModelCT is not null)
        {
            StaticMethods.AddProperties(tempModelCT, classTypeTemplate);
            tempModelCT = tempModelCT.ParentClassType as ModelCT;
        }
        ClassTypeTemplate handlerClass = new(classTypeTemplate.IdentifierName + "Handler")
        {

            Interfaces = new()
            {
               $"MediatR.IRequestHandler<{classTypeTemplate.IdentifierName},int>"
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

        //        if (s.IdentifierName == "PurchaseLineModel")
        //        {
        //#if DEBUG
        //            if (!System.Diagnostics.Debugger.IsAttached)
        //                System.Diagnostics.Debugger.Launch();
        //#endif
        //        }
        handlerClass.Members.Add(new TypeMethodTemplate($"Task<int>", "Handle")
        {

            Modifiers = "public async",
            Parameters = new() { (classTypeTemplate.IdentifierName, "command"), ("System.Threading.CancellationToken", "cancellationToken") },
            Body = (w, p, gp, _) =>
            {
                var objName = s.IdentifierNameWithoutPostFix.FirstCharToLowerCase();
                w.WriteLine($"var {objName} = new {s.NewQualifiedName}(); ");
                w.WriteLine($"{objName}.CreatedOn = System.DateTime.Now;");
                var tempModelCT = s;
                while (tempModelCT is not null)
                {
                    StaticMethods.AddProperties(tempModelCT, w, objName);
                    tempModelCT = tempModelCT.ParentClassType as ModelCT;
                }
                w.WriteLine($"await  _uow.{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Repository.InsertAsync({objName});");
                w.WriteLine($"return await Task.FromResult({objName}.Id);");
            }
        });
        classTypeTemplate.Members.Add(handlerClass);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource($"Create{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Command", fileScopedNamespace.CreateTemplate().GetTemplate());
    }

}
