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
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.Concurrent;

namespace EpamWebTests.PageTests
{
    [AllureNUnit]
    [TestFixture]
    [AllureSuite("Insights Page Tests")]
    public class InsightsPageTests : BaseTest
    {
        private static readonly ThreadLocal<IBrowser> browser = new();
        private static readonly ConcurrentDictionary<string, IPage> Pages = new();

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
            var testName = TestContext.CurrentContext.Test.Name;
            logger = new LoggerManager(ConfigManager.Instance, testName);
            logger.Info("STARTING NEW RUN");
            logger.Info("Setting up test context");

            mediaCaptureService = new MediaCaptureService(logger);
            allureAttachmentManager = new AllureAttachmentManager();

            browser.Value ??= await browserFactory.GetBrowser();
            context = await browser.Value.NewContextAsync(mediaCaptureService.StartVideoRecordingAsync());

            page = await context.NewPageAsync();
            Pages[TestContext.CurrentContext.Test.Name] = page;

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
            var testPage = Pages[TestContext.CurrentContext.Test.Name];
            var insightsPageService = serviceFactory.CreateInsightsPageService(testPage);

            await insightsPageService.NavigateToUrlAndAcceptCookiesAsync(ConstantData.EpamInsightsPageUrl);

            // Act
            await insightsPageService.InputTextInSearchFieldAsync();
            await insightsPageService.ClickFindButtonAsync();

            var result = await insightsPageService.GetSearchResultTextAsync();

            // Assert
            result.Should().Contain(TestData.SearchInput);
            logger.Info($"Checking page title; expected to contain: {TestData.SearchInput}, actual: {result}.");
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
            logger.Info($"Checking page title; expected: {expectedTitle}, actual: {result}.");
        }

        [TearDown]
        public async Task TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;

            if (Pages.TryRemove(testName, out var testPage) && !testPage.IsClosed)
            {
                var screenshotPath = await mediaCaptureService.CaptureScreenshot(page);
                await allureAttachmentManager.AddScreenshotAttachment(screenshotPath);

                await testPage.CloseAsync();
                await testPage.Context.CloseAsync();
                await allureAttachmentManager.AddVideoAttachment(page);
            }

            logger.CloseAndFlush();

            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs", $"{testName}");
            var logFilePath = Path.Combine(logDirectory, $"{testName}-log.txt");

            if (File.Exists(logFilePath))
            {
                allureAttachmentManager.AttachLogToAllure(logFilePath);
            }
        }
    }
}