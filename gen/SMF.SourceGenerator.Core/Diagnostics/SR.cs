namespace SMF.SourceGenerator.Core.Diagnostics;
/// <summary>
/// The s r.
/// </summary>

public partial record SR
{
    /// <summary>
    /// Gets the invalid s m class access modifier title.
    /// </summary>
    public static string InvalidSMClassAccessModifierTitle => GetResourceString(nameof(InvalidSMClassAccessModifierTitle));
    /// <summary>
    /// Gets the invalid s m class access modifier message.
    /// </summary>
    public static string InvalidSMClassAccessModifierMessage => GetResourceString(nameof(InvalidSMClassAccessModifierMessage));


    /// <summary>
    /// Gets the should be partial class title.
    /// </summary>
    public static string ShouldBePartialClassTitle => GetResourceString(nameof(ShouldBePartialClassTitle));
    /// <summary>
    /// Gets the should be partial class message.
    /// </summary>
    public static string ShouldBePartialClassMessage => GetResourceString(nameof(ShouldBePartialClassMessage));


    /// <summary>
    /// Gets the should not be comment on more than single type title.
    /// </summary>
    public static string ShouldNotHaveCommentOnMoreThanSingleTypeTitle => GetResourceString(nameof(ShouldNotHaveCommentOnMoreThanSingleTypeTitle));
    /// <summary>
    /// Gets the should not be comment on more than single type message.
    /// </summary>
    public static string ShouldNotHaveCommentOnMoreThanSingleTypeMessage => GetResourceString(nameof(ShouldNotHaveCommentOnMoreThanSingleTypeTitle));


    /// <summary>
    /// Gets the must have s m f model type title.
    /// </summary>
    public static string MustHaveSMFModelTypeTitle => GetResourceString(nameof(MustHaveSMFModelTypeTitle));
    /// <summary>
    /// Gets the must have s m f model type message.
    /// </summary>
    public static string MustHaveSMFModelTypeMessage => GetResourceString(nameof(MustHaveSMFModelTypeMessage));


    /// <summary>
    /// Gets the should not be registered more than one s m f model type title.
    /// </summary>
    public static string ShouldNotBeRegisteredMoreThanOneSMFModelTypeTitle => GetResourceString(nameof(ShouldNotBeRegisteredMoreThanOneSMFModelTypeTitle));
    /// <summary>
    /// Gets the should not be registered more than one s m f model type message.
    /// </summary>
    public static string ShouldNotBeRegisteredMoreThanOneSMFModelTypeMessage => GetResourceString(nameof(ShouldNotBeRegisteredMoreThanOneSMFModelTypeMessage));


    /// <summary>
    /// Gets the should have comment in atleast one s m f type title.
    /// </summary>
    public static string ShouldHaveCommentInAtleastOneSMFTypeTitle => GetResourceString(nameof(ShouldHaveCommentInAtleastOneSMFTypeTitle));
    /// <summary>
    /// Gets the should have comment in atleast one s m f type message.
    /// </summary>
    public static string ShouldHaveCommentInAtleastOneSMFTypeMessage => GetResourceString(nameof(ShouldHaveCommentInAtleastOneSMFTypeMessage));


    /// <summary>
    /// Gets the invalid s m f model in registered model field in module type title.
    /// </summary>
    public static string InvalidSMFModelInRegisteredModelFieldInModuleTypeTitle => GetResourceString(nameof(InvalidSMFModelInRegisteredModelFieldInModuleTypeTitle));
    /// <summary>
    /// Gets the invalid s m f model in registered model field in module type message.
    /// </summary>
    public static string InvalidSMFModelInRegisteredModelFieldInModuleTypeMessage => GetResourceString(nameof(InvalidSMFModelInRegisteredModelFieldInModuleTypeMessage));

    public static string MusthavePropertyCommentTitle => GetResourceString(nameof(MusthavePropertyCommentTitle));
    public static string MusthavePropertyCommentMessage => GetResourceString(nameof(MusthavePropertyCommentMessage));

}
