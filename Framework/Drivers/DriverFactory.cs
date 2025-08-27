using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace NUnitTestProject1.Framework.Drivers
{
    public static class DriverFactory
    {
        /// <summary>
        /// Create a WebDriver instance.
        /// Env vars (optional):
        ///   BROWSER=chrome|firefox  (only chrome shown here)
        ///   CI=true                  (enables headless + CI flags)
        ///   HEADLESS=true           (force headless locally)
        ///   SELENIUM_REMOTE_URL=http://grid:4444/wd/hub  (use Grid if set)
        /// </summary>
        public static IWebDriver Create(string browser = null)
        {
            var chosen = (browser ?? Environment.GetEnvironmentVariable("BROWSER") ?? "chrome").ToLowerInvariant();
            var ci = (Environment.GetEnvironmentVariable("CI") ?? "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            var forceHeadless = (Environment.GetEnvironmentVariable("HEADLESS") ?? "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            var remoteUrl = Environment.GetEnvironmentVariable("SELENIUM_REMOTE_URL");

            switch (chosen)
            {
                case "chrome":
                    var opts = new ChromeOptions();

                    // Local UX
                    opts.AddArgument("--start-maximized");

                    // Headless for CI (or when HEADLESS=true)
                    if (ci || forceHeadless)
                    {
                        opts.AddArgument("--headless=new");
                        opts.AddArgument("--no-sandbox");          // needed in many CI Linux containers
                        opts.AddArgument("--disable-dev-shm-usage"); // avoid /dev/shm bottleneck
                        opts.AddArgument("--window-size=1920,1080");
                    }

                    // If running against a Selenium Grid (Jenkins with a Grid/Standalone Chrome)
                    if (!string.IsNullOrWhiteSpace(remoteUrl))
                    {
                        // optional: set a name so you can see it in Grid UI
                        opts.AddAdditionalOption("se:name", $"CI_{Environment.MachineName}_{DateTime.UtcNow:HHmmss}");
                        return new RemoteWebDriver(new Uri(remoteUrl), opts);
                    }

                    // Local Chrome (Selenium Manager will fetch the correct chromedriver)
                    return new ChromeDriver(opts);

                default:
                    throw new NotSupportedException($"Browser not supported: {chosen}");
            }
        }
    }
}
