namespace SMF.ApplicationLayer.SourceGenerator;

using Humanizer;
using SMF.SourceGenerator.Core;
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
        foreach (var property in s.Properties!)
        {
            var identifer = property!.IdentifierName;
            var type = ModelPropertyTypes.GetPropertyType(property!);
            if (type.StartsWith("System.Collections.Generic"))
                identifer = identifer.Pluralize();

            if (property.SMField is not null && property.SMField.Field is not null)
            {
                if (property.SMField.Field.Compute)
                    continue;
            }
            var autoProp = new AutoPropertyTemplate(type, identifer)
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
            if (property!.IdentifierName is "Id" or "UpdatedOn")
                continue;
            if (property.SMField is not null && property.SMField.Field is not null && property.SMField.Field.Compute)
            {
                //w.WriteLine($"{objName}.{identifer} = Compute{property.IdentifierName}(_uow);");
                continue;
            }
            w.WriteLine($"{objName}.{property!.IdentifierName} = command.{property!.IdentifierName};");
        }
    }
}
