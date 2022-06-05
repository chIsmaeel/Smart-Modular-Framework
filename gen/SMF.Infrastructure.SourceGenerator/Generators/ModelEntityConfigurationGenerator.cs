namespace Infrastructure.Data.Config;

using Humanizer;
using SMF.SourceGenerator.Core;

/// <summary>                         
/// The model entity configuration generator.
/// </summary>

[Generator]

internal partial class ModelEntityConfigurations : CommonIncrementalGenerator
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

        SMFProductionContext context = new(c);
        FileScopedNamespaceTemplate fileScopedNamespace = new($"{s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Infrastructure.{s.ContainingModuleName}.Data.Config");
        ClassTypeTemplate classTypeTemplate = new($"{s.IdentifierNameWithoutPostFix}EntityTypeConfiguration")
        {
            Modifiers = "public partial",
            Interfaces = new() { "IEntityTypeConfiguration<" + s.NewQualifiedName + ">" },
        };
        TypeMethodTemplate typeMethodTemplate = new("void", "Configure")
        {
            Modifiers = "public",
            Parameters = new() { ($"EntityTypeBuilder<{s.NewQualifiedName}>", "builder") },
            Body = (writer, parameters, genericParameters, fields) =>
            {
                WriteConfigBody(s, writer);
            }
        };
        typeMethodTemplate.UsingNamespaces.AddRange(new[] { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.Metadata.Builders" });

        classTypeTemplate.Members.Add(typeMethodTemplate);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource($"{s.ModuleNameWithoutPostFix}_{s.IdentifierNameWithoutPostFix}Config", fileScopedNamespace.CreateTemplate().GetTemplate());
    }

    /// <summary>
    /// Writes the config body.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="writer">The writer.</param>
    private void WriteConfigBody(ModelCT s, IndentedTextWriter writer)
    {
        if (s.StringParentType == "ModelBase")
        {
            writer.Write(@$"builder.Property(e => e.CreatedOn).HasColumnType(""datetime""); ");
            writer.Write(@$"builder.Property(e => e.LastModifiedOn).HasColumnType(""datetime""); ");


        }
        //Debugger.Launch();
        var indexList = new List<string>()
        {
            "e.Id"
        };

        foreach (var property in s.Properties!)
        {

            SMField? field = property!.SMField;
            if (field!.Field is null) continue;

            //if (property.SMField!.Field is not null && property.SMField.Field.Compute)
            //{
            //    writer.Write($" builder.Ignore(e => e.{property.IdentifierName});");
            //    continue;
            //}
            WritePropertyFluentAPIs(writer, field, property!);
            if (property!.RelationshipWith is not null)
                AddRelationalFluentAPI(writer, property);

            if (field.Field.Index == true)
                indexList.Add("e." + property!.IdentifierName);
        }
        WriteIfHasIndex(writer, indexList);
    }
}
/// <summary>
/// The model entity configuration generator.
/// </summary>             

internal partial class ModelEntityConfigurations
{
    /// <summary>                         
    /// Writes the values.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="field">The field.</param>
    /// <param name="property">The property.</param>
    private void WritePropertyFluentAPIs(IndentedTextWriter writer, SMField field, TypeProperty property)
    {
        if (property.RelationshipWith is not null) return;
        writer.Write("builder.Property(e => e.{0})", property.IdentifierName);

        AddPropertyFluentAPI(writer, field, property);

        writer.WriteLine(";");
    }

    /// <summary>
    /// Adds the relational fluent a p i.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="field">The field.</param>
    /// <param name="property">The property.</param>
    private static void AddRelationalFluentAPI(IndentedTextWriter writer, TypeProperty property)
    {
        writer.Write("builder");
        if (property.RelationshipWith!.RelationshipType is SMF.SourceGenerator.Core.Types.RelationshipType.O2O or SMF.SourceGenerator.Core.Types.RelationshipType.O2M)
            writer.Write(".HasOne(_ => _.{0})", property.IdentifierName);
        else
            writer.Write(".HasMany(_ => _.{0})", property.IdentifierName.Pluralize());

        if (property.RelationshipWith!.RelationshipType is SMF.SourceGenerator.Core.Types.RelationshipType.O2O or SMF.SourceGenerator.Core.Types.RelationshipType.M2O)
            writer.Write(".WithOne(_ => _.{0})", property.RelationshipWith.WithRelationship.IdentifierName);
        else
            writer.Write(".WithMany(_ => _.{0})", property.RelationshipWith.WithRelationship.IdentifierName);


        if (property.RelationshipWith.ForeignKey is not null)
        {
            if (property.RelationshipWith.RelationshipType is SMF.SourceGenerator.Core.Types.RelationshipType.O2O)
                writer.Write(".HasForeignKey<{1}>(_ => _.{0})", property.RelationshipWith.ForeignKey.IdentifierName, (property.RelationshipWith.ForeignKey.ClassType as ModelCT)!.NewQualifiedName);
            else
                writer.Write(".HasForeignKey(_ => _.{0})", property.RelationshipWith.ForeignKey.IdentifierName);
        }
        writer.Write(";");
    }

    /// <summary>
    /// Adds the property fluent a p i.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="field">The field.</param>
    /// <param name="property">The property.</param>
    private void AddPropertyFluentAPI(IndentedTextWriter writer, SMField field, TypeProperty property)
    {
        AddFluentAPI(writer, "HasColumnType", field.Field!.DbType);
        AddFluentAPI(writer, "HasComment", property.Comment);
        AddFluentAPI(writer, "HasDefaultValueSql", field.Field!.DefaultValueSql);
        var defaultValue = GetDefaultValue(field.Field!);
        if (field.Field.DbType == "BIT" && defaultValue is "true" or "false")
            AddFluentAPIWithout(writer, "HasDefaultValue", defaultValue);
        else
            AddFluentAPI(writer, "HasDefaultValue", defaultValue);

        AddFluentAPIIfValueIsTrue(writer, "IsRequired", field.Field!.IsRequired);

    }

    /// <summary>
    /// Adds the fluent a p i if value is true.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="fluentAPIName">The fluent a p i name.</param>
    /// <param name="value">If true, value.</param>
    private static void AddFluentAPIIfValueIsTrue(IndentedTextWriter writer, string fluentAPIName, bool value)
    {
        if (value)
            writer.Write(".{0}()", fluentAPIName);
    }

    /// <summary>
    /// Adds the fluent a p i.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="field">The field.</param>
    private static void AddFluentAPI(IndentedTextWriter writer, string fluentAPIName, string? value)
    {
        if (!string.IsNullOrEmpty(value))
            writer.Write($".{fluentAPIName}(\"{value}\")");
    }

    private static void AddFluentAPIWithout(IndentedTextWriter writer, string fluentAPIName, string? value)
    {
        if (!string.IsNullOrEmpty(value))
            writer.Write($".{fluentAPIName}({value})");
    }

    /// <summary>
    /// Gets the default value.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <returns>A string? .</returns>
    private string? GetDefaultValue(SMF.ORM.Fields.Field field)
    {
        return field switch
        {
            // SMF.ORM.Fields.Binary => ((SMF.ORM.Fields.Binary)field).DefaultValue is null ? null : @$"CAST('{Convert.ToBase64String(((SMF.ORM.Fields.Binary)field).DefaultValue!.Invoke())}' AS VARBINARY)",
            SMF.ORM.Fields.Boolean => ((SMF.ORM.Fields.Boolean)field).DefaultValue is false ? $"false" : $"true",
            SMF.ORM.Fields.DateTime => ((SMF.ORM.Fields.DateTime)field).DefaultValue.ToString() == default(System.DateTime).ToString() ? null : ((SMF.ORM.Fields.DateTime)field).DefaultValue.ToString(),
            SMF.ORM.Fields.Float => ((SMF.ORM.Fields.Float)field).DefaultValue.ToString(),
            SMF.ORM.Fields.Int => ((SMF.ORM.Fields.Int)field).DefaultValue.ToString(),
            SMF.ORM.Fields.String => ((SMF.ORM.Fields.String)field).DefaultValue.ToString(),
            _ => null,

        };
    }

    /// <summary>
    /// Writes the if has index.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="indexList">The index list.</param>
    private static void WriteIfHasIndex(IndentedTextWriter writer, List<string> indexList)
    {
        if (indexList.Count == 1)
            writer.WriteLine($"builder.HasIndex(e => {indexList.FirstOrDefault()});");
        else if (indexList.Count > 1)
            writer.WriteLine($"builder.HasIndex(e=> new {{{string.Join(",", indexList)}}});");
    }
}

