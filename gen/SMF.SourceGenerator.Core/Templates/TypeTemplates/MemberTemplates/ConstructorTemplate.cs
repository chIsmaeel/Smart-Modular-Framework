namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates;

using SMF.SourceGenerator.Core.Templates.Interfaces;
using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.Interfaces;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

public record ConstructorTemplate(string IdentifierName) : IMemberTemplate
{
    protected StringBuilder _stringBuilder = new();
    protected StringWriter? _stringWriter;
    protected IndentedTextWriter? _indentedTextWriter;

    /// <summary>
    /// Gets the using namespaces.
    /// </summary>
    public List<string> UsingNamespaces { get; init; } = new();

    /// <summary>
    /// Gets or sets the base constructor.
    /// </summary>
    public ConstructorTemplate? BaseConstructor { get; set; }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    public List<(string type, string parameterName)> Parameters { get; init; } = new();


    /// <summary>
    /// Gets the comment.
    /// </summary>
    public string? Comment { get; init; }
    /// <summary>
    /// Gets or sets the body.
    /// </summary>
    public Action<IndentedTextWriter, List<(string, string)>?>? Body { get; set; }

    /// <summary>
    /// Gets or sets the parent.
    /// </summary>
    public ITemplate? Parent { get; set; }
    /// <summary>
    /// Gets a value indicating whether sub memberof other is type.
    /// </summary>
    public bool IsSubMemberofOtherType { get; init; } = false;


    /// <summary>
    /// Creates the template.
    /// </summary>
    /// <returns>An ITemplate.</returns>
    public ITemplate CreateTemplate()
    {
        _stringWriter = new(_stringBuilder);
        _indentedTextWriter = new(_stringWriter);


        if (IsSubMemberofOtherType)
            _indentedTextWriter.Indent++;
        WriteComment();
        _indentedTextWriter.Indent++;
        _indentedTextWriter.Write($"public {IdentifierName}({string.Join(", ", Parameters.Select(p => $"{p.type} {p.parameterName}"))})");
        if (BaseConstructor != null)
        {
            _indentedTextWriter.Write($" : base({string.Join(", ", Parameters.Select(p => $"{p.parameterName}"))})");
        }
        _indentedTextWriter.WriteLine();
        _indentedTextWriter.WriteLine("{");
        if (Body is not null)
        {
            _indentedTextWriter.Indent++;
            Body(_indentedTextWriter, Parameters);
            _indentedTextWriter.Indent--;
        }
        _indentedTextWriter.WriteLine("}");
        _indentedTextWriter.Indent--;


        return this;
    }

    /// <summary>
    /// Writes the comment.
    /// </summary>
    protected virtual void WriteComment()
    {
        _indentedTextWriter!.Write("    ");
        if (IsSubMemberofOtherType)
            _indentedTextWriter.Write("    ");
        if (string.IsNullOrWhiteSpace(Comment))
            _indentedTextWriter!.WriteLine(CommentTemplate.CreateCommentFromIdentifierName(IdentifierName));
        else
            _indentedTextWriter!.WriteLine(CommentTemplate.CreateCommentFromText(Comment!));
    }

    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <returns>A string.</returns>
    public string GetTemplate()
    {
        if (IsSubMemberofOtherType)
            _indentedTextWriter!.Indent--;
        return _stringWriter!.ToString();
    }
}
