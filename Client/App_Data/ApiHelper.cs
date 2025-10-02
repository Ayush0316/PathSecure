using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Client.App_Data;
using Client.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

public static class ApiHelper
{
    private static Uri _baseUrl = new Uri("https://localhost:7136/api/");

    public static async Task<ResponseDto<T>> CallApiAsync<T>(
        string url,
        HttpMethod method,
        object payload = null,
        Dictionary<string, string> headers = null,
        bool haveRetry = true
    ) where T : class
    {
        try
        {
            var request = new HttpRequestMessage(method, url);

            if (headers != null)
            {
                foreach (var h in headers)
                    request.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }

            if (payload != null)
            {
                string json = JsonConvert.SerializeObject(payload);
                request.Content = new StringContent(json,
                    System.Text.Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = await CreateClient().SendAsync(request);
            UpdateLoginStatus();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ResponseDto<T>>(jsonResponse);

            if(apiResponse != null && haveRetry && apiResponse?.StatusCode == "484")
            {
                var result = await RefreshRequest();
                if (result) return await CallApiAsync<T>(url, method, payload, headers, false);
            }

            if (!response.IsSuccessStatusCode || apiResponse == null || !apiResponse.IsSuccess )
            {
                string error = apiResponse?.Message ?? "Unknown error.";
                string statusCode = apiResponse?.StatusCode;
                if (String.IsNullOrEmpty(statusCode))
                    statusCode = response.StatusCode.ToString();
                ShowPopup($"Error {statusCode}: {error}", isSuccess: false);
                return apiResponse;
            }

            ShowPopup(apiResponse.Message ?? "Success!", true);
            return apiResponse;
        }
        catch (Exception ex)
        {
            ShowPopup("Unexpected error: " + ex.Message, false);
            return default;
        }
    }
    public static string GetAccessToken()
    {
        var container = GetUserCookieContainer();
        var cookies = container.GetCookies(_baseUrl);

        var accessTokenCookie = cookies["ACCESS_TOKEN"];
        return accessTokenCookie?.Value;
    }

    private static async Task<bool> RefreshRequest()
    {
        var userId = HttpContext.Current?.Session?["UserId"];
        var payload = new { UserId = userId };
        var result = await CallApiAsync<string>("auth/refresh", HttpMethod.Post, payload, haveRetry: false);
        return result?.IsSuccess ?? false;
    }

    private static CookieContainer GetUserCookieContainer()
    {
        if (HttpContext.Current.Session["ApiCookies"] == null)
            HttpContext.Current.Session["ApiCookies"] = new CookieContainer();
        return (CookieContainer)HttpContext.Current.Session["ApiCookies"];
    }

    private static HttpClient CreateClient()
    {
        var handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = GetUserCookieContainer()
        };
        return new HttpClient(handler)
        {
            BaseAddress = _baseUrl
        };
    }

    private static void UpdateLoginStatus()
    {
        var container = GetUserCookieContainer();
        var baseUri = _baseUrl;

        Cookie accessTokenCookie = null;
        foreach (Cookie c in container.GetCookies(baseUri))
        {
            if (c.Name == "ACCESS_TOKEN")
            {
                accessTokenCookie = c;
                break;
            }
        }

        bool isLogOn = accessTokenCookie != null && !string.IsNullOrEmpty(accessTokenCookie.Value);
        HttpContext.Current.Session["isLogOn"] = isLogOn;
    }

    private static void ShowPopup(string message, bool isSuccess)
    {
        int durationMs = isSuccess ? 1000 : 3000;

        var page = HttpContext.Current.CurrentHandler as BasePage;
        page?.GlobalPopup?.Show(message, isSuccess, durationMs);
    }
}
