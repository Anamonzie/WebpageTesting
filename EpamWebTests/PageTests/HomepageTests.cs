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


        //setting up context to record videos
        context = await browser.Value.NewContextAsync(new BrowserNewContextOptions
        {            
            RecordVideoDir = "videos/",
            RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
        });

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

        // Act
        var result = await homepageService.GetPageTitleAsync();

        // Assert
        result.Should().Be(expectedTitle);
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

        // Act
        await homepageService.ClickHamburgerMenuAsync();
        var actualItems = await homepageService.GetHamburgerMenuListItemsAsync();

        // Assert
        actualItems.Should().BeEquivalentTo(expectedItems);
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
