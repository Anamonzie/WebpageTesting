using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using EpamWeb.Utils;
using FluentAssertions;

namespace EpamWebTests.PageTests
{
    [TestFixture]
    [AllureFeature("Insights Page Tests")]
    public class InsightsPageTests : BaseTest
    {
        [Test]
        [AllureName("Insights Page Search Functionality Check")]
        [AllureTag("Search", "InsightsPage")]
        [Category("Smoke")]
        [AllureSeverity(SeverityLevel.critical)]
        public async Task EpamInsightsPage_SearchFunctionalityCheck()
        {
            // Arrange
            var testPage = Pages[TestContext.CurrentContext.Test.Name];
            var insightsPageService = serviceFactory.CreateInsightsPageService(testPage);

            await insightsPageService.NavigateToUrlAndAcceptCookiesAsync(ConstantData.EpamInsightsPageUrl);

            // Act
            await insightsPageService.InputTextInSearchFieldAsync();
            await insightsPageService.ClickFindButtonAsync();

            var result = await insightsPageService.GetSearchResultTextAsync();

            // Assert
            result.Should().Contain(TestData.SearchInput);
            logger.Info(TestContext.CurrentContext.Test.Name, $"Checking page title; expected to contain: {TestData.SearchInput}, actual: {result}.");
        }

        [Test]
        [AllureName("Insights Page Find Button Redirect Check")]
        [AllureTag("Redirect", "InsightsPage")]
        [Category("Regression")]
        [AllureSeverity(SeverityLevel.normal)]
        public async Task EpamInsightsPage_FindButtonRedirectCheck()
        {
            // Arrange
            var testPage = Pages[TestContext.CurrentContext.Test.Name];
            var insightsPageService = serviceFactory.CreateInsightsPageService(testPage);

            await insightsPageService.NavigateToUrlAndAcceptCookiesAsync(ConstantData.EpamInsightsPageUrl);
            const string expectedTitle = TestData.ExpectedSearchPageTitle;

            // Act
            await insightsPageService.ClickFindButtonAsync();
            await Task.Delay(TimeSpan.FromSeconds(2)); // 2-second delay - temporal check
            var result = await insightsPageService.GetPageTitleAsync();

            // Assert
            result.Should().Be(expectedTitle);
            logger.Info(TestContext.CurrentContext.Test.Name, $"Checking page title; expected: {expectedTitle}, actual: {result}.");
        }
    }
}
