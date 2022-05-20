namespace SMF.EntityFramework.SourceGenerator.Generators;
/// <summary>
/// The model entity configuration generator.
/// </summary>

[Generator]

internal class ModelEntityConfigurationGenerator : CommonIncrementalGenerator
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
                var indexList = new List<string>();
                if (s.Properties.Any(_ => _.IdentifierName == s.IdentifierNameWithoutPostFix + "Id"))
                    indexList.Add("e." + s.IdentifierNameWithoutPostFix + "Id");

                foreach (var property in s.Properties!)
                {
                    SMField? field = new(property);
                    if (field.SMFField is null) return;
                    WritePropertyFluentAPIs(writer, field, property);
                    if (field.SMFField.Index == true)
                        indexList.Add("e." + property.IdentifierName);
                }
                WriteIfHasIndex(writer, indexList);

            }
        };
        typeMethodTemplate.UsingNamespaces.AddRange(new[] { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.Metadata.Builders" });

        classTypeTemplate.Members.Add(typeMethodTemplate);
        fileScopedNamespace.TypeTemplates.Add(classTypeTemplate);
        context.AddSource(fileScopedNamespace);
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

    /// <summary>
    /// Writes the values.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="field">The field.</param>
    /// <param name="property">The property.</param>
    private static void WritePropertyFluentAPIs(IndentedTextWriter writer, SMField field, TypeProperty property)
    {

        if (!string.IsNullOrEmpty(property.Comment) || !string.IsNullOrEmpty(GetDefaultValue(field.SMFField!)))
        {
            writer.WriteLine("builder");
            writer.Indent++;
            writer.Write(".Property(e => e.{0})", property.IdentifierName);

            if (!string.IsNullOrEmpty(field.SMFField!.DbType))
            {
                writer.WriteLine();
                writer.Write($".HasColumnType(\"{field.SMFField!.DbType}\")");
            }
            if (!string.IsNullOrEmpty(property.Comment))
            {
                writer.WriteLine();
                writer.Write(".HasComment(\"{0}\")", property.Comment);
            }

            if (!string.IsNullOrEmpty(field.SMFField!.DefaultValueSql))
            {
                writer.WriteLine();
                writer.Write(".HasDefaultValueSql(\"{0}\")", field.SMFField!.DefaultValueSql);
            }

            if (!string.IsNullOrEmpty(GetDefaultValue(field.SMFField!)))
            {
                var defaultValue = GetDefaultValue(field.SMFField!);
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    writer.WriteLine();
                    writer.Write(".HasDefaultValue(\"{0}\")", defaultValue);
                }
            }

            if (field.SMFField!.IsRequired == true)
            {
                writer.WriteLine();
                writer.Write(".IsRequired()");
            }

            if (field.SMFField is ORM.Fields.Id)
            {
                writer.WriteLine();
                writer.Write(".UseHiLo()");
            }
            writer.WriteLine(";");
            writer.Indent--;
            writer.WriteLine();
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
}
