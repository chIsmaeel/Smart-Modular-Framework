namespace SMF.Grpc.SourceGenerator;

using Humanizer;
using SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;
using SMF.SourceGenerator.Core.Types.TypeMembers;
using System.Text;

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
        if (property.IdentifierName.EndsWith("_FK"))
            return null;

        if (!propertyType.StartsWith("SMFields"))
            return null;
        var result = propertyType switch
        {
            "SMFields.String" => "string",
            "SMFields.Id" => "int32",
            "SMFields.Int" => "int32",
            "SMFields.O2O" => (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix + "_" + (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix,
            "SMFields.O2M" => (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix + "_" + (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix,
            "SMFields.M2O" => $"repeated {(property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix + "_" + (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}",
            "SMFields.M2M" => $"repeated {(property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix + "_" + (property.RelationshipWith?.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}",
            "SMFields.DateTime" => "google.protobuf.Timestamp",
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
    internal static object ModifiedEntityImplicitOperatorProperties(ModelCT s, string objName, bool allowComputedValues = true, bool addCreatedOn = false, bool addLastModifiedOn = false)
    {
        StringBuilder sb = new();
        var tempModelCT = s;

        while (tempModelCT is not null)
        {
            foreach (var property in tempModelCT.Properties!)
            {
                var identifer = property!.IdentifierName;
                if (!allowComputedValues)
                {
                    if (property.SMField is not null && property.SMField.Field is not null)
                        if (property.SMField.Field.Compute)
                            continue;

                }
                if (property.Type is not null and ("SMFields.Binary" or "SMFields.Binary?"))
                {
                    sb.AppendLine($"if({objName}.{property.IdentifierName}?.Length > 0)");
                    sb.AppendLine($"resultObj.{property.IdentifierName} = {objName}.{property.IdentifierName}.ToByteArray();");
                    continue;
                }
                if (property.Type is not null && property.Type!.StartsWith("SMFields.O2") && property.RelationshipWith is not null)
                {
                    sb.AppendLine($"resultObj.{identifer}_ID = {objName}.{identifer}ID;");
                    continue;
                }
                if (property.Type is not null && property.Type!.StartsWith("SMFields.M2") && property.RelationshipWith is not null)
                {
                    sb.AppendLine($"if({objName}.{identifer.Pluralize()}IDs?.Count > 0){{");
                    sb.AppendLine($"resultObj.{identifer}_IDs = new ();");
                    sb.AppendLine($"resultObj.{identifer}_IDs.AddRange({objName}.{identifer}IDs);");
                    sb.AppendLine($"}}");
                    continue;
                }
                var type = GetProtoType(property!);
                if (type is null) continue;
                if (type is "google.protobuf.Timestamp")
                    sb.AppendLine($"resultObj.{identifer} = {objName}.{identifer} is not null ? {objName}.{identifer}.ToDateTime() : default" +
                        $"" +
                        $"" +
                        $"" +
                        $"" +
                        $"" +
                        $"" +
                        $";");
                else
                    sb.AppendLine($"resultObj.{identifer} = {objName}.{identifer};");
            }
            tempModelCT = tempModelCT.ParentClassType as ModelCT;
        }
        //Debug.Assert(false, sb.ToString());
        return sb.ToString();
    }

    internal static string ModifiedOperatorProperties(ModelCT s, string objName, bool allowComputedValues = true, bool addCreatedOn = false, bool addLastModifiedOn = false)
    {
        StringBuilder sb = new();
        var tempModelCT = s;

        while (tempModelCT is not null)
        {
            foreach (var property in tempModelCT.Properties!)
            {
                var identifer = property!.IdentifierName;
                if (!allowComputedValues)
                {
                    if (property.SMField is not null && property.SMField.Field is not null)
                        if (property.SMField.Field.Compute)
                            continue;

                }
                if (property.Type is not null and ("SMFields.Binary" or "SMFields.Binary?"))
                {
                    sb.AppendLine($"if({objName}.{property.IdentifierName}?.Length > 0)");
                    sb.AppendLine($"\t\t\t\tresultObj.{property.IdentifierName} =Google.Protobuf.ByteString.CopyFrom( {objName}.{property.IdentifierName});");
                    continue;
                }
                if (property.Type is not null && property.Type!.StartsWith("SMFields.O2") && property.RelationshipWith is not null)
                {
                    sb.AppendLine($"resultObj.{identifer}ID = {objName}.{identifer}_ID;");
                    continue;
                }

                if (property.Type is not null && property.Type!.StartsWith("SMFields.M2") && property.RelationshipWith is not null)
                {
                    sb.AppendLine($"if({objName}.{identifer}_IDs?.Count > 0){{");
                    sb.AppendLine($"resultObj.{identifer}IDs.AddRange({objName}.{identifer}_IDs);");

                    sb.AppendLine($"}}");
                    continue;
                }

                var type = GetProtoType(property!);
                if (type is null) continue;
                if (type is "google.protobuf.Timestamp")
                    sb.AppendLine($"resultObj.{identifer} = {objName}.{identifer} is System.DateTime ? DateTime.SpecifyKind((System.DateTime) {objName}.{identifer}, DateTimeKind.Utc).ToTimestamp() : null;");
                else
                    sb.AppendLine($"\t\t\tresultObj.{identifer} = {objName}.{identifer};");
            }
            tempModelCT = tempModelCT.ParentClassType as ModelCT;
        }
        //Debug.Assert(false, sb.ToString());
        return sb.ToString();
    }


    internal static object AddEntityImplicitOperatorProperties(ModelCT s, string objName, bool allowComputedValues = true, bool addCreatedOn = false, bool addLastModifiedOn = false)
    {
        StringBuilder sb = new();
        var tempModelCT = s;

        while (tempModelCT is not null)
        {
            foreach (var property in tempModelCT.Properties!)
            {
                var identifer = property!.IdentifierName;
                if (!allowComputedValues)
                {
                    if (property.SMField is not null && property.SMField.Field is not null)
                        if (property.SMField.Field.Compute)
                            continue;

                }
                if (property.Type is not null and ("SMFields.Binary" or "SMFields.Binary?"))
                {
                    sb.AppendLine($"if({objName}.{property.IdentifierName}?.Length > 0)");
                    sb.AppendLine($"resultObj.{property.IdentifierName} = {objName}.{property.IdentifierName}.ToByteArray();");
                    continue;
                }
                if (property.Type is not null && property.Type!.StartsWith("SMFields.M2") && property.RelationshipWith is not null)
                {
                    sb.AppendLine($"if({objName}.{identifer.Pluralize()}?.Count > 0){{");
                    sb.AppendLine($"resultObj.{identifer.Pluralize()} = new List<{(property.RelationshipWith.WithRelationship.ClassType as ModelCT)!.NewQualifiedName}>();");
                    sb.AppendLine(@$"foreach (var ro in {objName}.{identifer.Pluralize()})
                                            {{
{(property.RelationshipWith.WithRelationship.ClassType as ModelCT)!.NewQualifiedName} tempObj = ro;
");
                    if (addCreatedOn)
                        sb.AppendLine($"tempObj.CreatedOn = DateTime.Now;");
                    if (addLastModifiedOn)
                        sb.AppendLine($"tempObj.LastModifiedOn = DateTime.Now;");
                    sb.AppendLine($" resultObj.{identifer.Pluralize()}.Add(tempObj);");
                    sb.AppendLine($"}}}}");
                    continue;
                }
                //                else if (property.Type is not null && property.Type!.StartsWith("System.Collections.Generic") && property.HasRelation is not null)
                //                {
                //                    sb.AppendLine($"if({objName}.{identifer.Replace("_", "").Pluralize()}?.Count > 0){{");
                //                    sb.AppendLine($"resultObj.{identifer} = new List<{(property.HasRelation.HasRelation.ClassType as ModelCT)!.NewQualifiedName}>();");
                //                    sb.AppendLine(@$"foreach (var ro in {objName}.{identifer.Replace("_", "").Pluralize()})
                //                                            {{
                //{(property.HasRelation.HasRelation.ClassType as ModelCT)!.NewQualifiedName} tempObj = ro;
                //");
                //                    if (addCreatedOn)
                //                        sb.AppendLine($"tempObj.CreatedOn = DateTime.Now;");
                //                    if (addLastModifiedOn)
                //                        sb.AppendLine($"tempObj.LastModifiedOn = DateTime.Now;");
                //                    sb.AppendLine($" resultObj.{identifer}.Add(tempObj);");
                //                    sb.AppendLine($"}}}}");
                //                    continue;
                //                }
                var type = GetProtoType(property!);
                if (type is null) continue;
                if (type is "google.protobuf.Timestamp")
                    sb.AppendLine($"resultObj.{identifer} = {objName}.{identifer} is not null ? {objName}.{identifer}.ToDateTime() : default;");
                else
                    sb.AppendLine($"resultObj.{identifer} = {objName}.{identifer};");
            }
            tempModelCT = tempModelCT.ParentClassType as ModelCT;
        }
        //Debug.Assert(false, sb.ToString());
        return sb.ToString();
    }

    internal static string AddOperatorProperties(ModelCT s, string objName, bool allowComputedValues = true, bool addCreatedOn = false, bool addLastModifiedOn = false)
    {
        StringBuilder sb = new();
        var tempModelCT = s;

        while (tempModelCT is not null)
        {
            foreach (var property in tempModelCT.Properties!)
            {
                var identifer = property!.IdentifierName;
                if (!allowComputedValues)
                {
                    if (property.SMField is not null && property.SMField.Field is not null)
                        if (property.SMField.Field.Compute)
                            continue;

                }
                if (property.Type is not null and ("SMFields.Binary" or "SMFields.Binary?"))
                {
                    sb.AppendLine($"if({objName}.{property.IdentifierName}?.Length > 0)");
                    sb.AppendLine($"\t\t\t\tresultObj.{property.IdentifierName} =Google.Protobuf.ByteString.CopyFrom( {objName}.{property.IdentifierName});");
                    continue;
                }
                if (property.Type is not null && property.Type!.StartsWith("SMFields.M2") && property.RelationshipWith is not null)
                {
                    sb.AppendLine($"if({objName}.{identifer.Pluralize()}?.Count > 0){{");
                    sb.AppendLine(@$"foreach (var ro in {objName}.{identifer.Pluralize()})
                                     {{
                                     {(property.RelationshipWith.WithRelationship.ClassType as ModelCT)!.NewQualifiedName} tempObj = ro;
 ");
                    if (addCreatedOn)
                        sb.AppendLine($"tempObj.CreatedOn = DateTime.Now;");
                    if (addLastModifiedOn)
                        sb.AppendLine($"tempObj.LastModifiedOn = DateTime.Now;");
                    sb.AppendLine($" resultObj.{identifer.Pluralize()}.Add(tempObj);");
                    sb.AppendLine($"}}}}");
                    continue;
                }
                //                else if (property.Type is not null && property.Type!.StartsWith("System.Collections.Generic") && property.HasRelation is not null)
                //                {
                //                    sb.AppendLine($"if({objName}.{identifer}?.Count > 0){{");
                //                    sb.AppendLine(@$"foreach (var ro in {objName}.{identifer})
                //                                            {{
                //{(property.HasRelation.HasRelation.ClassType as ModelCT)!.NewQualifiedName} tempObj = ro;
                //");
                //                    if (addCreatedOn)
                //                        sb.AppendLine($"tempObj.CreatedOn = DateTime.Now;");
                //                    if (addLastModifiedOn)
                //                        sb.AppendLine($"tempObj.LastModifiedOn = DateTime.Now;");
                //                    sb.AppendLine($" resultObj.{identifer.Replace("_", "").Pluralize()}.Add(tempObj);");
                //                    sb.AppendLine($"}}}}");
                //                    continue;
                //                }

                var type = GetProtoType(property!);
                if (type is null) continue;
                if (type is "google.protobuf.Timestamp")
                    sb.AppendLine($"resultObj.{identifer} = {objName}.{identifer} is System.DateTime ? DateTime.SpecifyKind((System.DateTime) {objName}.{identifer}, DateTimeKind.Utc).ToTimestamp() : null;");
                else
                    sb.AppendLine($"\t\t\tresultObj.{identifer} = {objName}.{identifer};");
            }
            tempModelCT = tempModelCT.ParentClassType as ModelCT;
        }
        //Debug.Assert(false, sb.ToString());
        return sb.ToString();
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
            if (property.HasRelation is not null)
            {
                if (!property.Type.StartsWith("System.Collections.Generic"))
                    messages.AppendLine($"\t{(property.HasRelation.HasRelation.ClassType as ModelCT)!.ModuleNameWithoutPostFix}_{(property.HasRelation.HasRelation.ClassType as ModelCT)!.IdentifierNameWithoutPostFix} {identifer} = {i};");
                else
                    messages.AppendLine($"\trepeated {(property.HasRelation.HasRelation.ClassType as ModelCT)!.ModuleNameWithoutPostFix}_{(property.HasRelation.HasRelation.ClassType as ModelCT)!.IdentifierNameWithoutPostFix} {identifer.Pluralize()} = {i};");

                i++;
                continue;
            }
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
            if (property.Type is "SMFields.O2O" or "SMFields.O2M")
            {
                messages.AppendLine($"int32 {identifer}_ID = {i};");
                i++;
                continue;
            }

            if (property.Type is "SMFields.M2O" or "SMFields.M2M")
            {
                messages.AppendLine($"\t\t\trepeated int32 {identifer}_IDs = {i};");
                i++;
                continue;
            }
            var type = GetProtoType(property!);
            if (type is null) continue;

            if (property.SMField is not null && property.SMField.Field is not null)
                if (property.SMField.Field.Compute)
                    continue;
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
            if (property.Type is "SMFields.O2O" or "SMFields.O2M")
            {
                messages.AppendLine($"int32 {identifer}_ID = {i};");
                i++;
                continue;
            }

            if (property.Type is "SMFields.M2O" or "SMFields.M2M")
            {
                messages.AppendLine($"repeated int32 {identifer}_IDs = {i};");
                i++;
                continue;
            }

            var type = GetProtoType(property!);
            if (type is null) continue;
            if (type!.StartsWith("repeated"))
                identifer = identifer.Pluralize();

            if (property.SMField is not null && property.SMField.Field is not null)
                if (property.SMField.Field.Compute)
                    continue;
            messages.AppendLine($"\t{type} {identifer} = {i};");
            i++;
        }
    }
}