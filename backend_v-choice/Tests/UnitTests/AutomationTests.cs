using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace UnitTests
{
    public class AutomationTests
    {
        [Fact]
        public void Automation_TryGetFilmById_IfIdIncorrect()
        {
            using IWebDriver driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            int testId = -1;

            driver.Navigate().GoToUrl("https://localhost:5001/api/Film/"+$"{testId}");
            var content = driver.PageSource;
            bool containsFilmOrStatus404 = content.Contains($"\"id\":{testId}") || content.Contains("\"status\":404");

            Assert.False(containsFilmOrStatus404);
            Assert.False(testId >= 0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(11)]
        public void Automation_TryGetFilmById_IfIdCorrect(int testId)
        {
            using IWebDriver driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            driver.Navigate().GoToUrl("https://localhost:5001/api/Film/"+$"{testId}");
            var content = driver.PageSource;
            bool containsFilmOrStatus404 = content.Contains($"\"id\":{testId}") || content.Contains("\"status\":404");

            Assert.True(containsFilmOrStatus404);
            Assert.True(testId >= 0);
        }
    }
}