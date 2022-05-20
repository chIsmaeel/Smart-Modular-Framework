namespace SMF.SourceGenerator.Core.Templates.TypeTemplates;

using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.InterfaceMemberTemplate;
using System.Text;

public record InterfaceTemplate(string IdentifierName) : TypeTemplate(IdentifierName)
{
    /// <summary>
    /// Gets the type.
    /// </summary>
    public override string Type => "interface";

    /// <summary>
    /// Gets the members.
    /// </summary>
    /// <returns>A string.</returns>
    protected override string GetMembers()
    {
        // Add type Field Members.
        StringBuilder memberSB = new();

        // String Members.
        StringMembers.ForEach(_ => memberSB.AppendLine(_));

        // Add Type Auto Property in Members.
        Members.Where(_ => _ is PropertyInterfaceTemplate)
           .ForEach(_ =>
           {
               memberSB.AppendLine(_.CreateTemplate().GetTemplate());
               if (_.UsingNamespaces?.Count > 0)
                   UsingNamespaces.AddRange(_.UsingNamespaces);
           });

        Members.Where(_ => _ is MethodInterfaceTemplate)
          .ForEach(_ =>
          {
              memberSB.AppendLine(_.CreateTemplate().GetTemplate());
              if (_.UsingNamespaces?.Count > 0)
                  UsingNamespaces.AddRange(_.UsingNamespaces);
          });

        return "    " + memberSB.ToString().Trim();
    }
}
