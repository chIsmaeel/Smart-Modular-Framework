namespace SMF.EntityFramework.SourceGenerator.Generators;

using Humanizer;

/// <summary>
/// The model entity configuration generator.
/// </summary>

[Generator]

internal partial class ModelEntityConfigurationGenerator : CommonIncrementalGenerator
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
        FileScopedNamespaceTemplate fileScopedNamespace = new($"{s.ConfigSMFAndGlobalOptions.ConfigSMF!.SOLUTION_NAME}.Domain.{s.ContainingModuleName}.Entities.Configurations");
        ClassTypeTemplate classTypeTemplate = new(s.IdentifierNameWithoutPostFix + "EntityTypeConfiguration")
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
        context.AddSource(fileScopedNamespace);
    }

    /// <summary>
    /// Writes the config body.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="writer">The writer.</param>
    private static void WriteConfigBody(ModelCT s, IndentedTextWriter writer)
    {
        //Debugger.Launch();
        var indexList = new List<string>()
        {
            "e.Id"
        };

        foreach (var property in s.Properties!)
        {
            if (property!.IdentifierName == "SalePrices")
            {
                //#if DEBUG
                //                if (!System.Diagnostics.Debugger.IsAttached)
                //                    System.Diagnostics.Debugger.Launch();
                //#endif
            }
            SMField? field = new(property!);
            if (field.SMFField is null) continue;

            WritePropertyFluentAPIs(writer, field, property!);
            if (property.IdentifierName == "SalePrices")
            {

            }
            if (property!.RelationshipWith is not null)
                AddRelationalFluentAPI(writer, property);

            if (field.SMFField.Index == true)
                indexList.Add("e." + property!.IdentifierName);
        }
        WriteIfHasIndex(writer, indexList);
    }
}
/// <summary>
/// The model entity configuration generator.
/// </summary>

internal partial class ModelEntityConfigurationGenerator
{
    /// <summary>
    /// Writes the values.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="field">The field.</param>
    /// <param name="property">The property.</param>
    private static void WritePropertyFluentAPIs(IndentedTextWriter writer, SMField field, TypeProperty property)
    {
        writer.WriteLine("builder");
        writer.Indent++;
        writer.Write(".Property(e => e.{0})", property.IdentifierName);

        AddPropertyFluentAPI(writer, field, property);

        writer.WriteLine(";");
        writer.Indent--;
        writer.WriteLine();
    }

    /// <summary>
    /// Adds the relational fluent a p i.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="field">The field.</param>
    /// <param name="property">The property.</param>
    private static void AddRelationalFluentAPI(IndentedTextWriter writer, TypeProperty property)
    {
        writer.WriteLine("builder");
        writer.Indent++;
        if (property.RelationshipWith!.RelationshipType is SMF.SourceGenerator.Core.Types.RelationshipType.O2O or SMF.SourceGenerator.Core.Types.RelationshipType.O2M)
            writer.Write(".HasOne(_ => _.{0})", property.IdentifierName);
        else
            writer.Write(".HasMany(_ => _.{0})", property.IdentifierName.Pluralize());

        writer.WriteLine();

        if (property.RelationshipWith!.RelationshipType is SMF.SourceGenerator.Core.Types.RelationshipType.O2O or SMF.SourceGenerator.Core.Types.RelationshipType.M2O)
            writer.Write(".WithOne(_ => _.{0})", property.RelationshipWith.WithRelationship.IdentifierName);
        else
            writer.Write(".WithMany(_ => _.{0})", property.RelationshipWith.WithRelationship.IdentifierName);


        if (property.RelationshipWith.ForeignKey is not null)
        {
            writer.WriteLine();

            if (property.RelationshipWith.RelationshipType is SMF.SourceGenerator.Core.Types.RelationshipType.M2O)
                writer.Write(".HasForeignKey<{1}>(_ => _.{0})", property.RelationshipWith.ForeignKey.IdentifierName, (property.RelationshipWith.ForeignKey.ClassType as ModelCT)!.NewQualifiedName);
            else
                writer.Write(".HasForeignKey<{1}>(_ => _.{0})", property.RelationshipWith.ForeignKey.IdentifierName, (property.RelationshipWith.ForeignKey.ClassType as ModelCT)!.NewQualifiedName);
        }
        writer.WriteLine(";");
        writer.Indent--;
        writer.WriteLine();
    }

    /// <summary>
    /// Adds the property fluent a p i.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="field">The field.</param>
    /// <param name="property">The property.</param>
    private static void AddPropertyFluentAPI(IndentedTextWriter writer, SMField field, TypeProperty property)
    {
        AddFluentAPI(writer, "HasColumnType", field.SMFField!.DbType);
        AddFluentAPI(writer, "HasComment", property.Comment);
        AddFluentAPI(writer, "HasDefaultValueSql", field.SMFField!.DefaultValueSql);
        AddFluentAPI(writer, "HasDefaultValueSql", field.SMFField!.DefaultValueSql);
        AddFluentAPI(writer, "HasDefaultValue", GetDefaultValue(field.SMFField!));
        AddFluentAPIIfValueIsTrue(writer, "IsRequired", field.SMFField!.IsRequired);
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
        {
            writer.WriteLine();
            writer.Write(".{0}()", fluentAPIName);
        }
    }

    /// <summary>
    /// Adds the fluent a p i.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="field">The field.</param>
    private static void AddFluentAPI(IndentedTextWriter writer, string fluentAPIName, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            writer.WriteLine();
            writer.Write($".{fluentAPIName}(\"{value}\")");
        }
    }

    /// <summary>
    /// Gets the default value.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <returns>A string? .</returns>
    private static string? GetDefaultValue(ORM.Fields.Field field)
    {
        return field switch
        {
            ORM.Fields.Binary => ((ORM.Fields.Binary)field).DefaultValue is null ? null : @$"CAST('{Convert.ToBase64String(((ORM.Fields.Binary)field).DefaultValue!.Invoke())}' AS VARBINARY)",
            ORM.Fields.Boolean => ((ORM.Fields.Boolean)field).DefaultValue.ToString(),
            ORM.Fields.DateTime => ((ORM.Fields.DateTime)field).DefaultValue.ToString() == default(System.DateTime).ToString() ? null : ((ORM.Fields.DateTime)field).DefaultValue.ToString(),
            ORM.Fields.Decimal => ((ORM.Fields.Decimal)field).DefaultValue.ToString(),
            ORM.Fields.Int => ((ORM.Fields.Int)field).DefaultValue.ToString(),
            ORM.Fields.String => ((ORM.Fields.String)field).DefaultValue.ToString(),
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
        {
            writer.WriteLine("builder");
            writer.Indent++;
            writer.WriteLine($".HasIndex(e => {indexList.FirstOrDefault()});");
            writer.Indent--;
        }
        else if (indexList.Count > 1)
        {
            writer.WriteLine("builder");
            writer.Indent++;
            writer.WriteLine($".HasIndex(e=> new {{{string.Join(", ", indexList)}}});");
            writer.Indent--;
        }
    }
}

