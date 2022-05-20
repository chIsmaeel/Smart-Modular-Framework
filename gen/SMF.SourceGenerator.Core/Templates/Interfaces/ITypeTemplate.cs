namespace SMF.SourceGenerator.Core.Templates.Interfaces;

using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.Interfaces;

/// <summary>
/// The type template.
/// </summary>

public interface ITypeTemplate
{
    /// <summary>
    /// Gets the type.
    /// </summary>
    string Type { get; init; }

    /// <summary>
    /// Gets the modifiers.
    /// </summary>
    string Modifiers { get; init; }

    /// <summary>
    /// Gets the type attributes.
    /// </summary>
    List<string>? Attributes { get; init; }
    /// <summary>
    /// Gets the members.
    /// </summary>
    List<IMemberTemplate> Members { get; }

    /// <summary>
    /// Gets the string members.
    /// </summary>
    List<string> StringMembers { get; }
}