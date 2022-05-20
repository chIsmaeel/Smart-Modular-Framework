namespace SMF.SourceGenerator.Core.Helpers;

using Microsoft.CodeAnalysis.CSharp;

/// <summary>
/// The s m f production context.
/// </summary>

public class SMFProductionContext
{
    private readonly SourceProductionContext _ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="SMFProductionContext"/> class.
    /// </summary>
    /// <param name="ctx">The ctx.</param>
    public SMFProductionContext(SourceProductionContext ctx)
    {
        _ctx = ctx;
    }

    /// <summary>
    /// Adds the source.
    /// </summary>
    /// <param name="fileName">The file name.</param>
    /// <param name="sourceCode">The source code.</param>
    public void AddSource(string fileName, string sourceCode)
    {
        var diagnostics = CSharpSyntaxTree.ParseText(sourceCode).GetDiagnostics();
        foreach (var diagnostic in diagnostics)
        {
            ReportDiagnostic(diagnostic);
        }
        _ctx.AddSource(fileName + ".g.cs", sourceCode);
    }

    /// <summary>
    /// Adds the source.
    /// </summary>
    /// <param name="fileScopedNamespaceTemplate">The file scoped namespace template.</param>
    public void AddSource(FileScopedNamespaceTemplate fileScopedNamespaceTemplate)
    {
        AddSource(fileScopedNamespaceTemplate.FileName!, fileScopedNamespaceTemplate.CreateTemplate().GetTemplate());
    }

    /// <summary>
    /// Reports the diagnostic.
    /// </summary>
    /// <param name="diagnostic">The diagnostic.</param>
    public void ReportDiagnostic(Diagnostic diagnostic)
    {
        _ctx.ReportDiagnostic(diagnostic);
    }
}