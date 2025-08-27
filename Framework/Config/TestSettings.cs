using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitTestProject1.Framework.Config
{
    public class TestSettings
    {
        public string BaseUrl { get; set; } = "https://example.com";
        public int DefaultTimeoutSec { get; set; } = 10;

        public static TestSettings Current { get; } = new TestSettings();
    }
}
