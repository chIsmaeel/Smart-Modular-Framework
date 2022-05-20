namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.InterfaceMemberTemplate;

using SMF.SourceGenerator.Core.Templates.Interfaces;

public record MethodInterfaceTemplate(string Type, string IdentifierName) : InterfaceMemberTemplate(Type, IdentifierName)
{
    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    public List<(string, string)>? Parameters { get; init; } = new();

    /// <summary>
    /// Creates the template.
    /// </summary>
    /// <returns>An ITemplate.</returns>
    public override ITemplate CreateTemplate()
    {
        base.CreateTemplate();
        _indentedTextWriter!.Write(Type);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(IdentifierName);
        if (GenericParameters!.Count > 0)
        {
            _indentedTextWriter.Write('<');
            _indentedTextWriter.Write(string.Join(", ", GenericParameters));
            _indentedTextWriter.Write('>');
        }
        _indentedTextWriter.Write('(');
        _indentedTextWriter.Write(string.Join(", ", Parameters.Select(_ => _.Item1 + " " + _.Item2)));
        _indentedTextWriter.Write(')');
        _indentedTextWriter.Write(';');
        _indentedTextWriter!.Indent--;
        return this;
    }
}
