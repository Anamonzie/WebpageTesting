using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Attachments;
using EpamWeb.Config;
using EpamWeb.Factory;
using EpamWeb.Loggers;
using EpamWeb.Services;
using EpamWeb.Utils;
using FluentAssertions;
using Microsoft.Playwright;

namespace EpamWebTests.PageTests
{
    [AllureNUnit]
    [TestFixture]
    [AllureSuite("Insights Page Tests")]
    public class InsightsPageTests : BaseTest
    {
        private static readonly ThreadLocal<IBrowser> browser = new();

        private ILoggerManager logger;
        private IPageFactory pageFactory;
        private IServiceFactory serviceFactory;
        private IBrowserContext context;
        private IPage page;

        private IMediaCaptureService mediaCaptureService;
        private IAllureAttachmentManager allureAttachmentManager;

        [SetUp]
        public async Task Setup()
        {
            logger = new LoggerManager(ConfigManager.Instance);
            logger.Info("STARTING NEW RUN");
            logger.Info("Setting up test context");

            mediaCaptureService = new MediaCaptureService(logger);
            allureAttachmentManager = new AllureAttachmentManager();

            browser.Value ??= await browserFactory.GetBrowser();
            context = await browser.Value.NewContextAsync(mediaCaptureService.StartVideoRecordingAsync());
            page = await context.NewPageAsync();
            pageFactory = PageFactory.Instance(page);
            serviceFactory = ServiceFactory.Instance(pageFactory, page, logger);
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

            await insightsPageService.NavigateToUrlAndAcceptCookiesAsync(Constants.EpamInsightsPageUrl);

            // Act
            await insightsPageService.InputTextInSearchFieldAsync();
            await insightsPageService.ClickFindButtonAsync();

            var result = await insightsPageService.GetSearchResultTextAsync();

            // Assert
            result.Should().Contain(TestData.SearchInput);
            logger.Info($"Checking page title; expected to contain: {TestData.SearchInput}, actual: {result}. (Insights Page Tests: Search Check)");
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
            await insightsPageService.NavigateToUrlAndAcceptCookiesAsync(Constants.EpamInsightsPageUrl);
            const string expectedTitle = TestData.ExpectedSearchPageTitle;

            // Act
            await insightsPageService.ClickFindButtonAsync();
            await Task.Delay(TimeSpan.FromSeconds(2)); // 2-second delay - temporal check
            var result = await insightsPageService.GetPageTitleAsync();

            // Assert
            result.Should().Be(expectedTitle);
            logger.Info($"Checking page title; expected: {expectedTitle}, actual: {result}. (Insights Page Tests: Find Button Check)");
        }

        [TearDown]
        public async Task TearDown()
        {
            if (page != null && !page.IsClosed)
            {
                var screenshotPath = await mediaCaptureService.CaptureScreenshot(page);
                await allureAttachmentManager.AddScreenshotAttachment(screenshotPath);

                await context.CloseAsync();
                await allureAttachmentManager.AddVideoAttachment(page);
            }

            logger.CloseAndFlush();

            var testName = TestContext.CurrentContext.Test.Name;
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs", $"{testName}");
            var logFilePath = Path.Combine(logDirectory, $"{testName}-log.txt");

            if (File.Exists(logFilePath))
            {
                allureAttachmentManager.AttachLogToAllure(logFilePath);
            }
        }
    }
}
