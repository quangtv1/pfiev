using System.Globalization;
using System.Resources;
using System.Reflection;

namespace OrientationPFIEV.Helpers;

/// <summary>
/// Thin wrapper around ResourceManager for bilingual UI string lookup.
/// Usage: L.SetLanguage("fr") once at startup, then L.Get("BtnOk") everywhere.
/// Supported languages: "fr" (French, default), "vi" (Vietnamese).
/// </summary>
public static class L
{
    private static ResourceManager? _rm;

    /// <summary>
    /// Initialise the ResourceManager for the given language code.
    /// Must be called before any form is shown.
    /// </summary>
    /// <param name="lang">"fr" or "vi"</param>
    public static void SetLanguage(string lang)
    {
        var baseName = $"OrientationPFIEV.Resources.Strings.{lang}";
        _rm = new ResourceManager(baseName, Assembly.GetExecutingAssembly());

        var culture = lang == "vi" ? "vi-VN" : "fr-FR";
        var ci = new CultureInfo(culture);
        Thread.CurrentThread.CurrentUICulture = ci;
        Thread.CurrentThread.CurrentCulture = ci;
    }

    /// <summary>
    /// Returns the localised string for <paramref name="key"/>.
    /// Falls back to the key name itself if the resource is missing.
    /// </summary>
    public static string Get(string key)
        => _rm?.GetString(key) ?? key;

    /// <summary>
    /// Returns a formatted localised string (String.Format overload).
    /// Example: L.Fmt("MsgImportDone", imported, skipped)
    /// </summary>
    public static string Fmt(string key, params object[] args)
    {
        var template = Get(key);
        return string.Format(template, args);
    }
}
