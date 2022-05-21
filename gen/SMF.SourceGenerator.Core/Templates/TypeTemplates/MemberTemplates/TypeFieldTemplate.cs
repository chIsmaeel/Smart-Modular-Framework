namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates;
public record TypeFieldTemplate(string Type, string IdentifierName) : TypeMemberTemplate(IdentifierName)
{
    /// <summary>
    /// Gets or sets the default value.
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Creates the template.
    /// </summary>
    /// <returns>A MemberTemplateBase.</returns>
    public override ITemplate CreateTemplate()
    {
        base.CreateTemplate();
        WritePrivateFieldOfProperty();
        return this;
    }

    /// <summary>
    /// Writes the private field of property.
    /// </summary>
    private void WritePrivateFieldOfProperty()
    {
        _indentedTextWriter!.Write("private");
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(Type);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(IdentifierName);
        if (DefaultValue is not null)
        {
            _indentedTextWriter.Write(' ');
            _indentedTextWriter.Write('=');
            _indentedTextWriter.Write(' ');
            _indentedTextWriter.Write(DefaultValue);
        }
        _indentedTextWriter.WriteLine(';');
    }
}
