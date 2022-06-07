namespace Infrastructure.Repositories;

using Humanizer;
using SMF.InfrastructureLayer.SourceGenerator;
using System.Text;

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
        classTypeTemplate.Members.Add(new TypeFieldTemplate(s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Application.Interfaces.ISMFDbContext", "_context"));
        classTypeTemplate.Members.Add(new ConstructorTemplate(classTypeTemplate.IdentifierName)
        {
            Parameters = new() { (s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME! + ".Application.Interfaces.ISMFDbContext", "context") },
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
            UsingNamespaces = new() { "Microsoft.EntityFrameworkCore" },
            Parameters = new() { ($"Func<{s.NewQualifiedName}, bool>", "where = null") },
            Body = (_writer, parameters, genericParamerters, privateFields) =>
            {
                _writer.WriteLine("var response = _context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + GetInclude(s) + ".Where(where ?? (_ => true))" + s.OrderByString + ";");
                var hasComputedValue = false;
                var tempModelCTForComputedValues = s;
                while (tempModelCTForComputedValues is not null)
                {
                    if (tempModelCTForComputedValues.Properties.Any(property => property!.SMField!.Field is not null && property.SMField.Field.Compute))
                        hasComputedValue = true;
                    tempModelCTForComputedValues = tempModelCTForComputedValues.ParentClassType as ModelCT;
                }

                //if (hasComputedValue)
                //{
                //    _writer.WriteLine($"foreach (var entity in response)");
                //    _writer.WriteLine("{");
                //     _writer.WriteLine("}");
                //}

                _writer.WriteLine("return await Task.FromResult(response);");
            }
        });

        classTypeTemplate.Members.Add(new TypeMethodTemplate($"Task<{s.NewQualifiedName}?>", "GetByIdAsync")
        {
            Modifiers = "public async",
            Parameters = new() { ("int", "id") },
            Body = (_writer, parameters, genericParamerters, privateFields) =>
{

    _writer.WriteLine("var response = _context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + GetInclude(s) + ".Where(x => x.Id == id).FirstOrDefault(); ");
    _writer.WriteLine("if (response is null) return null;");

    //var tempModelCTForComputedValues = s;
    //while (tempModelCTForComputedValues is not null)
    //{
    //    AddComputedValues(tempModelCTForComputedValues, _writer);
    //    tempModelCTForComputedValues = tempModelCTForComputedValues.ParentClassType as ModelCT;
    //}
    _writer.WriteLine("return await Task.FromResult(response);");
}
        });

        classTypeTemplate.Members.Add(new TypeMethodTemplate("Task", "AddAsync")
        {
            Modifiers = "public async",
            Parameters = new() { ($"{s.NewQualifiedName}", "entity") },
            Body = (_writer, parameters, genericParamerters, privateFields) =>
            {
                _writer.WriteLine("try");
                _writer.WriteLine("{");
                _writer.Indent++;
                _writer.WriteLine("_context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + ".Add(entity);");

                _writer.WriteLine($"await (_context as Microsoft.EntityFrameworkCore.DbContext).SaveChangesAsync();");
                _writer.WriteLine($"entity = _context.{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix.Pluralize()}.Find(entity.Id);");

                var tempModelCTForComputedValues = s;
                while (tempModelCTForComputedValues is not null)
                {
                    AssignComputedProperties("entity", _writer, tempModelCTForComputedValues.Properties.Where(_ => _!.SMField is not null)!);
                    tempModelCTForComputedValues = tempModelCTForComputedValues.ParentClassType as ModelCT;
                }

                _writer.WriteLine($"await (_context as Microsoft.EntityFrameworkCore.DbContext).SaveChangesAsync();");


                //  entity = _context.Invoice_SaleInvoiceProductDetails.Find(entity.Id);
                //  entity.TotalPrice = ComputeTotalPrice(_context, entity);
                //  await(_context as Microsoft.EntityFrameworkCore.DbContext).SaveChangesAsync();

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

                var tempModelCTForComputedValues = s;
                while (tempModelCTForComputedValues is not null)
                {
                    AssignComputedProperties("tempEntity", _writer, tempModelCTForComputedValues.Properties.Where(_ => _!.SMField is not null)!);
                    tempModelCTForComputedValues = tempModelCTForComputedValues.ParentClassType as ModelCT;
                }


                //_writer.WriteLine("_context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;");
                _writer.WriteLine($"await (_context as Microsoft.EntityFrameworkCore.DbContext).SaveChangesAsync();");
                _writer.WriteLine("}");
                _writer.WriteLine("catch (Exception ex)");
                _writer.WriteLine("{");
                _writer.WriteLine("throw ex;");
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
                _writer.WriteLine("var tempEntity = await _context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + ".FindAsync(id);");
                _writer.WriteLine("_context." + s.ModuleNameWithoutPostFix + "_" + s.IdentifierNameWithoutPostFix.Pluralize() + ".Remove(tempEntity);");
                _writer.WriteLine($"await (_context as Microsoft.EntityFrameworkCore.DbContext).SaveChangesAsync();");
                _writer.WriteLine("}");
                _writer.WriteLine("catch (Exception ex)");
                _writer.WriteLine("{");
                _writer.WriteLine("throw ex;");
                _writer.WriteLine("}");
            }
        });

        var tempModelCTForMethods = s;
        while (tempModelCTForMethods is not null)
        {
            //classTypeTemplate.UsingNamespaces.AddRange(tempModelCTForMethods.Usings.Where(_ => _.StartsWith(tempModelCTForMethods.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME)));
            StaticMethods.AddModelMethods(tempModelCTForMethods, classTypeTemplate);
            tempModelCTForMethods = tempModelCTForMethods.ParentClassType as ModelCT;
        }

    }

    private static string GetInclude(ModelCT s)
    {
        StringBuilder sb = new();
        foreach (var p in s.Properties.Where(_ => _!.RelationshipWith is not null))
        {
            sb.Append($".Include(_=>_.{p!.IdentifierName})");
        }
        return sb.ToString();
    }



    /// <summary>
    /// Assigns the computed properties.
    /// </summary>
    /// <param name="w">The w.</param>
    /// <param name="smFields">The sm fields.</param>
    private static void AssignComputedProperties(string obj, IndentedTextWriter w, IEnumerable<TypeProperty> smFields)
    {
        foreach (var property in smFields)
            if (property!.SMField!.Field is not null && property.SMField.Field.Compute)
            {
                w.WriteLine($"{obj}.{property!.IdentifierName} = Compute{property.IdentifierName}(_context,entity);");
                continue;
            }
    }

    /// <summary>
    /// Adds the computed values.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="w">The w.</param>
    private static void AddComputedValues(ModelCT s, IndentedTextWriter w)
    {
        foreach (var property in s.Properties.Where(_ => _!.SMField is not null))
            if (property!.SMField!.Field is not null && property.SMField.Field.Compute)
            {
                w.WriteLine($"response.{property!.IdentifierName} = Compute{property.IdentifierName}(_context,response);");
                continue;
            }
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
