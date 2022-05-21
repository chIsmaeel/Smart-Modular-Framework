namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.InterfaceMemberTemplate;

using SMF.SourceGenerator.Core.Templates.Interfaces;

public record PropertyInterfaceTemplate(string Type, string IdentifierName) : InterfaceMemberTemplate(Type, IdentifierName)
{
    /// <summary>
    /// Gets the first accessor.
    /// </summary>
    public string FirstAccessor { get; init; } = "get";
    /// <summary>
    /// Gets the second accessor.
    /// </summary>
    public string? SecondAccessor { get; init; }

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

        _indentedTextWriter.Write('{');
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(FirstAccessor);
        if (SecondAccessor is not null)
        {
            _indentedTextWriter.Write(';');
            _indentedTextWriter.Write(' ');
            _indentedTextWriter.Write(SecondAccessor);
        }
        _indentedTextWriter.Write(';');
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write('}');
        return this;
    }
}
