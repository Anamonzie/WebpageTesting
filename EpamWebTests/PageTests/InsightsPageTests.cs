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

            context = await browser.Value.NewContextAsync( new BrowserNewContextOptions
            {
                RecordVideoDir = "videos/",
                RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
            });
            Log.Information("Video recording initialized for context. (Insigts Page Tests)");

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

            Log.Information("Navigating to EPAM insights page. (Insigts Page Tests: Search Check)");
            await insightsPageService.NavigateToUrlAsync(Constants.EpamInsightsPageUrl);

            // Act
            await insightsPageService.InputTextInSearchFieldAsync();
            Log.Information("Inputted text in search field. (Insigts Page Tests: Search Check)");

            await insightsPageService.ClickFindButtonAsync();
            Log.Information("Clicked Find Button. (Insigts Page Tests: Search Check)");

            var result = await insightsPageService.GetSearchResultTextAsync();

            // Assert
            result.Should().Contain(TestData.SearchInput);
            Log.Information($"Checking page title; expected: {await insightsPageService.GetPageTitleAsync()}, actual: {result}. (Insigts Page Tests: Search Check)");
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

            Log.Information("Navigating to EPAM insights page. (Insigts Page Tests: Find Button Check)");
            await insightsPageService.NavigateToUrlAsync(Constants.EpamInsightsPageUrl);

            const string expectedTitle = TestData.ExpectedSearchPageTitle;

            // Act
            await insightsPageService.ClickFindButtonAsync();
            var result = await insightsPageService.GetPageTitleAsync();

            // Assert
            result.Should().Be(expectedTitle);
            Log.Information($"Checking page title; expected: {expectedTitle}, actual: {result}. (Insigts Page Tests: Find Button Check)");
        }

        [TearDown]
        public async Task GlobalTearDown()
        {
            if (page != null && !page.IsClosed)
            {
                // Capture a screenshot before closing
                var screenshotsDirectory = Path.Combine("Screenshots", TestContext.CurrentContext.Test.Name);
                Directory.CreateDirectory(screenshotsDirectory);                
                var screenshotPath = Path.Combine(screenshotsDirectory, $"screenshot_{DateTime.UtcNow:MMdd_HHmm}.png");
                Log.Information($"Captured screenshot at {screenshotPath}. (Insigts Page Tests)");

                await page.ScreenshotAsync(new()
                {
                    Path = screenshotPath,
                    FullPage = true,
                });

                AllureApi.AddAttachment("Screenshot", "image/png", screenshotPath);

                await page.CloseAsync();
            }

            if (context != null)
            {
                await context.CloseAsync();

                var path = await page.Video.PathAsync();

                var videoPath = Path.Combine("videos", path);
                AllureApi.AddAttachment("Test Video", "video/webm", videoPath);

                Log.Information($"Test video saved at {videoPath}. (Insigts Page Tests)");
                Log.Information("Page and context closed after test. (Insigts Page Tests)");
            }
        }

        [OneTimeTearDown]
        public static void TearDownLogging()
        {
            Log.CloseAndFlush();

            var logDirectory = new DirectoryInfo("./logs");
            var latestLogFile = logDirectory.GetFiles("test-log*.txt")
                                            .OrderByDescending(f => f.LastWriteTime)
                                            .FirstOrDefault();

            if (latestLogFile != null && latestLogFile.Exists)
            {
                AllureApi.AddAttachment("Test Logs", "text/plain", File.ReadAllText(latestLogFile.FullName));
            }
        }
    }
}
