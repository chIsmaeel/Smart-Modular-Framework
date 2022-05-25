﻿namespace SMF.Grpc.SourceGenerator.Generators;

using SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;
using System.Text;
using Humanizer;
using SMF.SourceGenerator.Core.Types.TypeMembers;

/// <summary>
/// The static methods.
/// </summary>

internal class StaticMethods
{

    /// <summary>
    /// Gets the proto type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>An object.</returns>
    public static string? GetProtoType(TypeProperty property)
    {
        string propertyType = property!.Type;
        if (!propertyType.StartsWith("SMFields"))
        {
            //if ()
            return propertyType;
        }
        var result = propertyType switch
        {
            "SMFields.String" => "string",
            "SMFields.Id" => "int32",
            "SMFields.Int" => "int32",
            "SMFields.O2O" => (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix + "_" + (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix,
            "SMFields.O2M" => (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix + "_" + (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix,
            "SMFields.M2O" => $"repeated {(property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix + "_" + (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}",
            "SMFields.M2M" => $"repeated {(property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix + "_" + (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}",
            "SMFields.DateTime" => "DateTime",
            "SMFields.Int?" => "google.protobuf.Int32Value",
            "SMFields.Long?" => "google.protobuf.Int64Value",
            "SMFields.String?" => "google.protobuf.StringValue",
            "SMFields.Float?" => "google.protobuf.DecimalValue",
            "SMFields.DateTime?" => "google.protobuf.Timestamp",
            "SMFields.Boolean?" => "google.protobuf.BoolValue",
            "SMFields.Boolean" => "bool",
            "SMFields.Binary" => "byte",
            "SMFields.Binary?" => "google.protobuf.BytesValue",

            "SMFields.ByteString?" => "google.protobuf.BytesValue",
            "SMFields.Double" => "double",
            "SMFields.Float" => "float",
            "SMFields.Byte" => "byte",
            "SMFields.SByte" => "sbyte",
            "SMFields.Int16" => "short",
            "SMFields.Int32" => "int",
            "SMFields.Int64" => "int64",
            "SMFields.UInt16" => "ushort",
            "SMFields.UInt32" => "uint",
            "SMFields.UInt64" => "ulong",
            "SMFields.Guid" => "Guid",
            "SMFields.Char" => "char",
            "SMFields.Object" => "object",
            _ => propertyType
        };
        return result;

    }
    /// <summary>
    /// Adds the properties.
    /// </summary>
    /// <param name="tempModelCT">The temp model c t.</param>
    /// <param name="messages">The messages.</param>
    /// <param name="i">The i.</param>
    internal static void AddProperties(ModelCT tempModelCT, StringBuilder messages, ref int i)
    {
        foreach (var property in tempModelCT.Properties!)
        {
            var identifer = property!.IdentifierName;
            var type = GetProtoType(property!);
            if (type is null) continue;
            if ((bool)type?.StartsWith("repeated")!)
                identifer = identifer.Pluralize();
            messages.AppendLine($"\t{type} {identifer} = {i};");
            i++;
        }
    }

    /// <summary>
    /// Adds the properties.
    /// </summary>
    /// <param name="tempModelCT">The temp model c t.</param>
    /// <param name="messages">The messages.</param>
    /// <param name="i">The i.</param>
    internal static void CreateCommandProperties(ModelCT tempModelCT, StringBuilder messages, ref int i)
    {
        foreach (var property in tempModelCT.Properties!)
        {
            var identifer = property!.IdentifierName;
            var type = GetProtoType(property!);
            if (type is null) return;
            if (type!.StartsWith("repeated"))
                identifer = identifer.Pluralize();

            if (property.SMField is not null && property.SMField.Field is not null)
            {
                if (property.SMField.Field.Compute)
                    continue;
            }
            messages.AppendLine($"\t{type} {identifer} = {i};");
            i++;
        }
    }
    /// <summary>
    /// Adds the properties.
    /// </summary>
    /// <param name="tempModelCT">The temp model c t.</param>
    /// <param name="messages">The messages.</param>
    /// <param name="i">The i.</param>
    internal static void UpdateCommandProperties(ModelCT tempModelCT, StringBuilder messages, ref int i)
    {
        foreach (var property in tempModelCT.Properties!)
        {
            var identifer = property!.IdentifierName;
            var type = GetProtoType(property!);
            if (type is null) return;
            if (type!.StartsWith("repeated"))
                identifer = identifer.Pluralize();

            if (property.SMField is not null && property.SMField.Field is not null)
            {
                if (property.SMField.Field.Compute)
                    continue;
            }
            messages.AppendLine($"\t{type} {identifer} = {i};");
            i++;
        }
    }
}