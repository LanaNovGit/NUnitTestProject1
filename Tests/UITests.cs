using FluentAssertions;
using NUnit.Framework;
using NUnitTestProject1.Framework.Drivers;
using NUnitTestProject1.Framework.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace NUnitTestProject1
{
    public class UITests
    {
        private IWebDriver _driver;
        private UiHelper _helper;
        [SetUp]
        public void Setup()
        {
            _driver = DriverFactory.Create();
            _helper = new UiHelper(_driver);
        }

        [Test]
        public void Test1()
        {
            string str = "SoftwareTestAutomation";

            var dic = UiHelper.countAccourences(str);

            foreach (var set in dic) {
                //Console.WriteLine(set.Key+" "+ set.Value);
                System.Diagnostics.Debug.WriteLine($"{set.Key} {set.Value}");
            }
        }

        [Test]
        public void CountChars_basic()
        {
            var result = UiHelper.countAccourences("Abba  ");
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new Dictionary<char, int>
            {
                ['a'] = 2,
                ['b'] = 2
            });
            result.Keys.Should().OnlyContain(c => char.IsLower(c));
        }

        [Test]
        public void CountChars_empty_returns_empty_dict()
        {
            UiHelper.countAccourences("").Should().BeEmpty();
            UiHelper.countAccourences(null).Should().BeEmpty();
        }

        [Test]
        public void Return_FirstUniqueChar()
        {
            var result = UiHelper.getFirstUnique("Abcbac  ");
            result.Should().NotBeNull();
            result.Should().Be('c');
        }

        [Test]
        public void Return_NonRepeationgChars_Empty()
        {
            var result = _helper.nonRepeating("Abcbac  ");
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void Return_NonRepeationgChars_OneChar()
        {
            var result = _helper.nonRepeating("Abcbacd  ");
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo("d");
        }


        [Test]
        public void Test2()
        {
            _driver.Navigate().GoToUrl("https://www.linkedin.com/");
            _helper.UntilVisibleLambadaBased(By.XPath(""));
        }

        [Test]
        public void Should_throw_when_arg_is_null()
        {
            //Action act = () => SomeMethod(null);
            //act.Should().Throw<ArgumentNullException>()
            //   .WithParameterName("name");
        }

        [Test]
        public void Should_return_two_sum_indices()
        {
            //TwoSum(new[] { 2, 7, 11, 15 }, 9).Should().Be((0, 1));
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