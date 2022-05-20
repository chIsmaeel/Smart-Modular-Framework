namespace SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;

/// <summary>
/// The controller class type.                                  
/// </summary>

public partial record ControllerCT(
    IEnumerable<ClassDeclarationSyntax> ClassDSs, CancellationToken CancellationToken) : ClassType(ClassDSs, CancellationToken);