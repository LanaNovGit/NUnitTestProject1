using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitTestProject1.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        IWebElement User => _wait.Until(d => d.FindElement(By.Id("username")));
        IWebElement Pass => _driver.FindElement(By.Id("password"));
        IWebElement Submit => _driver.FindElement(By.CssSelector("button[type='submit']"));

        public void Login(string u, string p)
        {
            User.Clear(); User.SendKeys(u);
            Pass.Clear(); Pass.SendKeys(p);
            Submit.Click();
        }
    }
}
