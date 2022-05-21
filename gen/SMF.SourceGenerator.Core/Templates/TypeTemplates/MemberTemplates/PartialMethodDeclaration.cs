namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates;

using SMF.SourceGenerator.Core.Templates.Interfaces;
using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.Interfaces;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// The partial method declaration.
/// </summary>

public record PartialMethodTemplate(string Type, string IdentifierName) : IMemberTemplate
{
    protected StringBuilder _stringBuilder = new();
    protected StringWriter? _stringWriter;
    protected IndentedTextWriter? _indentedTextWriter;


    /// <summary>
    /// Gets the modifier.
    /// </summary>
    public string Modifiers { get; init; } = "public";

    /// <summary>
    /// Gets or sets the parent.
    /// </summary>
    public ITemplate? Parent { get; set; }

    /// <summary>
    /// Gets the using namespaces.
    /// </summary>
    public List<string> UsingNamespaces { get; init; } = new();

    /// <summary>
    /// Gets the generic parameters.
    /// </summary>
    public List<string>? GenericParameters { get; init; }

    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    public List<(string, string)>? Parameters { get; init; }



    /// <summary>
    /// Gets the comment.
    /// </summary>
    public string? Comment { get; init; }

    /// <summary>
    /// Creates the template.
    /// </summary>
    /// <returns>An ITemplate.</returns>
    public ITemplate CreateTemplate()
    {
        _stringWriter = new(_stringBuilder);
        _indentedTextWriter = new(_stringWriter);


        TypeMemberTemplate.WriteComment(Comment, IdentifierName, _indentedTextWriter);
        _indentedTextWriter!.Write(Modifiers);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write("partial");
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(Type);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(IdentifierName);

        if (GenericParameters?.Count > 0)
            _indentedTextWriter.Write($"<{string.Join(", ", GenericParameters)}>");
        _indentedTextWriter.Write('(');
        if (Parameters?.Count > 0)
            _indentedTextWriter.Write(string.Join(", ", Parameters.Select(p => $"{p.Item1} {p.Item2}")));
        _indentedTextWriter.Write(')');
        _indentedTextWriter.WriteLine(";");
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
