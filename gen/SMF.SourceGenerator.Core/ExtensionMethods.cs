namespace SMF.SourceGenerator.Core;

/// <summary>
/// The extension methods.
/// </summary>

public static class ExtensionMethods
{
    /// <summary>
    /// Gets the qualified name.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <returns>A string.</returns>
    public static string GetQualifiedName(this ClassDeclarationSyntax cds)
    {
        var nameSpace = cds?.GetContainingNamespace();
        if (nameSpace is null || nameSpace == cds?.Identifier.Text)
        {
            return cds?.Identifier.Text!;
        }

        return nameSpace + "." + cds!.Identifier.Text;
    }

    /// <summary>
    /// Gets the containing namespace.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <returns>A string.</returns>
    public static string? GetContainingNamespace(this ClassDeclarationSyntax cds)
    {
        return cds?.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>() is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDS
            ? fileScopedNamespaceDS.Name.ToString()
            : cds?.FirstAncestorOrSelf<NamespaceDeclarationSyntax>() is NamespaceDeclarationSyntax namespaceDeclarationSyntax
            ? namespaceDeclarationSyntax.Name.ToString() : null;
    }

    /// <summary>
    /// Gets the using qualified names.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <returns>An IEnumerable&lt;string?&gt;? .</returns>
    public static IEnumerable<string?>? GetUsingDeclarationSyntaxNames(this ClassDeclarationSyntax? cds)
    {
        var usingQualifiedNames = new List<string?>();
        if (cds!.Parent!.Parent is CompilationUnitSyntax compilationUnitSyntax && compilationUnitSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(compilationUnitSyntax.Usings.Select(_ => _.Name.ToString()));
        if (cds.Parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDS && fileScopedNamespaceDS.Usings.Count > 0)
            usingQualifiedNames.AddRange(fileScopedNamespaceDS.Usings.Select(_ => _.Name.ToString()));
        if (cds.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax && namespaceDeclarationSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(namespaceDeclarationSyntax.Usings.Select(_ => _.Name.ToString()));
        return usingQualifiedNames;
    }

    /// <summary>
    /// Gets the all possible qualified names.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <param name="identifierName">The identifier name.</param>
    /// <returns>An IEnumerable&lt;string?&gt;? .</returns>
    public static IEnumerable<string?>? GetAllPossibleQualifiedNames(this ClassDeclarationSyntax? cds, string identifierName)
    {
        var usingQualifiedNames = new List<string?>
        {
            identifierName,
            cds!.GetContainingNamespace() + "." + identifierName
        };
        if (cds!.Parent!.Parent is CompilationUnitSyntax compilationUnitSyntax && compilationUnitSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(compilationUnitSyntax.Usings.Select(_ => _.Name.ToString() + "." + identifierName));
        if (cds.Parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDS && fileScopedNamespaceDS.Usings.Count > 0)
            usingQualifiedNames.AddRange(fileScopedNamespaceDS.Usings.Select(_ => _.Name.ToString() + "." + identifierName));
        if (cds.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax && namespaceDeclarationSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(namespaceDeclarationSyntax.Usings.Select(_ => _.Name.ToString() + "." + identifierName));
        return usingQualifiedNames;
    }

    /// <summary>
    /// Gets the all possible qualified names.
    /// </summary>
    /// <param name="cds">The cds.</param>
    /// <returns>An IEnumerable&lt;string?&gt;? .</returns>
    public static IEnumerable<string?>? GetAllPossibleQualifiedNames(this ClassDeclarationSyntax? cds)
    {
        var usingQualifiedNames = new List<string?>
        {

            cds!.GetContainingNamespace()
        };
        if (cds!.Parent!.Parent is CompilationUnitSyntax compilationUnitSyntax && compilationUnitSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(compilationUnitSyntax.Usings.Select(_ => _.Name.ToString()));
        if (cds.Parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDS && fileScopedNamespaceDS.Usings.Count > 0)
            usingQualifiedNames.AddRange(fileScopedNamespaceDS.Usings.Select(_ => _.Name.ToString()));
        if (cds.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax && namespaceDeclarationSyntax.Usings.Count > 0)
            usingQualifiedNames.AddRange(namespaceDeclarationSyntax.Usings.Select(_ => _.Name.ToString()));
        return usingQualifiedNames;
    }

    /// <summary>
    /// Fors the each.
    /// </summary>
    /// <param name="values">The values.</param>
    /// <param name="action">The action.</param>
    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var value in values)
        {
            action(value);
        }
    }

    /// <summary>
    /// Firsts the char to lower case.
    /// </summary>
    /// <param name="str">The str.</param>
    /// <returns>A string? .</returns>
    public static string? FirstCharToLowerCase(this string? str)
    {
        return !string.IsNullOrEmpty(str) && char.IsUpper(str![0])
            ? str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str.Substring(1)
            : str;
    }

    /// <summary>
    /// Firsts the char to upper case.
    /// </summary>
    /// <param name="str">The str.</param>
    /// <returns>A string? .</returns>
    public static string? FirstCharToUpperCase(this string? str)
    {
        return !string.IsNullOrEmpty(str) && char.IsLower(str![0])
            ? str.Length == 1 ? char.ToUpper(str[0]).ToString() : char.ToUpper(str[0]) + str.Substring(1)
            : str;
    }

    /// <summary>
    /// Have the more than one c d s.
    /// </summary>
    /// <param name="cdsArray">The cds array.</param>
    /// <param name="cds">The cds.</param>
    /// <returns>A list of ClassDeclarationSyntax?.</returns>
    public static IEnumerable<ClassDeclarationSyntax?>? GetAllPartialClasses(this IEnumerable<ClassDeclarationSyntax?>? cdss, ClassDeclarationSyntax? cds)
    {
        return cdss.Where(_ => _!.GetQualifiedName() == cds!.GetQualifiedName());
    }

    /// <summary>
    /// Have the more than one c d s.
    /// </summary>
    /// <param name="cdsFirst">The cds first.</param>
    /// <param name="cdsSecond">The cds second.</param>
    /// <returns>A bool.</returns>
    public static bool GetAllPartialClasses(this ClassDeclarationSyntax? cdsFirst, ClassDeclarationSyntax? cdsSecond)
    {
        var hasSameIdentifier = cdsFirst?.Identifier.ValueText == cdsSecond?.Identifier.ValueText;
        if (!hasSameIdentifier)
        {
            return false;
        }

        string? firstNamespace = null;
        string? secondNamespace = null;

        var cdsFirstHasFileScopedNamespace = cdsFirst?.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();
        var cdsFirstHasNamespaceScopedNamespace = cdsFirst?.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString();
        firstNamespace = cdsFirstHasFileScopedNamespace ?? cdsFirstHasNamespaceScopedNamespace;

        var cdsSecondHasFileScopedNamespace = cdsSecond?.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();
        var cdsSecondHasNamespaceScopedNamespace = cdsSecond?.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString();

        secondNamespace = cdsSecondHasFileScopedNamespace ?? cdsSecondHasNamespaceScopedNamespace;

        var r = firstNamespace == secondNamespace;

        return r;
    }

    #region SyntaxNode

    /// <summary>
    /// Are the module or model or model property or model property.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsSMFClass(this SyntaxNode node)
    {
        return node.IsModuleClass() || node.IsModelClass() || node.IsControllerClass();
    }

    /// <summary>
    /// Are the module class.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsModuleClass(this SyntaxNode node)
    {
        return node is ClassDeclarationSyntax cds && cds!.Identifier.ValueText.EndsWith("Module");
    }

    /// <summary>
    /// Are the model class.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsModelClass(this SyntaxNode node)
    {
        return node is ClassDeclarationSyntax cds && cds!.Identifier.ValueText.EndsWith("Model");
    }


    /// <summary>
    /// Are the controller class.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsControllerClass(this SyntaxNode node)
    {
        return node is ClassDeclarationSyntax cds && cds!.Identifier.ValueText.EndsWith("Controller");
    }

    /// <summary>
    /// Are the model property.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsModelProperty(this SyntaxNode node)
    {
        return node is PropertyDeclarationSyntax pds && pds.Type.ToString().StartsWith("SM.");
    }

    /// <summary>
    /// Are the model field.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>A bool.</returns>
    public static bool IsModelField(this SyntaxNode node)
    {
        return node is FieldDeclarationSyntax fds && (fds.Parent as ClassDeclarationSyntax)!.Identifier.ValueText.EndsWith("Model");
    }
    #endregion SyntaxNode

}
