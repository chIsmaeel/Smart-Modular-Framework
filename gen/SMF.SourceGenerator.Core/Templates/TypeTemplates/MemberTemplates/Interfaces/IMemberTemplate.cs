namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.Interfaces;
/// <summary>
/// IMemberTemplate
/// </summary>
public interface IMemberTemplate : ITemplate
{
    /// <summary>
    /// Gets or sets the parent.
    /// </summary>
    ITemplate? Parent { get; set; }

    /// <summary>
    /// Gets a value indicating whether sub memberof other is type.
    /// </summary>
    bool IsSubMemberofOtherType { get; init; }
}
