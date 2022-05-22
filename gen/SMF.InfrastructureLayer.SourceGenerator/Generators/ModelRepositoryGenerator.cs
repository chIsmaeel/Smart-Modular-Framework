namespace Infrastructure.Repositories;

using Humanizer;

/// <summary>
/// This class is responsible to generate the source code for the repositories.
/// </summary>
[Generator]
internal class RepositorGenerator : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(RegisteredModelCTs, AddModelRepositoies);
    }

    /// <summary>
    /// Adds the model repositoies.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddModelRepositoies(SourceProductionContext c, ModelCT s)
    {

        SMFProductionContext context = new(c);
        if (s.ContainingModuleName is null) return;
        FileScopedNamespaceTemplate fileScopedNamespace = new(s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME + ".Infrastructure." + s.ContainingModuleName + ".Repositories");
        ClassTypeTemplate classTypeTemplate = new($"{s.IdentifierNameWithoutPostFix}Repository")
        {
            Interfaces = new() { s.ConfigSMFAndGlobalOptions.ConfigSMF.SOLUTION_NAME + ".Application." + s.ContainingModuleName + ".Repositories.Interfaces.I" + s.IdentifierNameWithoutPostFix + "Repository" },
        };
        AddConstructor(s, classTypeTemplate);
        AddRepositoryMethods(s, classTypeTemplate);

        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource($"{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Repository", fileScopedNamespace.CreateTemplate().GetTemplate());
    }

    /// <summary>
    /// Adds the constructor.
    /// </summary>
    /// <param name="classTypeTemplate">The class type template.</param>
    private void AddConstructor(ModelCT s, ClassTypeTemplate classTypeTemplate)
    {
        classTypeTemplate.Members.Add(new TypeFieldTemplate(s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Infrastructure.Data.SMFDbContext", "_context"));
        classTypeTemplate.Members.Add(new ConstructorTemplate(classTypeTemplate.IdentifierName)
        {
            Parameters = new() { (s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Infrastructure.Data.SMFDbContext", "context") },
            Body = (writer, parameters) => { writer.WriteLine("_context = context;"); },
        });

    }

    /// <summary>
    /// Adds the repository methods.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="classTypeTemplate">The class type template.</param>
    private static void AddRepositoryMethods(ModelCT s, ClassTypeTemplate classTypeTemplate)
    {
        classTypeTemplate.Members.Add(new TypeMethodTemplate($"Task<IEnumerable<{s.NewQualifiedName}>>", "GetAllAsync")
        {
            Modifiers = "public async",
            //UsingNamespaces = new() { "Microsoft.EntityFrameworkCore.ChangeTracking" },
            Body = (_writer, parameters, genericParamerters, privateFields) =>
            {
                _writer.WriteLine("return await Task.FromResult(_context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + ".ToList()" + s.OrderByString + ");");
            }
        }); ;

        classTypeTemplate.Members.Add(new TypeMethodTemplate($"Task<{s.NewQualifiedName}>", "GetByIdAsync")
        {
            Modifiers = "public async",
            Parameters = new() { ("int", "id") },
            Body = (_writer, parameters, genericParamerters, privateFields) =>
            {
                _writer.WriteLine("return await Task.FromResult(_context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + ".Where(x => x.Id == id).FirstOrDefault());");
            }
        });

        classTypeTemplate.Members.Add(new TypeMethodTemplate("Task", "InsertAsync")
        {
            Modifiers = "public async",
            Parameters = new() { ($"{s.NewQualifiedName}", "entity") },
            Body = (_writer, parameters, genericParamerters, privateFields) =>
            {
                _writer.WriteLine("try");
                _writer.WriteLine("{");
                _writer.Indent++;
                _writer.WriteLine("_context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + ".Add(entity);");
                _writer.WriteLine("await _context.SaveChangesAsync();");
                _writer.Indent--;
                _writer.WriteLine("}");
                _writer.WriteLine("catch (Exception ex)");
                _writer.WriteLine("{");
                _writer.Indent++;
                _writer.WriteLine("throw ex;");
                _writer.Indent--;
                _writer.WriteLine("}");
            }
        });

        classTypeTemplate.Members.Add(new TypeMethodTemplate("Task", "UpdateAsync")
        {
            Modifiers = "public async",
            Parameters = new() { ($"{s.NewQualifiedName}", "entity") },
            Body = (_writer, parameters, genericParamerters, privateFields) =>
            {
                _writer.WriteLine("try");
                _writer.WriteLine("{");
                _writer.Indent++;
                _writer.WriteLine("var tempEntity = _context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + ".FirstOrDefault(e=>e.Id==entity.Id);");
                var tempModelCT = s;

                _writer.WriteLine($"tempEntity.LastModifiedOn = entity.LastModifiedOn;");

                while (tempModelCT.ParentClassType is not null)
                {
                    AddProperties((ModelCT)tempModelCT.ParentClassType, _writer);
                    tempModelCT = (ModelCT)tempModelCT.ParentClassType;
                }
                AddProperties(s, _writer);

                //_writer.WriteLine("_context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;");
                _writer.WriteLine("await _context.SaveChangesAsync();");
                _writer.Indent--;
                _writer.WriteLine("}");
                _writer.WriteLine("catch (Exception ex)");
                _writer.WriteLine("{");
                _writer.Indent++;
                _writer.WriteLine("throw ex;");
                _writer.Indent--;
                _writer.WriteLine("}");
            }
        });

        classTypeTemplate.Members.Add(new TypeMethodTemplate("Task", "DeleteAsync")
        {
            Modifiers = "public async",
            Parameters = new() { ("int", "id") },
            Body = (_writer, parameters, genericParamerters, privateFields) =>
            {
                _writer.WriteLine("try");
                _writer.WriteLine("{");
                _writer.Indent++;
                _writer.WriteLine("var tempEntity = await _context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + ".FindAsync(id);");
                _writer.WriteLine("_context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + ".Remove(tempEntity);");
                _writer.WriteLine("await _context.SaveChangesAsync();");
                _writer.Indent--;
                _writer.WriteLine("}");
                _writer.WriteLine("catch (Exception ex)");
                _writer.WriteLine("{");
                _writer.Indent++;
                _writer.WriteLine("throw ex;");
                _writer.Indent--;
                _writer.WriteLine("}");
            }
        });
    }

    /// <summary>
    /// Adds the properties.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="w">The w.</param>
    /// <param name="objName">The obj name.</param>
    private static void AddProperties(ModelCT s, IndentedTextWriter w)
    {
        foreach (var property in s.Properties!)
        {
            if (property!.IdentifierName is "Id" or "CreatedOn" or "UpdatedOn")
                continue;
            w.WriteLine($"tempEntity.{property.IdentifierName} = entity.{property.IdentifierName};");
        }
    }
}
