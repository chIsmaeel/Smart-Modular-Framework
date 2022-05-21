namespace SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.PropertyTemplates;

using System.Text;

// Properties
public record PropertyTemplate(string Type, string IdentifierName) : TypeMemberTemplate(IdentifierName)
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
    /// Gets the default value.
    /// </summary>
    public string? DefaultValue { get; init; }

    /// <summary>
    /// Creates the method template.
    /// </summary>
    /// <returns>A string.</returns>
    public override ITemplate CreateTemplate()
    {
        base.CreateTemplate();
        return this;
    }

    /// <summary>
    /// Writes the comment.
    /// </summary>                                                 
    protected void WriteComment()
    {
        if (Comment is null || Comment == IdentifierName || Comment == IdentifierName + ".")
            DefaultPropertyComment();
        else if (Comment != IdentifierName)
            _indentedTextWriter!.WriteLine(CommentTemplate.CreateCommentFromText(Comment!));
    }

    /// <summary>
    /// Defaults the property comment.
    /// </summary>
    private void DefaultPropertyComment()
    {
        var tempComment = new StringBuilder();
        tempComment.Append(FirstAccessor.FirstCharToUpperCase()).Append('s');
        tempComment.Append(' ');
        if (SecondAccessor is not null)
            tempComment.Append("or").Append(' ').Append(SecondAccessor.FirstCharToUpperCase()).Append('s').Append(' ');
        tempComment.Append("the").Append(' ').Append(IdentifierName).Append('.');
        _indentedTextWriter!.WriteLine(CommentTemplate.CreateCommentFromText(tempComment.ToString()));
    }

    /// <summary>
    /// Writes the property declaration.
    /// </summary>
    protected void WritePropertyDeclaration()
    {
        _indentedTextWriter!.Write(Modifiers);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(Type);
        _indentedTextWriter.Write(' ');
        _indentedTextWriter.Write(IdentifierName);
    }
}