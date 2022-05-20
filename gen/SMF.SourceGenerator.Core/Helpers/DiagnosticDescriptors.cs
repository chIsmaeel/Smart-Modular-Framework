namespace SMF.SourceGenerator.Core.Diagnostics;
using System;
/// <summary>
/// The diagnostic descriptors.
/// </summary>

public partial record DiagnosticDescriptors
{
    /// <summary>
    /// The _category.
    /// </summary>
    private const string _category = "Smart Modular Framework";
    /// <summary>
    /// Creates the diagnostic descriptor.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="title">The title.</param>
    /// <param name="messageFormat">The message format.</param>
    /// <param name="category">The category.</param>
    /// <param name="defaultSeverity">The default severity.</param>
    /// <param name="isEnabledByDefault">If true, is enabled by default.</param>
    /// <param name="customTags">The custom tags.</param>
    /// <returns>A DiagnosticDescriptor.</returns>
    public static DiagnosticDescriptor CreateDiagnosticDescriptor(
        string id,
        LocalizableString title,
        LocalizableString messageFormat,
        string category = _category,
        DiagnosticSeverity severity = DiagnosticSeverity.Error,
        bool isEnabledByDefault = true,
        params string[] customTags)
    {
        return new DiagnosticDescriptor(
            id: id,
            title: title,
            messageFormat: messageFormat,
            category: category,
            defaultSeverity: severity,
            isEnabledByDefault: isEnabledByDefault,
            customTags: customTags ?? new string[] { WellKnownDiagnosticTags.Build });
    }


    /// <summary>
    /// Creates the localizable string.
    /// </summary>
    /// <param name="resourceKey">The resource key.</param>
    /// <param name="resourceSource">The resource source.</param>
    /// <returns>A LocalizableString.</returns>
    public static LocalizableString CreateLocalizableString(string resourceKey, Type resourceSource)
    {
        return new LocalizableResourceString(resourceKey, SR.ResourceManager, resourceSource);
    }

    /// <summary>
    /// Creates the localizable string.
    /// </summary>
    /// <param name="resourceKey">The resource key.</param>
    /// <returns>A LocalizableString.</returns>
    public static LocalizableString CreateLocalizableString(string resourceKey)
    {
        return new LocalizableResourceString(resourceKey, SR.ResourceManager, typeof(Resources.Strings));
    }
}
