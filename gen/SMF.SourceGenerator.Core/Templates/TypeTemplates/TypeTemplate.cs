namespace SMF.SourceGenerator.Core.Templates.TypeTemplates;

using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.Interfaces;
using SMF.SourceGenerator.Core.Templates.TypeTemplates.MemberTemplates.PropertyTemplates;
using System.CodeDom.Compiler;
using System.Text;

/// <summary>
/// The template for the class that represents TypeTemplate.
/// </summary>
public abstract partial record TypeTemplate(string IdentifierName) : ITypeTemplate, ITypeMemberTemplate
{
    protected StringBuilder? stringBuilder;
    protected StringWriter? _stringWriter;
    protected IndentedTextWriter? _indentedTextWriter;



    /// <summary>
    /// Gets the type comment.
    /// </summary>
    public virtual string? Comment { get; init; }

    /// <summary>
    /// Gets the type attributes.
    /// </summary>
    public virtual List<string>? Attributes { get; init; } = new();

    /// <summary>                
    /// Gets the modifiers.
    /// </summary>
    public virtual string Modifiers { get; init; } = "public";
    /// <summary>
    /// Gets the type.
    /// </summary>
    public virtual string Type { get; init; } = "class";

    /// <summary>
    /// Gets a value indicating whether add default comment if not exist.
    /// </summary>
    public bool AddDefaultCommentIfNotExist { get; init; } = true;

    /// <summary>
    /// Gets the base type.
    /// </summary>
    public virtual string? ParentType { get; init; }

    /// <summary>
    /// Gets the interfaces.
    /// </summary>
    public virtual List<string>? Interfaces { get; init; } = new();

    /// <summary>
    /// Gets the generic parameters.
    /// </summary>
    public List<string>? GenericParameters { get; init; } = new();

    /// <summary>
    /// Gets the members.
    /// </summary>                        
    public List<IMemberTemplate> Members { get; } = new();

    /// <summary>
    /// Gets the string members.
    /// </summary>
    public List<string> StringMembers { get; } = new();

    /// <summary>
    /// Gets the using namespaces.
    /// </summary>
    public List<string> UsingNamespaces { get; init; } = new();
    /// <summary>
    /// Gets or sets the parent.
    /// </summary>
    public ITemplate? Parent { get; set; }

    /// <summary>
    /// Gets the type comment.
    /// </summary>
    /// <returns>A string.</returns>
    public string? WriteComment()
    {

        if (!string.IsNullOrWhiteSpace(Comment))
            return (CommentTemplate.CreateCommentFromText(Comment!));
        else if (AddDefaultCommentIfNotExist && string.IsNullOrWhiteSpace(Comment))
            return (CommentTemplate.CreateCommentFromIdentifierName(IdentifierName));
        return null;
    }

    /// <summary>
    /// Gets the type attributes.
    /// </summary>
    /// <returns>A string? .</returns>
    public string? GetTypeAttributes()
    {
        if (Attributes is null || Attributes.Count == 0) return null;
        StringBuilder stringBuilder = new();
        foreach (var attribute in Attributes)
        {
            stringBuilder.Append('[').Append(attribute).AppendLine("]");
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Gets the type declaration with base type and interfaces.
    /// </summary>
    /// <returns>A string.</returns>
    private string GetTypeDeclarationWithBaseTypeAndInterfaces()
    {
        StringBuilder typeDelarationBuider = new();

        typeDelarationBuider!.Append(Modifiers).Append(' ').Append(Type).Append(' ').Append(IdentifierName);

        if (GenericParameters?.Count > 0)
            typeDelarationBuider.Append($"<{string.Join(", ", GenericParameters)}>");
        if ((!string.IsNullOrEmpty(ParentType) || Interfaces?.Count > 0) == false)
            return typeDelarationBuider.ToString();

        // Append base type and interfaces.
        typeDelarationBuider.Append(" : ");
        if (!string.IsNullOrEmpty(ParentType))
        {
            typeDelarationBuider.Append(ParentType);
            if (Interfaces?.Count > 0)
                typeDelarationBuider.Append(", ");
        }

        if (Interfaces?.Count > 0)
            typeDelarationBuider.Append(string.Join(", ", Interfaces?.ToArray()));

        return typeDelarationBuider.ToString();
    }

    /// <summary>
    /// Gets the members.
    /// </summary>
    /// <returns>A string.</returns>
    protected virtual string GetMembers()
    {
        // Add type Field Members.
        StringBuilder memberSB = new();

        // Members
        List<TypeFieldTemplate> typeFields = new();
        Members.Where(_ => _ is TypeFieldTemplate).Cast<TypeFieldTemplate>().ForEach(typeFields.Add);
        Members.Where(_ => _ is FullPropertyTemplate).Cast<FullPropertyTemplate>().ForEach(_ => typeFields.Add(_.FieldTemplate));

        // Add Members In Template.

        // Add Type Field in Members.
        typeFields.ForEach(_ =>
               {
                   _.Parent = this;
                   memberSB.Append(_.CreateTemplate().GetTemplate());
                   if (_.UsingNamespaces?.Count > 0)
                       UsingNamespaces.AddRange(_.UsingNamespaces);
               });

        memberSB.AppendLine();

        // Add Type Constructor in Members.
        Members.Where(_ => _ is ConstructorTemplate)
           .ForEach(_ =>
           {
               _!.Parent = this;
               memberSB.AppendLine(_.CreateTemplate().GetTemplate());
               if (_.UsingNamespaces?.Count > 0)
                   UsingNamespaces.AddRange(_.UsingNamespaces);
           });

        // Add Type Auto Property in Members.
        Members.Where(_ => _ is AutoPropertyTemplate)
           .ForEach(_ =>
           {
               _!.Parent = this;
               memberSB.AppendLine(_.CreateTemplate().GetTemplate());
               if (_.UsingNamespaces?.Count > 0)
                   UsingNamespaces.AddRange(_.UsingNamespaces);
           });

        // Add Type Full Property in Members.
        Members.Where(_ => _ is FullPropertyTemplate).Cast<FullPropertyTemplate>()
           .ForEach(_ =>
           {
               _.Parent = this;
               memberSB.AppendLine(_.CreateTemplate(typeFields).GetTemplate());
               if (_.UsingNamespaces?.Count > 0)
                   UsingNamespaces.AddRange(_.UsingNamespaces);
           });


        // Add Type Method in Members.
        Members.Where(_ => _ is TypeMethodTemplate).Cast<TypeMethodTemplate>()
          .ForEach(_ =>
          {
              _.Parent = this;
              memberSB.AppendLine(_.CreateTemplate(typeFields).GetTemplate());
              if (_.UsingNamespaces?.Count > 0)
                  UsingNamespaces.AddRange(_.UsingNamespaces);
          });

        // Add Partial Methods in Members.
        Members.Where(_ => _ is PartialMethodTemplate).Cast<PartialMethodTemplate>()
          .ForEach(_ =>
          {
              memberSB.AppendLine(_.CreateTemplate().GetTemplate());
              //if (_.UsingNamespaces?.Count > 0)
              //    UsingNamespaces.AddRange(_.UsingNamespaces);
          });

        // Add a Type in Members.
        Members.Where(_ => _ is TypeTemplate).Cast<TypeTemplate>()
          .ForEach(_ =>
          {
              _.Parent = this;
              memberSB.AppendLine(_.CreateTemplate().GetTemplate());
              if (_.UsingNamespaces?.Count > 0)
                  UsingNamespaces.AddRange(_.UsingNamespaces);
          });

        // String Members.
        StringMembers.ForEach(_ => memberSB.AppendLine(_));


        return "    " + memberSB.ToString().Trim();
    }

    /// <summary>
    /// Extracts the values.
    /// </summary>
    /// <param name="headerDocFileName">The header doc file name.</param>
    /// <param name="fileScopedNamespace">The file scoped namespace.</param>
    /// <param name="usingNamespaces">The using namespaces.</param>
    /// <param name="typeComment">The type comment.</param>
    /// <param name="typeAttributes">The type attributes.</param>
    /// <param name="typeDeclarationWithParent">The type declaration with parent.</param>
    /// <param name="members">The members.</param>
    private void ExtractValues(out string? typeComment, out string? typeAttributes, out string typeDeclarationWithParent, out string members)
    {
        typeComment = WriteComment();
        typeDeclarationWithParent = GetTypeDeclarationWithBaseTypeAndInterfaces();
        members = GetMembers();
        typeAttributes = GetTypeAttributes();
    }

    /// <summary>
    /// Generates the template.                             
    /// </summary>                                             
    public virtual ITemplate CreateTemplate()
    {
        stringBuilder = new();
        _stringWriter = new(stringBuilder);
        _indentedTextWriter = new(_stringWriter);
        ExtractValues(out var typeComment, out string? typeAttributes, out var typeDeclarationWithParent, out var members);
        if (typeComment is not null) _indentedTextWriter!.WriteLine(typeComment);
        if (typeAttributes is not null) _indentedTextWriter!.WriteLine(typeAttributes);
        _indentedTextWriter!.WriteLine(typeDeclarationWithParent);
        _indentedTextWriter!.WriteLine("{");
        //_indentedTextWriter!.Indent++;
        if (members.Length > 4)
            _indentedTextWriter!.WriteLine(members);
        _indentedTextWriter!.WriteLine("}");
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
