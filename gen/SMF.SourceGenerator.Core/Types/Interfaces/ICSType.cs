namespace SMF.SourceGenerator.Core.Types.Interfaces;

using SMF.SourceGenerator.Core.Helpers;
/// <summary>
/// The c s type.
/// </summary>

public interface ICSType
{
    /// <summary>
    /// Gets the comment.
    /// </summary>
    string? Comment { get; }

    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    string IdentifierName { get; }

    /// <summary>
    /// Gets the containing namespace.
    /// </summary>
    string ContainingNamespace { get; set; }

    /// <summary>
    /// Gets the qualified name.
    /// </summary>
    string? QualifiedName { get; set; }

    /// <summary>
    /// Gets the usings.
    /// </summary>
    IEnumerable<string> Usings { get; }

    /// <summary>
    /// Gets the cancellation token.
    /// </summary>
    CancellationToken CancellationToken { get; }

    /// <summary>
    /// Reports the diagnostics.
    /// </summary>
    /// <param name="context">The context.</param>
    void ReportDiagnostics(SMFProductionContext context);
}