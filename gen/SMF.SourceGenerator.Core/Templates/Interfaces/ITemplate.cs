namespace SMF.SourceGenerator.Core.Templates.Interfaces;
/// <summary>
/// This class represents the interface template.
/// </summary>
public interface ITemplate
{
    /// <summary>
    /// Gets the using namespaces.
    /// </summary>
    List<string> UsingNamespaces { get; init; }
    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    string IdentifierName { get; init; }




    /// <summary>
    /// Gets the type comment.
    /// </summary>
    string? Comment { get; init; }

    /// <summary>
    /// Creates the template.
    /// </summary>
    /// <returns>A TypeTemplate.</returns>
    ITemplate CreateTemplate();
    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <returns>A string.</returns>
    string GetTemplate();
}
