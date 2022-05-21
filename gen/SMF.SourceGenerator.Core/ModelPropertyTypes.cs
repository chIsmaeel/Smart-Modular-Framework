namespace SMF.SourceGenerator.Core;

using SMF.SourceGenerator.Core.Types.TypeMembers;

/// <summary>
/// The model property types.
/// </summary>

public class ModelPropertyTypes
{
    /// <summary>
    /// Gets the property type.
    /// </summary>
    public static string GetPropertyType(TypeProperty property)
    {
        string propertyType = property!.Type;
        bool isNullable = false;
        if (!propertyType.StartsWith("SMFields")) return propertyType;
        if (propertyType.EndsWith("?"))
        {
            isNullable = true;
            propertyType = propertyType.Substring(0, propertyType.Length - 1);
        }
        string? result = propertyType switch
        {
            "SMFields.String" => "string",
            "SMFields.Id" => "int",
            "SMFields.Int" => "int",
            "SMFields.O2O" => property.RelationshipWith!.WithRelationship.ClassType.NewQualifiedName,
            "SMFields.O2M" => property.RelationshipWith!.WithRelationship.ClassType.NewQualifiedName,
            "SMFields.M2O" => $"System.Collections.Generic.List<{property.RelationshipWith!.WithRelationship.ClassType.NewQualifiedName}>",
            "SMFields.M2M" => $"System.Collections.Generic.ICollection<{property.RelationshipWith!.WithRelationship.ClassType.NewQualifiedName}>",
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
        if (result is null) return "";
        return isNullable ? $"{result}?" : result;
    }
}
