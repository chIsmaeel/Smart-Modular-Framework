namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.PropertyTemplates;

using System.CodeDom.Compiler;

public record FullPropertyTemplate(string Type, string IdentifierName) : PropertyTemplate(Type, IdentifierName)
{
    private TypeFieldTemplate? _fieldTemplate;
    private string _propertyField = string.Empty;

    /// <summary>
    /// Gets or sets the first accessor body action.
    /// </summary>
    public Action<IndentedTextWriter, string, IEnumerable<TypeFieldTemplate>>? FirstAccessorBodyAction { get; set; }

    /// <summary>
    /// Gets or sets the second accessor body action.
    /// </summary>
    public Action<IndentedTextWriter, string, IEnumerable<TypeFieldTemplate>>? SecondAccessorBodyAction { get; set; }

    /// <summary>
    /// Gets a value indicating whether apply cache statments.
    /// </summary>
    public bool ApplyCacheStatments { get; init; } = true;

    /// <summary>
    /// Gets the property field.
    /// </summary>
    public string PropertyField
    {
        get
        {
            if (_propertyField != string.Empty) return _propertyField;
            _propertyField = "_" + IdentifierName.FirstCharToLowerCase();
            return _propertyField;
        }
    }

    /// <summary>
    /// Gets the field template.
    /// </summary>
    public TypeFieldTemplate FieldTemplate
    {
        get
        {
            if (_fieldTemplate is null)
                _fieldTemplate = new TypeFieldTemplate(Type!, PropertyField);
            return _fieldTemplate;
        }
        set => _fieldTemplate = value;
    }

    /// <summary>
    /// Gets the write property cache statement.
    /// </summary>
    public static string? GetPropertyCacheStatement(string? type, string fieldName)
    {
        return $"if({fieldName} is not default({type})) return {fieldName};";
    }


    /// <summary>
    /// Creates the property template.
    /// </summary>
    public TypeMemberTemplate CreateTemplate(IEnumerable<TypeFieldTemplate> fieldTemplates)
    {
        base.CreateTemplate();

        WriteCommentAndAttributes();
        WritePropertyDeclaration();
        _indentedTextWriter!.WriteLine();
        _indentedTextWriter.WriteLine('{');
        _indentedTextWriter.Indent++;

        if (FirstAccessorBodyAction is null)
            DefaultFirstAccessorBody();
        else
        {
            _indentedTextWriter.WriteLine(FirstAccessor);
            _indentedTextWriter.WriteLine('{');
            _indentedTextWriter.Indent++;
            if (ApplyCacheStatments)
                _indentedTextWriter.WriteLine(GetPropertyCacheStatement(Type, PropertyField));
            FirstAccessorBodyAction(_indentedTextWriter, PropertyField!, fieldTemplates);
            _indentedTextWriter.Indent--;
            _indentedTextWriter.WriteLine("}");
        }

        if (SecondAccessor is not null && SecondAccessorBodyAction is null)
            DefaultSecondAccessorBody();
        else if (SecondAccessor is not null && SecondAccessorBodyAction is not null)
        {
            _indentedTextWriter.WriteLine(SecondAccessor);
            _indentedTextWriter.WriteLine('{');
            _indentedTextWriter.Indent++;
            SecondAccessorBodyAction(_indentedTextWriter, PropertyField!, fieldTemplates);
            _indentedTextWriter.Indent--;
            _indentedTextWriter.WriteLine("}");
        }

        _indentedTextWriter.Indent--;
        _indentedTextWriter.WriteLine('}');
        //_indentedTextWriter.WriteLine();
        return this;
    }

    /// <summary>
    /// Defaults the first accessor body.
    /// </summary>
    private void DefaultFirstAccessorBody()
    {
        _indentedTextWriter!.Write(FirstAccessor);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write("=>");
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(PropertyField);
        _indentedTextWriter.WriteLine(';');
    }

    /// <summary>
    /// Defaults the second accessor body.
    /// </summary>
    private void DefaultSecondAccessorBody()
    {
        _indentedTextWriter!.Write(SecondAccessor);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write("=>");
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(PropertyField);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write('=');
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write("value");
        _indentedTextWriter.WriteLine(';');

    }
}