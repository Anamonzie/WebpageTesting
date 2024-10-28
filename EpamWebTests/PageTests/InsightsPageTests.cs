using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Factory;
using EpamWeb.Utils;
using FluentAssertions;
using Microsoft.Playwright;
using Serilog;

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

        private static readonly ThreadLocal<IBrowser> browser = new();
        private IBrowserContext context;
        private IPage page;

        [OneTimeSetUp]
        public static void GlobalSetup()
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("./logs/test-log.txt",
                              rollingInterval: RollingInterval.Day, // Creates a new log file each day
                              outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
            .Enrich.WithThreadId()
            .CreateLogger();

            Log.Information("Trying to instantiate a browser...");
            browserFactory = BrowserFactory.Instance;
            Log.Information("Browser instantiated.");
        }

        [SetUp]
        public async Task Setup()
        {
            browser.Value = await browserFactory.GetBrowser();

            //setting up context to record videos
            context = await browser.Value.NewContextAsync( new BrowserNewContextOptions
            {
                RecordVideoDir = "videos/",
                RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
            });

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
        [Category("Regression")]
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
            await page.ScreenshotAsync(new()
            {
                Path = "screenshot.png",
                FullPage = true,
            });

            await page.CloseAsync();
            await context.CloseAsync();

            if (browser.Value != null)
            {
                await browser.Value.CloseAsync();
                browser.Value = null;
            }
        }

        [OneTimeTearDown]
        public void TearDownLogging()
        {
            Log.CloseAndFlush();
        }
    }
}
