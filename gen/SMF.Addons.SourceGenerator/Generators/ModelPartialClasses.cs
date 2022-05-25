namespace SMF.Addons.SourceGenerator.Generators;

using SMF.SourceGenerator.Abstractions;
using SMF.SourceGenerator.Core;
using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates;

/// <summary>
/// The model partial classes.
/// </summary>
[Generator]

internal class ModelPartialClasses : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        //System.Diagnostics.Debugger.Launch();
        context.RegisterImplementationSourceOutput(ModelCTs, AddModelPartialClasses);
    }

    /// <summary>
    /// Adds the model partial classes.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddModelPartialClasses(SourceProductionContext c, ModelCT s)
    {
        SMFProductionContext context = new(c);
        //if (s.IdentifierName == "PurchaseLineModel")

        FileScopedNamespaceTemplate namespaceTemplate = new(s.ContainingNamespace);
        var ii = s.ParentClassType;
        ClassTypeTemplate classTemplate = new(s.IdentifierName)
        {
            AddDefaultCommentIfNotExist = false,
            Modifiers = "public partial",
            ParentType = s.QualifiedParentName == "SMF.ORM.Models.ModelBase" ? s.QualifiedParentName : s.ParentClassType?.QualifiedName,
        };
        if (s.StringParentType == "ModelBase")
            DefaultPropertiesIfInheritModelIsNotDefined(classTemplate);
        foreach (var property in s.Properties.Where(_ => _!.SMField is not null && _.SMField.Field is not null))
        {
            if ((bool)(property?.SMField?.Field?.Compute)!)
            {
                classTemplate.Members.Add(new PartialMethodTemplate(ModelPropertyTypes.GetPropertyType(property), $"Compute{property.IdentifierName}")
                {

                    Modifiers = "private",
                    Parameters = new() { ($"{QualifiedNames.GetISMFDbContext(s.ConfigSMFAndGlobalOptions.ConfigSMF!)}", "_context"), ((property.ClassType as ModelCT)!.NewQualifiedName, "currentObj") }
                });
            }
        }

        namespaceTemplate.TypeTemplates.Add(classTemplate);
        context.AddSource(namespaceTemplate);
    }

    /// <summary>
    /// Defaults the properties if inherit model is not defined.
    /// </summary>
    /// <param name="classTemplate">The class template.</param>
    private static void DefaultPropertiesIfInheritModelIsNotDefined(ClassTypeTemplate classTemplate)
    {
        AutoPropertyTemplate id = new("SMFields.Id", "Id")
        {
            Comment = "Primary Key of the Model.",
            DefaultValue = "new()",
        };
        AutoPropertyTemplate createdOn = new("SMFields.DateTime", "CreatedOn")
        {
            Comment = "Primary Key of the Model.",
            DefaultValue = "new()"
        };
        AutoPropertyTemplate lastModifiedOn = new("SMFields.DateTime", "LastModifiedOn")
        {
            Comment = "Primary Key of the Model.",
            DefaultValue = "new()"
        };

        classTemplate.Members.Add(id);
        classTemplate.Members.Add(createdOn);
        classTemplate.Members.Add(lastModifiedOn);
    }
}

