namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.Interfaces;
/// <summary>
/// The type member template.
/// </summary>

public interface ITypeMemberTemplate : IMemberTemplate
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

}