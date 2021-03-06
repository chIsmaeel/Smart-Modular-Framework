namespace SMF.EntityFramework.SourceGenerator;

using Microsoft.CodeAnalysis.CSharp.Scripting;

/// <summary>
/// The s m fields.
/// </summary>

internal class SMField
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFramework.Generator.SMField"/> class.
    /// </summary>
    /// <param name="modelField">The model field.</param>
    public SMField(TypeProperty modelProperty)
    {
        ModelProperty = modelProperty;
        AssignValues();
    }

    /// <summary>
    /// Gets the model property.
    /// </summary>
    public TypeProperty ModelProperty { get; }

    /// <summary>
    /// Gets the s m field.
    /// </summary>
    public ORM.Fields.Field? SMFField { get; private set; }

    /// <summary>
    /// Assigns the values.
    /// </summary>
    public void AssignValues()
    {

        SMFField = GetFieldObj(ModelProperty.Type);
        if (SMFField is null || ModelProperty.AssignmentExpressionsIdentiferAndValue is null) return;
        foreach ((string identifer, string value) in ModelProperty.AssignmentExpressionsIdentiferAndValue!)
        {
            if (identifer == "DefaultValue")
            {

                //StringBuilder sb = new();
                //sb.Append(GlobalUsings);
                //foreach (var cds in ClassType.ClassDSs)
                //{
                //    sb.AppendLine(cds.ToFullString());
                //}
                //#if DEBUG
                //                if (!System.Diagnostics.Debugger.IsAttached)
                //                    System.Diagnostics.Debugger.Launch();
                //#endif
                //var rr = ClassType.Methods.FirstOrDefault(m => m.IdentifierName == value)?.MDS?.ToString();
                //var v = CSharpScript.Create(sb.ToString()).RunAsync().Result;
            }

            SMFField?.GetType().GetProperty(identifer)?.SetValue(SMFField, GetValue(identifer, value));
        }
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="indentifier">The indentifier.</param>
    /// <param name="value">The value.</param>                                       
    /// <returns>An object.</returns>
    private object? GetValue(string indentifier, string value)
    {
        if (indentifier == "DefaultValue")
            return GetDefaultValue(value);
        return indentifier switch
        {
            "IsReadOnly" => bool.Parse(value),
            "IsRequired" => bool.Parse(value),
            "Store" => bool.Parse(value),
            "Compute" => bool.Parse(value),
            "Translate" => bool.Parse(value),
            "Index" => bool.Parse(value),
            _ => value.Substring(1, value.Length - 2),
        };
    }

    /// <summary>
    /// Gets the default value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>An object.</returns>
    private object? GetDefaultValue(string? value)
    {
        if (value is null) return null;
        return SMFField switch
        {
            ORM.Fields.Binary => value.Contains("=>") ? CSharpScript.EvaluateAsync<Func<byte[]>>(value).Result : null,
            ORM.Fields.DateTime => System.DateTime.Parse(value),
            ORM.Fields.Decimal => decimal.Parse(value),
            ORM.Fields.Id => int.Parse(value),
            ORM.Fields.Int => int.Parse(value),
            _ => value.Substring(1, value.Length - 2)
        };
    }

    /// <summary>
    /// Gets the field obj.
    /// </summary>
    /// <param name="type">The type.</param>       
    private static ORM.Fields.Field? GetFieldObj(string typeName)
    {
        if (!typeName.StartsWith("SMFields.")) return null;
        if (typeName.EndsWith("?")) typeName = typeName.Substring(0, typeName.Length - 1);
        return typeName switch
        {
            "SMFields.String" => new ORM.Fields.String(),
            "SMFields.Int" => new ORM.Fields.Int(),
            "SMFields.Decimal" => new ORM.Fields.Decimal(),
            "SMFields.DateTime" => new ORM.Fields.DateTime(),
            "SMFields.Boolean" => new ORM.Fields.Boolean(),
            "SMFields.Id" => new ORM.Fields.Id(),
            "SMFields.Binary" => new ORM.Fields.Binary(),
            _ => null
        };
    }
}
