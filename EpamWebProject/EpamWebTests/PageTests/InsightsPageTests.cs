using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Attachments;
using EpamWeb.Factory;
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

        private IPageFactory pageFactory;
        private IServiceFactory serviceFactory;
        private IBrowserContext context;
        private IPage page;

        private MediaCaptureService mediaCaptureService;

        [SetUp]
        public async Task Setup()
        {
            logger.Info("setting up test context");

            mediaCaptureService = new MediaCaptureService(logger);
            browser.Value = await browserFactory.GetBrowser();

            //context = await browser.Value.NewContextAsync();
            context = await browser.Value.NewContextAsync(MediaCaptureService.StartVideoRecordingAsync());

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

            await insightsPageService.NavigateToUrlAsync(Constants.EpamInsightsPageUrl);

            // Act
            await insightsPageService.ClickAcceptAllCookies();
            await insightsPageService.InputTextInSearchFieldAsync();
            await insightsPageService.ClickFindButtonAsync();

            var result = await insightsPageService.GetSearchResultTextAsync();

            // Assert
            result.Should().Contain(TestData.SearchInput);
            logger.Info($"Checking page title; expected: {await insightsPageService.GetPageTitleAsync()}, actual: {result}. (Insights Page Tests: Search Check)");
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
            await insightsPageService.ClickAcceptAllCookies();

            await insightsPageService.ClickFindButtonAsync();
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
                await AllureAttachmentManager.AddScreenshotAttachment(screenshotPath);

                await context.CloseAsync();
                await AllureAttachmentManager.AddVideoAttachment(page);
            }

            //if (page != null && !page.IsClosed)
            //{
            //    await AllureAttachmentManager.AddVideoAttachment(page);

            //    //var screenshotPath = await mediaCaptureService.CaptureScreenshot(page);
            //    ////await AllureAttachmentManager.AddScreenshotAttachment(screenshotPath);

            //    await page.CloseAsync();
            //}

            //if (context != null)
            //{
            //    await context.CloseAsync();
            //}

            // logger.Info("Page and context closed after test. (Insights Page Tests)");
            //logger.CloseAndFlush();
            //// AllureAttachmentManager.AttachLogToAllure();
        }
    }
}
