namespace SMF.EntityFramework.SourceGenerator.Generators;

using Humanizer;
using SMF.SourceGenerator.Core;
using SMF.SourceGenerator.Core.Types;

/// <summary>
/// The model generator.
/// </summary>
[Generator]
internal class ModelGenerator : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(RegisteredModelCTs, AddModelClasses);
    }

    /// <summary>
    /// Adds the model classes.
    /// </summary>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    private void AddModelClasses(SourceProductionContext c, ModelCT s)
    {
        SMFProductionContext context = new(c);
        if (s.ContainingModuleName is null) return;

        FileScopedNamespaceTemplate fileScopedNamespace = new(s.NewContainingNamespace);

        ClassTypeTemplate classTypeTemplate = new(s.IdentifierNameWithoutPostFix)
        {
            Modifiers = "public partial",
            ParentType = s.StringParentType == "ModelBase" ? null : s.ParentClassType?.NewQualifiedName,

        };
        if (s.StringParentType == "ModelBase")
            AddDefaultProperties(classTypeTemplate);
        foreach (var property in s.Properties!)
        {

            string identifierName = property!.IdentifierName;
            if (property.RelationshipWith is RelationshipWith relationshipWith && relationshipWith.RelationshipType is RelationshipType.M2O or RelationshipType.M2M)
                identifierName = identifierName.Pluralize();
            AutoPropertyTemplate p = new(ModelPropertyTypes.GetPropertyType(property!), identifierName)
            {
                Comment = property.Comment,
                SecondAccessor = "set"
            };
            classTypeTemplate.Members.Add(p);
        }
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource(fileScopedNamespace);
        //Debug.Assert(false, classTypeTemplate.CreateTemplate().GetTemplate());
    }

    /// <summary>
    /// Adds the default properties.
    /// </summary>
    /// <param name="classTypeTemplate">The class type template.</param>
    /// <param name="s">The s.</param>
    private static void AddDefaultProperties(ClassTypeTemplate classTypeTemplate)
    {
        classTypeTemplate.Members.Add(new AutoPropertyTemplate("int", "Id") { SecondAccessor = "set" });
        classTypeTemplate.Members.Add(new AutoPropertyTemplate("System.DateTime", "CreatedOn") { SecondAccessor = "set" });
        classTypeTemplate.Members.Add(new AutoPropertyTemplate("System.DateTime", "LastModifiedOn") { SecondAccessor = "set" });
    }
}
