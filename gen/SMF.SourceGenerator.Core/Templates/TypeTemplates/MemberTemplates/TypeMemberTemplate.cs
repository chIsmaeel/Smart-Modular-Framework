namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates;

using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.Interfaces;
using System.CodeDom.Compiler;
using System.Text;

public abstract record TypeMemberTemplate(string IdentifierName) : ITypeMemberTemplate
{
    protected StringBuilder _stringBuilder = new();
    protected StringWriter? _stringWriter;
    protected IndentedTextWriter? _indentedTextWriter;

    /// <summary>
    /// Gets the modifier.
    /// </summary>
    public string Modifiers { get; init; } = "public";

    /// <summary>                                     
    /// Gets the comment.
    /// </summary>
    public string? Comment { get; init; }

    /// <summary>
    /// Gets the attributes.
    /// </summary>
    public List<string>? Attributes { get; init; } = new();

    /// <summary>
    /// Gets the using namespaces.
    /// </summary>
    public List<string> UsingNamespaces { get; init; } = new();
    /// <summary>
    /// Gets the type.
    /// </summary>
    public abstract string Type { get; init; }

    /// <summary>
    /// Gets or sets the parent.
    /// </summary>
    public ITemplate? Parent { get; set; }

    /// <summary>
    /// Adds the comment and attributes.
    /// </summary>
    protected void WriteAttributes()
    {

        if (Attributes?.Count > 0)
            foreach (var attribute in Attributes)
            {
                _indentedTextWriter!.Write('[');
                _indentedTextWriter.Write(attribute);
                _indentedTextWriter.WriteLine("]");
            }
    }

    /// <summary>
    /// Writes the comment and attributes.
    /// </summary>
    protected void WriteCommentAndAttributes()
    {
        WriteComment(Comment, IdentifierName, _indentedTextWriter);
        WriteAttributes();
    }

    /// <summary>
    /// Writes the comment.
    /// </summary>
    public static void WriteComment(string? comment, string identifierName, IndentedTextWriter? w)
    {
        if (string.IsNullOrWhiteSpace(comment))
            w!.WriteLine(CommentTemplate.CreateCommentFromIdentifierName(identifierName));
        else
            w!.WriteLine(CommentTemplate.CreateCommentFromText(comment!));
    }

    /// <summary>
    /// Creates the method template.
    /// </summary>
    public virtual ITemplate CreateTemplate()
    {
        _stringWriter = new(_stringBuilder);
        _indentedTextWriter = new(_stringWriter);

        return this;
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