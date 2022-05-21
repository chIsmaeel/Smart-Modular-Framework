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
}
