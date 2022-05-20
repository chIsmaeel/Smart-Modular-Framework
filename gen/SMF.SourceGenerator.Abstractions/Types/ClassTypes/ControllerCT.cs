namespace SMF.Common.SourceGenerator.Abstractions.Types.ClassTypes;

/// <summary>
/// The controller class type.                                  
/// </summary>

public partial record ControllerCT(
    IEnumerable<ClassDeclarationSyntax> ClassDSs, ConfigSMFAndGlobalOptions ConfigSMFAndGlobalOptions, CancellationToken CancellationToken) : ClassType(ClassDSs, ConfigSMFAndGlobalOptions, CancellationToken);