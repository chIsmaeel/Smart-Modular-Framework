namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.InterfaceMemberTemplate;

using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.Interfaces;
using System.CodeDom.Compiler;
using System.Text;

public record InterfaceMemberTemplate(string Type, string IdentifierName) : IMemberTemplate
{
    protected StringBuilder? stringBuilder;
    protected StringWriter? _stringWriter;
    protected IndentedTextWriter? _indentedTextWriter;

    /// <summary>
    /// Gets the generic parameters.
    /// </summary>
    public List<string>? GenericParameters { get; init; } = new();

    /// <summary>
    /// Gets or sets the members.
    /// </summary>
    public List<InterfaceMemberTemplate> Members { get; } = new();
    /// <summary>
    /// Gets the comment.
    /// </summary>
    public string? Comment { get; init; }
    /// <summary>
    /// Gets the using namespaces.
    /// </summary>
    public List<string> UsingNamespaces { get; init; } = new();
    /// <summary>
    /// Gets or sets the parent.
    /// </summary>
    public ITemplate? Parent { get; set; }

    /// <summary>
    /// Creates the template.
    /// </summary>
    /// <returns>An ITemplate.</returns>
    public virtual ITemplate CreateTemplate()
    {
        stringBuilder = new();
        _stringWriter = new(stringBuilder);
        _indentedTextWriter = new(_stringWriter);
        WriteComment();
        return this;
    }

    /// <summary>
    /// Writes the comment.
    /// </summary>
    protected virtual void WriteComment()
    {

        if (string.IsNullOrWhiteSpace(Comment))
            _indentedTextWriter!.WriteLine("    " + CommentTemplate.CreateCommentFromIdentifierName(IdentifierName));
        else
            _indentedTextWriter!.WriteLine("    " + CommentTemplate.CreateCommentFromText(Comment!));
    }

    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <returns>A string.</returns>
    public string GetTemplate()
    {
        return _stringWriter!.ToString();
    }
}
