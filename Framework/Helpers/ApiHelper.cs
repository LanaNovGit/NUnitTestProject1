using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace NUnitTestProject1.Framework.Helpers
{
    public static class ApiHelper
    {
        public static IConfiguration Config { get; }
        public static HttpClient Client { get; }
        public static string BaseUrl => (Config["Api:BaseUrl"] ?? "").Trim().TrimEnd('/');
        public static string Key => (Config["Api:Key"] ?? "").Trim();
        public static string Token => (Config["Api:Token"] ?? "").Trim();

        // Initializes once on first use
        static ApiHelper()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development"}.json", optional: true);
            // .AddEnvironmentVariables(); // keep off while debugging to avoid overrides

            Config = builder.Build();

            if (string.IsNullOrWhiteSpace(BaseUrl)) throw new Exception("Api:BaseUrl missing");
            if (string.IsNullOrWhiteSpace(Key)) throw new Exception("Api:Key missing");
            if (string.IsNullOrWhiteSpace(Token)) throw new Exception("Api:Token missing");

            // Normalize to exactly one "/1"
            var root = BaseUrl.EndsWith("/1", StringComparison.Ordinal) ? BaseUrl : BaseUrl + "/1";

            Client = new HttpClient(); // no BaseAddress to avoid accidental double /1 when you pass absolute URLs
            _normalizedRoot = root;
        }

        private static readonly string _normalizedRoot;

        /// <summary>
        /// Builds an ABSOLUTE URL with key/token and any extra query params.
        /// Example: https://api.trello.com/1/members/me/boards?key=...&token=...&fields=id,name,url
        /// </summary>
        public static string Q(string path, IDictionary<string, string?>? extra = null)
        {
            // Build absolute base (ensure single trailing slash)
            var rootWithSlash = _normalizedRoot.EndsWith("/") ? _normalizedRoot : (_normalizedRoot + "/");

            // Absolute URL from root + path (avoid // and duplicate /1)
            var absolute = new Uri(new Uri(rootWithSlash), path.TrimStart('/'));

            // Create a fresh query string with auth + extras (ensures exactly one '?')
            var ub = new UriBuilder(absolute);
            var qs = HttpUtility.ParseQueryString(string.Empty);
            qs["key"] = Key;
            qs["token"] = Token;

            if (extra != null)
            {
                foreach (var kv in extra)
                    qs[kv.Key] = kv.Value;
            }

            ub.Query = qs.ToString(); // e.g. "key=...&token=...&fields=id%2cname%2curl"

            return ub.Uri.AbsoluteUri; // FULL URL
        }

        public static async Task<T> ReadJson<T>(HttpResponseMessage resp)
        {
            var payload = await resp.Content.ReadFromJsonAsync<T>();
            payload.Should().NotBeNull("response should be valid JSON");
            return payload!;
        }
    }
}