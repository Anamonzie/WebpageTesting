using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Factory;
using EpamWeb.Utils;
using FluentAssertions;
using Microsoft.Playwright;
using Serilog;
using SeverityLevel = Allure.Net.Commons.SeverityLevel;

namespace EpamWebTests.PageTests;

[AllureNUnit]
[TestFixture]
[AllureSuite("EPAM Homepage Tests")]
public class Tests
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

        context = await browser.Value.NewContextAsync(new BrowserNewContextOptions
        {
            RecordVideoDir = "videos/",
            RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
        });
        Log.Information("Video recording initialized for context. (Homepage Tests)");

        page = await context.NewPageAsync();
        pageFactory = PageFactory.Instance(page);
        serviceFactory = ServiceFactory.Instance(pageFactory, page);
    }

    [Test]
    [AllureName("EPAM Homepage Title Check")]
    [AllureDescription("Checks if the title of the EPAM homepage is as expected.")]
    [Category("Integration")]
    [AllureTag("HamburgerMenu")]
    [AllureSeverity(SeverityLevel.minor)]
    public async Task EpamHomepage_TitleCheck()
    {
        // Arrange
        const string expectedTitle = TestData.ExpectedHomepageTitle;

        var homepageService = serviceFactory.CreateHomepageService();
        await homepageService.NavigateToUrlAsync(Constants.EpamHomepageUrl);
        Log.Information("Navigating to EPAM homepage. (Homepage Tests: Title Check)");

        // Act
        var result = await homepageService.GetPageTitleAsync();

        // Assert
        result.Should().Be(expectedTitle);
        Log.Information($"Checking page title; expected: {expectedTitle}, actual: {result}. (Homepage Tests: Title Check)");
    }

    [Test]
    [AllureName("EPAM Homepage Hamburger Menu Check")]
    [AllureDescription("Checks if the hamburger menu items are as expected.")]
    [Category("Smoke")]
    [AllureTag("HamburgerMenu")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task EpamHomepage_HamburgerMenu()
    {
        // Arrange
        var expectedItems = TestData.ExpectedHamburgerMenuItems;

        var homepageService = serviceFactory.CreateHomepageService();
        await homepageService.NavigateToUrlAsync(Constants.EpamHomepageUrl);
        Log.Information("Navigating to EPAM homepage. (Homepage Tests: Hamburger Menu)");

        // Act
        await homepageService.ClickHamburgerMenuAsync();
        Log.Information("Clicked on Hamburger Menu. (Homepage Tests: Hamburger Menu)");

        var actualItems = await homepageService.GetHamburgerMenuListItemsAsync();

        // Assert
        actualItems.Should().BeEquivalentTo(expectedItems);
        Log.Information($"Menu items are: {string.Join(", ", expectedItems)}. (Homepage Tests: Hamburger Menu)");
    }

    [TearDown]
    public async Task TearDown()
    {
        if (page != null && !page.IsClosed)
        {
            var screenshotsDirectory = Path.Combine("Screenshots", TestContext.CurrentContext.Test.Name);
            Directory.CreateDirectory(screenshotsDirectory);
            var screenshotPath = Path.Combine(screenshotsDirectory, $"screenshot_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png");
            Log.Information($"Captured screenshot at {screenshotPath}. (Homepage Tests)");
            
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

            Log.Information($"Test video saved at {videoPath}. (Homepage Tests)");
            Log.Information("Page and context closed after test. (Homepage Tests)");
        }
    }

    [OneTimeTearDown]
    public void TearDownLogging()
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
