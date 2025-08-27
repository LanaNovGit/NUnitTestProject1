using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnitTestProject1.Framework.Config;
using System;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTestProject1.Framework.Helpers
{
    public class WaitHelper
    {
        private readonly WebDriverWait _wait;

        private static IWebDriver _driver;
        public WaitHelper(IWebDriver driver, int? timeoutSec = null)
        {
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSec ?? TestSettings.Current.DefaultTimeoutSec));
            _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            _driver = driver;
        }

        public IWebElement UntilClickable(By by) => _wait.Until(ExpectedConditions.ElementToBeClickable(by));

        public IWebElement? UntilVisible(By by)
        {
            try
            {
                return _wait.Until(driver =>
                {
                    try
                    {
                        var el = driver.FindElement(by);
                        return el.Displayed ? el : null;
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine($"Element {by} not found yet...");
                        return null;
                    }
                });
            }
            catch (WebDriverTimeoutException exc)
            {
                Console.WriteLine($"Timeout: Element {by} not visible after {_wait.Timeout.TotalSeconds} seconds.");
                Console.WriteLine(exc.Message);
                return null;
            }

        }
        public bool UntilUrlContains(string part) => _wait.Until(d => d.Url.Contains(part));
        public IWebElement UntilVisibleLambadaBased(By by)
        {
            return _wait.Until(driver =>
            {
                try
                {
                    var el = driver.FindElement(by);
                    return (el != null && el.Displayed) ? el : null;
                }
                catch (NoSuchElementException)
                {
                    return null; // keep polling
                }
            });
        }

        public bool UntilVisibleLambadaBased(IWebElement element)
        {
            return _wait.Until(driver =>
            {
                try
                {
                    return element != null && element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false; // keep polling
                }
            });
        }
        public static void func()
        {

            List<IWebElement> buttons = _driver.FindElements(By.XPath("//a[contains(.,'Sign in')]")).ToList();

            foreach (var button in buttons)
            {

                Console.WriteLine("found a button");

            }

        }


        public static Dictionary<char, int> countAccourences(string str)
        {

            var map = new Dictionary<char, int>();
            foreach (char c in str)
            {
                //Verify if the charalready in the map
                if (map.ContainsKey(c))
                {
                    //do ++;
                    map[c]++;
                }
                else {
                    map[c] = 1;
                }             

            }
            return map;
        }

    }
}
