using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnitTestProject1.Framework.Config;
using System;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NUnitTestProject1.Framework.Helpers
{
    public class UiHelper
    {
        private readonly WebDriverWait _wait;

        private static IWebDriver _driver;
        public UiHelper(IWebDriver driver, int? timeoutSec = null)
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

        //finding non-repeating characters in a string

        public List<char> nonRepeating(string str) {
            var res = new List<char>();
            var resTwoIteretion = new List<char>();
            var dic = new Dictionary<char, int>();

            foreach (char c in str) {
                char currentChar = char.ToLowerInvariant(c);
                if (char.IsWhiteSpace(currentChar)) continue;

                if (dic.ContainsKey(currentChar)) { 
                    dic[currentChar]++;
                    res.Remove(currentChar);
                } else {

                    dic[currentChar] = 1;
                    if (!res.Contains(currentChar))
                    {
                        res.Add(currentChar);
                    }
                }           
            }
            foreach(var node in dic)
            {
                if (node.Value.Equals(1)) resTwoIteretion.Add(node.Key);
            }

            return res;
        }

        //Are two strings anagrams? (ignore spaces/case)
        public static bool AreAnagrams(string a, string b)
        {
            if (a is null || b is null) return false;
            int[] freq = new int[26];

            foreach (var ch in a.ToLowerInvariant())
                if (ch >= 'a' && ch <= 'z') freq[ch - 'a']++;

            foreach (var ch in b.ToLowerInvariant())
                if (ch >= 'a' && ch <= 'z') freq[ch - 'a']--;

            foreach (var f in freq) if (f != 0) return false;
            return true;
        }

        //Reverse words in a sentence (keep single spaces)
        public static string ReverseWords(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            var parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Array.Reverse(parts);
            return string.Join(' ', parts);
        }

        //Remove duplicates from an int array, preserve order
        public static int[] Dedup(int[] nums)
        {
            var seen = new HashSet<int>();
            var result = new List<int>(nums.Length);
            foreach (var n in nums)
                if (seen.Add(n)) result.Add(n);
            return result.ToArray();
        }

        //Two-sum (return indices, or (-1,-1) if none)
        public static (int i, int j) TwoSum(int[] a, int target)
        {
            var map = new Dictionary<int, int>(); // value -> index
            for (int i = 0; i < a.Length; i++)
            {
                int need = target - a[i];
                if (map.TryGetValue(need, out var j)) return (j, i);
                map[a[i]] = i;
            }
            return (-1, -1);
        }

        //Try-style API with out parameter
        public static bool TryCountOf(string s, char target, out int count)
        {
            count = 0;
            if (string.IsNullOrEmpty(s)) return false;
            var t = char.ToLowerInvariant(target);
            foreach (var ch in s) if (char.ToLowerInvariant(ch) == t) count++;
            return true;
        }

        //LINQ: top-K most frequent chars (letters only)
        public static IEnumerable<(char c, int n)> TopK(string s, int k)
        {
            return (s ?? "")
                .Where(char.IsLetter)
                .Select(char.ToLowerInvariant)
                .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .Take(k)
                .Select(g => (g.Key, g.Count()));
        }

        //Async await
        public static async Task<string> GetStringAsync(string url, CancellationToken ct)
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            using var resp = await client.GetAsync(url, ct);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        //Wait and select
        public static void SelectByText(IWebDriver driver, By bySelect, string text)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var elem = wait.Until(d => d.FindElement(bySelect));
            new SelectElement(elem).SelectByText(text);
        }

        //Choose frop custom drop down
        public static void ChooseOption(IWebDriver driver, string optionText)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.FindElement(By.CssSelector("[data-test='country-dropdown']")).Click();

            var option = wait.Until(d =>
                d.FindElement(By.XPath($"//ul[@role='listbox']//li[normalize-space()='{optionText}']")));
            option.Click();
        }

        //Retry on stale element once
        public static void SafeClick(IWebDriver driver, By by)
        {
            try { driver.FindElement(by).Click(); }
            catch (StaleElementReferenceException)
            {
                driver.FindElement(by).Click();
            }
        }


        public static Dictionary<char, int> countAccourences(string str)
        {

            var map = new Dictionary<char, int>();

            if (string.IsNullOrEmpty(str)) return map;

            foreach (char c in str)
            {
                char cChar = char.ToLowerInvariant(c);

                if (char.IsWhiteSpace(cChar)) continue;

                //Verify if the charalready in the map
                if (map.ContainsKey(cChar))
                {
                    //do ++;
                    map[cChar]++;
                }
                else {
                    map[cChar] = 1;
                }             

            }
            return map;
        }
        
        //First non-repeating char (ignore spaces, case-insensitive)
        //Task: return the first char that appears exactly once; otherwise '\0'.

        public static char getFirstUnique(string str)
        {
            var counts = new Dictionary<char, int>();
            var q = new Queue<char>();

            foreach (char cr in str) {
                if (string.IsNullOrEmpty(str)) return '\0';
                var c = char.ToLowerInvariant(cr);
                if (char.IsWhiteSpace(c)) continue;

                // update count
                if (!counts.TryAdd(c, 1)) counts[c]++;
                else q.Enqueue(c); // first time seen

                // drop anything from the front that's no longer unique
                while (q.Count > 0 && counts[q.Peek()] > 1)
                    q.Dequeue();
            }
            
            return q.Count > 0 ? q.Peek() : '\0';
        }
    }
}
