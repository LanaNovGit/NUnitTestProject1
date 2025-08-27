using NUnit.Framework;
using NUnitTestProject1.Framework.Drivers;
using NUnitTestProject1.Framework.Helpers;
using OpenQA.Selenium;
using System;

namespace NUnitTestProject1
{
    public class UITests
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = DriverFactory.Create();
        }

        [Test]
        public void Test1()
        {
            string str = "SoftwareTestAutomation";

            var dic = WaitHelper.countAccourences(str);

            foreach (var set in dic) {
                //Console.WriteLine(set.Key+" "+ set.Value);
                System.Diagnostics.Debug.WriteLine($"{set.Key} {set.Value}");
            }
        }

        [Test]
        public void Test2()
        {
            _driver.Navigate().GoToUrl("https://www.linkedin.com/");
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();   // closes all windows & ends session
                _driver.Dispose();
            }
        }

    }
}