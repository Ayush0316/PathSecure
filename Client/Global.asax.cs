using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using System.Resources;

namespace Client
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["Resource"] = LoadAllLocalResources();
        }

        // Structure:
        // PageName -> culture -> key -> value
        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> LoadAllLocalResources()
        {
            var result = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>(StringComparer.OrdinalIgnoreCase);

            var folderPhysical = HttpContext.Current.Server.MapPath("~/App_LocalResources");
            if (!Directory.Exists(folderPhysical))
                return result;

            var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                         .Select(c => c.Name)
                                         .Where(n => !string.IsNullOrEmpty(n))
                                         .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var defaultCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            foreach (var file in Directory.EnumerateFiles(folderPhysical, "*.resx", SearchOption.TopDirectoryOnly))
            {
                var fileName = Path.GetFileName(file);                 // e.g., LoginPage.aspx.fr.resx
                var stem = Path.GetFileNameWithoutExtension(fileName); // e.g., LoginPage.aspx.fr
                string culture;
                string pageName;

                // Detect culture: last token after '.' that is a valid culture name (e.g. fr or fr-FR)
                var lastDot = stem.LastIndexOf('.');
                if (lastDot > -1)
                {
                    var possibleCulture = stem.Substring(lastDot + 1);
                    if (allCultures.Contains(possibleCulture))
                    {
                        culture = possibleCulture;
                        pageName = stem.Substring(0, lastDot);
                    }
                    else
                    {
                        culture = defaultCulture;
                        pageName = stem;
                    }
                }
                else
                {
                    culture = defaultCulture;
                    pageName = stem;
                }

                // Ensure dictionary entry exists
                if (!result.TryGetValue(pageName, out var cultureDict))
                {
                    cultureDict = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
                    result[pageName] = cultureDict;
                }

                var kv = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                try
                {
                    using (var reader = new ResXResourceReader(file))
                    {
                        foreach (System.Collections.DictionaryEntry entry in reader)
                        {
                            var key = entry.Key?.ToString();
                            var value = entry.Value?.ToString();
                            if (!string.IsNullOrEmpty(key))
                                kv[key] = value;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"[OK] {file} is valid.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[INVALID] {file} → {ex.Message}");
                }

                cultureDict[culture] = kv;
            }

            return result;
        }
    }
}
