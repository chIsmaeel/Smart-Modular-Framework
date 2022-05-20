namespace SMF.SourceGenerator.Core.Diagnostics;
using System;
using System.Resources;
/// <summary>
/// The s r.
/// </summary>

public partial record SR
{
    private static ResourceManager? _resourceManager;
    /// <summary>
    /// Gets the resource manager.
    /// </summary>
    internal static ResourceManager ResourceManager => _resourceManager ??= new ResourceManager(typeof(Resources.Strings));
    private static readonly bool _usingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out bool usingResourceKeys) && usingResourceKeys;

    // This method is used to decide if we need to append the exception message parameters to the message when calling SR.Format.
    // by default it returns the value of System.Resources.UseSystemResourceKeys AppContext switch or false if not specified.
    // Native code generators can replace the value this returns based on user input at the time of native code generation.
    // The Linker is also capable of replacing the value of this method when the application is being trimmed.


    /// <summary>
    /// Usings the resource keys.
    /// </summary>
    /// <returns>A bool.</returns>
    private static bool UsingResourceKeys()
    {
        return _usingResourceKeys;
    }

    /// <summary>
    /// Gets the resource string.
    /// </summary>
    /// <param name="resourceKey">The resource key.</param>
    /// <returns>A string.</returns>
    public static string GetResourceString(string resourceKey)
    {
        if (UsingResourceKeys())
        {
            return resourceKey;
        }

        string? resourceString = null;
        try
        {
            resourceString =
#if SYSTEM_PRIVATE_CORELIB || CORERT
                    InternalGetResourceString(resourceKey);
#else
                ResourceManager.GetString(resourceKey);
#endif
        }
        catch (MissingManifestResourceException) { }

        return resourceString!; // only null if missing resources
    }

    /// <summary>
    /// Gets the resource string.
    /// </summary>
    /// <param name="resourceKey">The resource key.</param>
    /// <param name="defaultString">The default string.</param>
    /// <returns>A string.</returns>
    internal static string GetResourceString(string resourceKey, string defaultString)
    {
        string resourceString = GetResourceString(resourceKey);

        return resourceKey == resourceString || resourceString == null ? defaultString : resourceString;
    }

    /// <summary>
    /// Formats the.
    /// </summary>
    /// <param name="resourceFormat">The resource format.</param>
    /// <param name="p1">The p1.</param>
    /// <returns>A string.</returns>
    internal static string Format(string resourceFormat, object? p1)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1);
        }

        return string.Format(resourceFormat, p1);
    }

    /// <summary>
    /// Formats the.
    /// </summary>
    /// <param name="resourceFormat">The resource format.</param>
    /// <param name="p1">The p1.</param>
    /// <param name="p2">The p2.</param>
    /// <returns>A string.</returns>
    internal static string Format(string resourceFormat, object? p1, object? p2)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2);
        }

        return string.Format(resourceFormat, p1, p2);
    }

    /// <summary>
    /// Formats the.
    /// </summary>
    /// <param name="resourceFormat">The resource format.</param>
    /// <param name="p1">The p1.</param>
    /// <param name="p2">The p2.</param>
    /// <param name="p3">The p3.</param>
    /// <returns>A string.</returns>
    internal static string Format(string resourceFormat, object? p1, object? p2, object? p3)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2, p3);
        }

        return string.Format(resourceFormat, p1, p2, p3);
    }

    /// <summary>
    /// Formats the.
    /// </summary>
    /// <param name="resourceFormat">The resource format.</param>
    /// <param name="args">The args.</param>
    /// <returns>A string.</returns>
    internal static string Format(string resourceFormat, params object?[]? args)
    {
        if (args != null)
        {
            if (UsingResourceKeys())
            {
                return resourceFormat + ", " + string.Join(", ", args);
            }

            return string.Format(resourceFormat, args);
        }

        return resourceFormat;
    }

    /// <summary>
    /// Formats the.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="resourceFormat">The resource format.</param>
    /// <param name="p1">The p1.</param>
    /// <returns>A string.</returns>
    internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1);
        }

        return string.Format(provider, resourceFormat, p1);
    }

    /// <summary>
    /// Formats the.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="resourceFormat">The resource format.</param>
    /// <param name="p1">The p1.</param>
    /// <param name="p2">The p2.</param>
    /// <returns>A string.</returns>
    internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2);
        }

        return string.Format(provider, resourceFormat, p1, p2);
    }

    /// <summary>
    /// Formats the.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="resourceFormat">The resource format.</param>
    /// <param name="p1">The p1.</param>
    /// <param name="p2">The p2.</param>
    /// <param name="p3">The p3.</param>
    /// <returns>A string.</returns>
    internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2, object? p3)
    {
        if (UsingResourceKeys())
        {
            return string.Join(", ", resourceFormat, p1, p2, p3);
        }

        return string.Format(provider, resourceFormat, p1, p2, p3);
    }

    /// <summary>
    /// Formats the.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="resourceFormat">The resource format.</param>
    /// <param name="args">The args.</param>
    /// <returns>A string.</returns>
    internal static string Format(IFormatProvider? provider, string resourceFormat, params object?[]? args)
    {
        if (args != null)
        {
            if (UsingResourceKeys())
            {
                return resourceFormat + ", " + string.Join(", ", args);
            }

            return string.Format(provider, resourceFormat, args);
        }

        return resourceFormat;
    }
}
