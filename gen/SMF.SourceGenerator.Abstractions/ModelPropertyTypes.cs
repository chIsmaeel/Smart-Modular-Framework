namespace SMF.Common.SourceGenerator.Abstractions;
/// <summary>
/// The model property types.
/// </summary>

public class ModelPropertyTypes
{
    /// <summary>
    /// Gets the property type.
    /// </summary>
    public static string GetPropertyType(string propertyType)
    {
        bool isNullable = false;
        if (!propertyType.StartsWith("SMFields")) return propertyType;
        if (propertyType.EndsWith("?"))
        {
            isNullable = true;
            propertyType = propertyType.Substring(0, propertyType.Length - 1);
        }
        var result = propertyType switch
        {
            "SMFields.String" => "string",
            "SMFields.Id" => "int",
            "SMFields.Int" => "int",
            "SMFields.Decimal" => "decimal",
            "SMFields.DateTime" => "DateTime",
            "SMFields.Binary" => "byte[]",
            "SMFields.Boolean" => "bool",
            "SMFields.Double" => "double",
            "SMFields.Float" => "float",
            "SMFields.Byte" => "byte",
            "SMFields.SByte" => "sbyte",
            "SMFields.Int16" => "short",
            "SMFields.Int32" => "int",
            "SMFields.Int64" => "long",
            "SMFields.UInt16" => "ushort",
            "SMFields.UInt32" => "uint",
            "SMFields.UInt64" => "ulong",
            "SMFields.Guid" => "Guid",
            "SMFields.Char" => "char",
            "SMFields.Object" => "object",
            _ => propertyType
        };
        return isNullable ? $"{result}?" : result;
    }
}
