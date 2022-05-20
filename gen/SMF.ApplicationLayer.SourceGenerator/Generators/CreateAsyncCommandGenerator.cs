namespace SMF.EntityFramework.SourceGenerator.Generators;

using SMF.SourceGenerator.Core;
using System.CodeDom.Compiler;

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
            Interfaces = new() { $"MediatR.IRequest<IEnumerable<{s.NewQualifiedName}>>" },
        };

        foreach (var property in s.Properties!)
        {
            AutoPropertyTemplate p;
            if (property.IdentifierName is "Id")
                continue;
            p = new(ModelPropertyTypes.GetPropertyType(property.Type), property.IdentifierName)
            {
                Comment = property.Comment,
                SecondAccessor = "set"
            };
            classTypeTemplate.Members.Add(p);
        }
        if (s.StringParentType == "ModelBase")
            AddDefaultProperties(classTypeTemplate);
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
                AddProperties(s, w, objName);

                while (tempModelCT.ParentClassType is not null)
                {
                    AddProperties((ModelCT)tempModelCT.ParentClassType, w, objName);
                    tempModelCT = (ModelCT)tempModelCT.ParentClassType;
                }
                w.WriteLine($"_uow.{s.IdentifierNameWithoutPostFix}Repository.InsertAsync({objName});");
                w.WriteLine($"return {objName}.Id;");
            }
        });
        classTypeTemplate.Members.Add(handlerClass);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource(fileScopedNamespace);
    }

    private static void AddProperties(ModelCT s, IndentedTextWriter w, string? objName)
    {
        foreach (var property in s.Properties!)
        {
            if (property.IdentifierName is "Id" or "UpdatedOn")
                continue;
            if (property.IdentifierName is "CreatedOn")
            {
                w.WriteLine($"{objName}.{property.IdentifierName} = DateTime.Now;");
                continue;
            }
            w.WriteLine($"{objName}.{property.IdentifierName} = command.{property.IdentifierName};");
        }
    }


    /// <summary>
    /// Adds the default properties.
    /// </summary>
    /// <param name="classTypeTemplate">The class type template.</param>
    /// <param name="s">The s.</param>
    private void AddDefaultProperties(ClassTypeTemplate classTypeTemplate)
    {
        classTypeTemplate.Members.Add(new AutoPropertyTemplate("int", "Id"));
        classTypeTemplate.Members.Add(new AutoPropertyTemplate("DateTime", "CreatedOn"));
        classTypeTemplate.Members.Add(new AutoPropertyTemplate("DateTime", "LastModifiedOn"));
    }
}
