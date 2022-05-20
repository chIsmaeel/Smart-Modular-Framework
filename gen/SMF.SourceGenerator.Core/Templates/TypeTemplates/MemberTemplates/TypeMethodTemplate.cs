namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates;

using System.CodeDom.Compiler;

public record TypeMethodTemplate(string Type, string IdentifierName) : TypeMemberTemplate(IdentifierName)
{
    /// <summary>
    /// Gets the generic parameters.
    /// </summary>
    public List<string>? GenericParameters { get; init; }

    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    public List<(string, string)>? Parameters { get; init; }

    /// <summary>
    /// Gets a value indicating whether is abstract.
    /// </summary>
    public virtual bool IsAbstract { get; } = false;
    /// <summary>
    /// Gets or sets the body.
    /// </summary>
    public Action<IndentedTextWriter, List<(string, string)>?, List<string>?, IEnumerable<TypeFieldTemplate>>? Body { get; set; }
    /// <summary>
    /// Creates the method template.
    /// </summary>
    /// <returns>A string.</returns>
    public ITemplate CreateTemplate(IEnumerable<TypeFieldTemplate> fieldTemplates)
    {
        base.CreateTemplate();
        WriteCommentAndAttributes();
        if (IsSubMemberofOtherType)
            _indentedTextWriter!.Indent++;
        _indentedTextWriter!.Write(Modifiers);
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

        if (IsAbstract)
        {
            _indentedTextWriter.WriteLine(";");
            _indentedTextWriter.WriteLine();
            return this;
        }

        _indentedTextWriter.WriteLine();
        _indentedTextWriter.WriteLine("{");

        if (Body is not null)
        {
            _indentedTextWriter.Indent++;
            Body(_indentedTextWriter, Parameters, GenericParameters, fieldTemplates);
            _indentedTextWriter.Indent--;
        }

        _indentedTextWriter.WriteLine("}");

        //_indentedTextWriter.WriteLine();

        return this;
    }
}
