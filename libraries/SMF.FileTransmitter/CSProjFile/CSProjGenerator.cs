namespace SMF.FileTransmitter.CSProjFile;

using System.Text;
/// <summary>
/// The reference type.
/// </summary>

public enum ReferenceType
{
    Project,
    Package
}
/// <summary>
/// The reference.
/// </summary>
public class CSProjGenerator
{

    /// <summary>
    /// Gets the properties.
    /// </summary>
    /// <param name="properties">The properties.</param>
    /// <returns>A string.</returns>
    public static string GetProperties(List<CSProjProperties> properties)
    {
        StringBuilder sb = new();
        foreach (var (propertyName, value) in properties)
        {
            sb.Append("        ");
            sb.Append('<');
            sb.Append(propertyName);
            sb.Append('>');
            sb.Append(value);
            sb.Append("</");
            sb.Append(propertyName);
            sb.AppendLine(">");

        }
        return sb.ToString().TrimEnd() + "\n";
    }

    /// <summary>
    /// Gets the references.
    /// </summary>
    /// <param name="references">The references.</param>
    /// <param name="extraInfo">The extra info.</param>
    /// <returns>A string.</returns>
    public static string GetReferences(List<References> references)
    {
        StringBuilder sb = new();
        foreach (var (reference, type, extraInfo) in references)
        {
            sb.Append("        ");
            sb.Append('<');
            sb.Append(type == ReferenceType.Project ? "Project" : "Package");
            sb.Append("Reference");
            sb.Append(' ');
            sb.Append("Include = \"");
            sb.Append(reference);
            sb.Append('"');
            sb.Append(' ');
            if (extraInfo.Any())
                foreach (var (key, value) in extraInfo)
                    if (key is not null && value is not null)
                    {
                        sb.Append(' ');
                        sb.Append(key);
                        sb.Append(" = \"");
                        sb.Append(value);
                        sb.Append('"');
                    }
            sb.AppendLine(" />");


        }
        return sb.ToString().TrimEnd() + "\n";
    }

    /// <summary>
    /// Templates the.
    /// </summary>
    /// <param name="properties">The properties.</param>
    /// <param name="references">The references.</param>
    public static string Template(string properties, string references, string version = "net6.0", string? extraInfo = null)
    {
        return
@$"<Project Sdk=""Microsoft.NET.Sdk"">                 

    <PropertyGroup>
        <TargetFramework>{version}</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>

{properties}
    </PropertyGroup>

    <ItemGroup>
{references}
{extraInfo}
    </ItemGroup>

</Project>
";
    }
}
