namespace SMF.SourceGenerator.Core.Diagnostics;
/// <summary>
/// The diagnostic descriptors.
/// </summary>

public partial record DiagnosticDescriptors
{
    /// <summary>
    /// Gets the invalid s m class access modifier.
    /// </summary>
    public static DiagnosticDescriptor InvalidSMClassAccessModifier { get; } = CreateDiagnosticDescriptor(
        id: "SMF001",
        title: CreateLocalizableString(nameof(SR.InvalidSMClassAccessModifierTitle)),
        messageFormat: CreateLocalizableString(nameof(SR.InvalidSMClassAccessModifierMessage)));

    /// <summary>
    /// Gets the should be partial class.
    /// </summary>
    public static DiagnosticDescriptor ShouldBePartialClass { get; } = CreateDiagnosticDescriptor(
       id: "SMF002",
       title: CreateLocalizableString(nameof(SR.ShouldBePartialClassTitle)),
       messageFormat: CreateLocalizableString(nameof(SR.ShouldBePartialClassMessage)));

    /// <summary>
    /// Gets the should not be comment on more than single type.
    /// </summary>
    public static DiagnosticDescriptor ShouldNotBeCommentOnMoreThanSingleType { get; } = CreateDiagnosticDescriptor(
      id: "SMF003",
      title: CreateLocalizableString(nameof(SR.ShouldNotHaveCommentOnMoreThanSingleTypeTitle)),
      messageFormat: CreateLocalizableString(nameof(SR.ShouldNotHaveCommentOnMoreThanSingleTypeMessage)));

    /// <summary>
    /// Gets the must have s m f model type.
    /// </summary>
    public static DiagnosticDescriptor MustHaveSMFModelType { get; } = CreateDiagnosticDescriptor(
     id: "SMF004",
     title: CreateLocalizableString(nameof(SR.MustHaveSMFModelTypeTitle)),
     messageFormat: CreateLocalizableString(nameof(SR.MustHaveSMFModelTypeMessage)));

    /// <summary>
    /// Gets the should not be registered more than one s m f model type.
    /// </summary>
    public static DiagnosticDescriptor ShouldNotBeRegisteredMoreThanOneSMFModelType { get; } = CreateDiagnosticDescriptor(
    id: "SMF005",
    title: CreateLocalizableString(nameof(SR.ShouldNotBeRegisteredMoreThanOneSMFModelTypeTitle)),
    messageFormat: CreateLocalizableString(nameof(SR.ShouldNotBeRegisteredMoreThanOneSMFModelTypeMessage)));

    /// <summary>
    /// Gets the should have comment in atleast one s m f type.
    /// </summary>
    public static DiagnosticDescriptor ShouldHaveCommentInAtleastOneSMFType { get; } = CreateDiagnosticDescriptor(
   id: "SMF006",
   title: CreateLocalizableString(nameof(SR.ShouldHaveCommentInAtleastOneSMFTypeTitle)),
   messageFormat: CreateLocalizableString(nameof(SR.ShouldHaveCommentInAtleastOneSMFTypeMessage)),
   severity: DiagnosticSeverity.Warning);

    /// <summary>
    /// Gets the invalid s m f model in registered model field in module type.
    /// </summary>
    public static DiagnosticDescriptor InvalidSMFModelInRegisteredModelFieldInModuleType { get; } = CreateDiagnosticDescriptor(
        id: "SMF007",
        title: CreateLocalizableString(nameof(SR.InvalidSMFModelInRegisteredModelFieldInModuleTypeTitle)),
        messageFormat: CreateLocalizableString(nameof(SR.InvalidSMFModelInRegisteredModelFieldInModuleTypeMessage)));

}
