namespace SMF.ApplicationLayer.SourceGenerator;

using SMF.SourceGenerator.Core;
using SMF.SourceGenerator.Core.Templates.TypeTemplates;
using System.CodeDom.Compiler;
/// <summary>
/// The static methods.
/// </summary>

internal class StaticMethods
{
    /// <summary>
    /// Adds the properties.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="classTypeTemplate">The class type template.</param>
    public static void AddProperties(ModelCT s, ClassTypeTemplate classTypeTemplate)
    {
        AutoPropertyTemplate autoProp;
        foreach (var property in s.Properties!)
        {

            var identifer = property!.IdentifierName;
            if (property.Type is "SMFields.O2O" or "SMFields.O2M")
            {
                autoProp = new("int", identifer + "_ID")
                {
                    Comment = property.Comment,
                    SecondAccessor = "set",
                };
                classTypeTemplate.Members.Add(autoProp);
                continue;
            }

            if (property.Type is "SMFields.M2O" or "SMFields.M2M")
            {
                autoProp = new("System.Collections.Generic.List<int>", identifer + "_IDs")
                {
                    Comment = property.Comment,
                    SecondAccessor = "set",
                    DefaultValue = "new()"
                };
                classTypeTemplate.Members.Add(autoProp);
                continue;
            }

            var type = ModelPropertyTypes.GetPropertyType(property!);
            if (type is null) continue;
            if (property.SMField is not null && property.SMField.Field is not null)
            {
                if (property.SMField.Field.Compute)
                    continue;
            }
            autoProp = new AutoPropertyTemplate(type, identifer)
            {
                Comment = property.Comment,
                SecondAccessor = "set",
            };
            classTypeTemplate.Members.Add(autoProp);

        }
    }


    /// <summary>
    /// Adds the properties.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="w">The w.</param>
    /// <param name="objName">The obj name.</param>
    public static void AddProperties(ModelCT s, IndentedTextWriter w, string? objName)
    {
        foreach (var property in s.Properties!)
        {
            if (property!.IdentifierName.EndsWith("_FK"))
                continue;
            if (property!.IdentifierName is "Id" or "UpdatedOn")
                continue;
            if (property.Type is "SMFields.O2O" or "SMFields.O2M")
            {
                w.WriteLine($"{objName}.{property!.IdentifierName} =  await _uow.{(property.RelationshipWith!.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix}_{(property.RelationshipWith!.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}Repository.GetByIdAsync(command.{property!.IdentifierName}_ID);");

                continue;
            }

            if (property.Type is "SMFields.M2O" or "SMFields.M2M")
            {
                w.WriteLine($$"""
  foreach (var id in command.{{property!.IdentifierName}}_IDs)
            {
                var obj = await _uow.{{(property.RelationshipWith!.WithRelationship.ClassType as ModelCT)!.ModuleNameWithoutPostFix}}_{{(property.RelationshipWith!.WithRelationship.ClassType as ModelCT)!.IdentifierNameWithoutPostFix}}Repository.GetByIdAsync(id);
                if (obj is null) continue;
                {{objName}}.{{property!.IdentifierName}}.Add(obj);

            }
""");
                continue;
            }
            if (property.SMField is not null && property.SMField.Field is not null && property.SMField.Field.Compute)
            {
                //w.WriteLine($"{objName}.{identifer} = Compute{property.IdentifierName}(_uow);");
                continue;
            }
            w.WriteLine($"{objName}.{property!.IdentifierName} = command.{property!.IdentifierName};");
        }
    }
}
