using EpamWeb;
using EpamWeb.Factory;
using EpamWeb.Pages;
using Microsoft.Playwright;
using FluentAssertions;
using Allure.NUnit;
using Allure.NUnit.Attributes;

namespace EpamWebTests
{
    [AllureNUnit]
    [TestFixture]
    [AllureSuite("Insights Page Tests")]
    public class InsightsPageTests
    {
        private IBrowserFactory factory;

        private static readonly ThreadLocal<IBrowser> browser = new();
        private IBrowserContext context;
        private IPage page;

        [SetUp]
        public async Task Setup()
        {
            factory = BrowserFactory.Instance;

            browser.Value = await factory.GetBrowser();
            context = await browser.Value.NewContextAsync();
            page = await context.NewPageAsync();
        }

        [Test]
        [AllureName("Insights Page Search Functionality Check")]
        [AllureDescription("Verifies that the search functionality on the EPAM Insights page works as expected.")]
        [AllureTag("Search", "InsightsPage")]
        public async Task EpamInsightsPage_SearchFunctionalityCheck()
        {
            // Arrange
            var insightsPageService = PageFactory.CreateInsightsPageService(page);
            await insightsPageService.NavigateToUrlAsync(Constants.EpamInsightsPageUrl);

            // Act
            await insightsPageService.InputTextInSearchFieldAsync();
            await insightsPageService.ClickFindButtonAsync();
            var result = await insightsPageService.GetSearchResultTextAsync();

            // Assert
            result.Should().Contain(TestData.SearchInput);

        }

        [Test]
        [AllureName("Insights Page Find Button Redirect Check")]
        [AllureDescription("Checks if the 'Find' button redirects to the correct page.")]
        [AllureTag("Redirect", "InsightsPage")]
        public async Task EpamInsightsPage_FindButtonRedirectCheck()
        {
            // Arrange
            var insightsPageService = PageFactory.CreateInsightsPageService(page);
            await insightsPageService.NavigateToUrlAsync(Constants.EpamInsightsPageUrl);
            const string expectedTitle = TestData.ExpectedSearchPageTitle;

            // Act
            await insightsPageService.ClickFindButtonAsync();
            var result = await insightsPageService.GetPageTitleAsync();

            // Assert
            result.Should().Be(expectedTitle);
        }

        [TearDown]
        public async Task GlobalTearDown()
        {
            if (browser.Value != null)
            {
                await browser.Value.CloseAsync();
                browser.Value = null;
            }
        }
    }
}
