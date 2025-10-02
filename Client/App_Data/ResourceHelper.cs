using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;

public static class ResourceHelper
{
    private const string DefaultCulture = "en";

    public static string GetString(string key) =>
        GetString(GetCurrentPageName(), key);

    public static string GetString(string pageName, string key)
    {
        if (string.IsNullOrEmpty(key))
            return string.Empty;

        var app = HttpContext.Current?.Application;
        if (app == null)
            return Placeholder(pageName, key);

        var allResources = app["Resource"] as Dictionary<string, Dictionary<string, Dictionary<string, string>>>;
        if (allResources == null)
            return Placeholder(pageName, key);

        if (string.IsNullOrEmpty(pageName) || !allResources.TryGetValue(pageName, out var cultureMap))
            return Placeholder(pageName, key);

        var ui = CultureInfo.CurrentUICulture;
        var full = ui.Name;              // e.g. fr-FR
        var neutral = ui.TwoLetterISOLanguageName; // e.g. fr

        // 1. full culture
        if (TryGetValue(cultureMap, full, key, out var value))
            return value;

        // 2. neutral (if different)
        if (!string.Equals(full, neutral, StringComparison.OrdinalIgnoreCase) &&
            TryGetValue(cultureMap, neutral, key, out value))
            return value;

        // 3. default fallback
        if (!string.Equals(neutral, DefaultCulture, StringComparison.OrdinalIgnoreCase) &&
            TryGetValue(cultureMap, DefaultCulture, key, out value))
            return value;

        return Placeholder(pageName, key);
    }

    private static bool TryGetValue(
        Dictionary<string, Dictionary<string, string>> cultureMap,
        string culture,
        string key,
        out string value)
    {
        value = null;
        if (!cultureMap.TryGetValue(culture, out var kv))
            return false;

        return kv.TryGetValue(key, out value);
    }

    private static string GetCurrentPageName()
    {
        // AppRelativeCurrentExecutionFilePath: ~/Folder/LoginPage.aspx
        var path = HttpContext.Current?.Request?.AppRelativeCurrentExecutionFilePath;
        if (string.IsNullOrEmpty(path))
            return null;
        return Path.GetFileName(path); // keeps extension: LoginPage.aspx
    }

    private static string Placeholder(string pageName, string key) =>
        $"[{pageName ?? "?"}:{key}]";
}
