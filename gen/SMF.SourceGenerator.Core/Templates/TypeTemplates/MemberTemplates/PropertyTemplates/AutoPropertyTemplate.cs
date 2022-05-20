namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.PropertyTemplates;

public record AutoPropertyTemplate(string Type, string IdentifierName) : PropertyTemplate(Type, IdentifierName)
{
    /// <summary>
    /// Creates the property template.
    /// </summary>
    public override ITemplate CreateTemplate()
    {
        base.CreateTemplate();
        WriteCommentAndAttributes();
        WritePropertyDeclaration();
        _indentedTextWriter!.Write(' ');
        _indentedTextWriter.Write('{');
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(FirstAccessor);
        _indentedTextWriter.Write(';');
        if (SecondAccessor is not null)
        {
            _indentedTextWriter.Write(' ');
            _indentedTextWriter.Write(SecondAccessor);
            _indentedTextWriter.Write(';');
        }
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write('}');
        if (DefaultValue is not null)
        {
            _indentedTextWriter.Write(' ');
            _indentedTextWriter.Write('=');
            _indentedTextWriter.Write(' ');
            _indentedTextWriter.Write(DefaultValue);
            _indentedTextWriter.Write(';');
        }
        _indentedTextWriter.WriteLine();
        return this;
    }
}
