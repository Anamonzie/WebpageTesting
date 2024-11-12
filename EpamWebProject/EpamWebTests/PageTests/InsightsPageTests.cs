using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Attachments;
using EpamWeb.Factory;
using EpamWeb.Services;
using EpamWeb.Utils;
using FluentAssertions;
using Microsoft.Playwright;
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

        private IPageFactory pageFactory;
        private IServiceFactory serviceFactory;
        private IBrowserContext context;

        private IMediaCaptureService mediaCaptureService;
        private IAllureAttachmentManager allureAttachmentManager;

        [SetUp]
        public async Task Setup()
        {
            var testName = TestContext.CurrentContext.Test.Name;

            logger.InitializeLogFilePath(TestContext.CurrentContext.Test.Name);

            mediaCaptureService = new MediaCaptureService(logger);
            allureAttachmentManager = new AllureAttachmentManager();

            browser.Value = await browserFactory.GetBrowser();
            context = await browser.Value.NewContextAsync(mediaCaptureService.StartVideoRecordingAsync());

            var page = await context.NewPageAsync();
            Pages[TestContext.CurrentContext.Test.Name] = page;

            pageFactory = new PageFactory(page);
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
            logger.Info(TestContext.CurrentContext.Test.Name, $"Checking page title; expected to contain: {TestData.SearchInput}, actual: {result}.");
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
            logger.Info(TestContext.CurrentContext.Test.Name, $"Checking page title; expected: {expectedTitle}, actual: {result}.");
        }

        [TearDown]
        public async Task TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;

            // Attempt to remove the page for the current test from the dictionary
            if (Pages.TryRemove(TestContext.CurrentContext.Test.Name, out var testPage) && !testPage.IsClosed)
            {
                var screenshotPath = await mediaCaptureService.CaptureScreenshot(testPage);
                await allureAttachmentManager.AddScreenshotAttachment(screenshotPath);

                await testPage.CloseAsync();
                await testPage.Context.CloseAsync();

                await allureAttachmentManager.AddVideoAttachment(testPage);
            }

            logger.CloseAndFlush(TestContext.CurrentContext.Test.Name);

            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs", $"{TestContext.CurrentContext.Test.Name}");
            var logFilePath = Path.Combine(logDirectory, $"{TestContext.CurrentContext.Test.Name}-log.txt");

            if (File.Exists(logFilePath))
            {
                allureAttachmentManager.AttachLogToAllure(logFilePath);
            }
        }

        //[OneTimeTearDown]
        //public async Task OneTimeTearDown()
        //{
        //    // Clean up all contexts and browsers after all tests complete
        //    foreach (var testBrowser in Browsers.Values)
        //    {
        //        await testBrowser.CloseAsync();
        //    }

        //    Browsers.Clear();
        //}
    }
}