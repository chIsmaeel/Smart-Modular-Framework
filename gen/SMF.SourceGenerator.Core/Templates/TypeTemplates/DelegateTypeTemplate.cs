namespace SMF.SourceGenerator.Core.Templates.TypeTemplates;

using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.Interfaces;
using System.CodeDom.Compiler;
using System.Text;

/// <summary>
/// Strongly typed template for the class.
/// </summary>
/// <param name="Type"></param>
/// <param name="IdentifierName"></param>
public record DelegateTypeTemplate(string Type, string IdentifierName) : ITypeTemplate
{
    private StringBuilder? _stringBuilder;
    private StringWriter? _stringWriter;
    private IndentedTextWriter? _indentedTextWriter;
    /// <summary>
    /// Gets the type.
    /// </summary>
    public string Type { get; init; } = Type;

    /// <summary>
    /// Gets the generic parameters.
    /// </summary>
    public List<string>? GenericParameters { get; init; } = new();

    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    public List<(string, string)>? Parameters { get; } = new();
    /// <summary>
    /// Gets the modifiers.
    /// </summary>
    public string Modifiers { get; init; } = "public";
    /// <summary>
    /// Gets the type attributes.
    /// </summary>
    public List<string>? Attributes { get; init; } = new();
    /// <summary>
    /// Gets the type comment.
    /// </summary>
    public string? Comment { get; init; }
    /// <summary>
    /// Gets the using namespaces.
    /// </summary>
    public List<string> UsingNamespaces { get; init; } = new();
    /// <summary>
    /// Gets the members.
    /// </summary>
    public List<ITypeMemberTemplate> Members { get; } = new();
    /// <summary>
    /// Gets the string members.
    /// </summary>
    public List<string> StringMembers { get; init; } = new();
    /// <summary>
    /// Gets the members.
    /// </summary>
    List<IMemberTemplate> ITypeTemplate.Members { get; } = new();

    /// <summary>
    /// Creates the template.
    /// </summary>
    /// <returns>A TypeMemberTemplateBase.</returns>
    public ITypeTemplate CreateTemplate()
    {
        _stringBuilder = new();
        _stringWriter = new(_stringBuilder);
        _indentedTextWriter = new(_stringWriter);

        _indentedTextWriter!.Write(Modifiers);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter!.Write("delegate");
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
        //_indentedTextWriter.WriteLine();
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
