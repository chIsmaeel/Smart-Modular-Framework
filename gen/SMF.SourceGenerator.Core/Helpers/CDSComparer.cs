namespace SMF.SourceGenerator.Core.Helpers;
/// <summary>
/// The c d s comparer.
/// </summary>

public class CDSComparer : Comparer<ClassDeclarationSyntax?, CDSComparer>
{
    /// <summary>
    /// Adds the to hash code.
    /// </summary>
    /// <param name="hashCode">The hash code.</param>
    /// <param name="obj">The obj.</param>
    protected override void AddToHashCode(ref HashCode hashCode, ClassDeclarationSyntax? obj)
    {
        hashCode.Add(obj?.Identifier.Text);
    }

    /// <summary>
    /// Are the equal.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <returns>A bool.</returns>
    protected override bool AreEqual(ClassDeclarationSyntax? cdsFirst, ClassDeclarationSyntax? cdsSecond)
    {
        return cdsFirst?.GetQualifiedName() == cdsSecond?.GetQualifiedName();
        //var hasSameIdentifier = cdsFirst?.Identifier.ValueText == cdsSecond?.Identifier.ValueText;
        //if (!hasSameIdentifier)
        //{
        //    return false;
        //}

        //string? firstNamespace = null;
        //string? secondNamespace = null;

        //var cdsFirstHasFileScopedNamespace = cdsFirst?.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();
        //var cdsFirstHasNamespaceScopedNamespace = cdsFirst?.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString();
        //firstNamespace = cdsFirstHasFileScopedNamespace ?? cdsFirstHasNamespaceScopedNamespace;

        //var cdsSecondHasFileScopedNamespace = cdsSecond?.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();
        //var cdsSecondHasNamespaceScopedNamespace = cdsSecond?.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString();

        //secondNamespace = cdsSecondHasFileScopedNamespace ?? cdsSecondHasNamespaceScopedNamespace;

        //var r = firstNamespace == secondNamespace;

        //return r;
    }
}
