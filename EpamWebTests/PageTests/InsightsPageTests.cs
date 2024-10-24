using EpamWeb.Factory;
using Microsoft.Playwright;
using FluentAssertions;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Utils;
using EpamWeb.Config;
using Allure.Net.Commons;

namespace EpamWebTests.PageTests
{
    [AllureNUnit]
    [TestFixture]
    [AllureSuite("Insights Page Tests")]
    public class InsightsPageTests
    {
        private static IBrowserFactory browserFactory;
        private IPageFactory pageFactory;
        private IServiceFactory serviceFactory;
        //private static IConfigurationManager configurationManager;

        private static readonly ThreadLocal<IBrowser> browser = new();
        private IBrowserContext context;
        private IPage page;

        [OneTimeSetUp]
        public static void GlobalSetup()
        {
            //configurationManager = ConfigurationManager.Instance(); // Use the singleton instance
            browserFactory = BrowserFactory.Instance; // Pass it to the BrowserFactory
        }

        [SetUp]
        public async Task Setup()
        {
            browser.Value = await browserFactory.GetBrowser();
            context = await browser.Value.NewContextAsync();
            page = await context.NewPageAsync();

            pageFactory =  PageFactory.Instance(page);
            serviceFactory = ServiceFactory.Instance(pageFactory, page);
        }

        [Test]
        [AllureName("Insights Page Search Functionality Check")]
        [AllureDescription("Verifies that the search functionality on the EPAM Insights page works as expected.")]
        [AllureTag("Search", "InsightsPage")]
        [Category("Smoke")]
        [AllureSeverity(SeverityLevel.critical)]
        public async Task EpamInsightsPage_SearchFunctionalityCheck()
        {
            // Arrange
            var insightsPageService = serviceFactory.CreateInsightsPageService();
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
        [Category("Smoke")]
        [AllureSeverity(SeverityLevel.normal)]
        public async Task EpamInsightsPage_FindButtonRedirectCheck()
        {
            // Arrange
            var insightsPageService = serviceFactory.CreateInsightsPageService();
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
