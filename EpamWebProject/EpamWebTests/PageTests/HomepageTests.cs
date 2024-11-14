using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Attachments;
using EpamWeb.Factory;
using EpamWeb.Services;
using EpamWeb.Utils;
using FluentAssertions;
using Microsoft.Playwright;
using System.Collections.Concurrent;
using SeverityLevel = Allure.Net.Commons.SeverityLevel;

namespace EpamWebTests.PageTests;

[AllureNUnit]
[TestFixture]
[AllureSuite("EPAM Homepage Tests")]
public class Tests : BaseTest
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
        serviceFactory = ServiceFactory.CreateInstance(pageFactory, page, logger);
    }

    [Test]
    [AllureName("Google Title Check")]
    [AllureDescription("Checks the title to see if network error persists with Google as well.")]
    [AllureSeverity(SeverityLevel.minor)]
    [AllureLabel("Environment", "Staging")]
    [AllureLabel("Component", "Login")]
    public async Task Google_NetworkConnectionCheck()
    {
        // Arrange
        const string expectedTitle = TestData.ExpectedGoogleTitle;
        var testPage = Pages[TestContext.CurrentContext.Test.Name];

        await testPage.GotoAsync(ConstantData.GoogleUrl);

        // Act
        var result = await testPage.TitleAsync();

        // Assert
        result.Should().Be(expectedTitle);
        logger.Info(TestContext.CurrentContext.Test.Name, $"Checking page title; expected: {expectedTitle}, actual: {result}.");
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
        var testPage = Pages[TestContext.CurrentContext.Test.Name];

        var homepageService = serviceFactory.CreateHomepageService(testPage);
        await homepageService.NavigateToUrlAndAcceptCookiesAsync(ConstantData.EpamHomepageUrl);

        // Act
        var result = await homepageService.GetPageTitleAsync();

        // Assert
        result.Should().Be(expectedTitle);
        logger.Info(TestContext.CurrentContext.Test.Name, $"Checking page title; expected: {expectedTitle}, actual: {result}.");
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
        var testPage = Pages[TestContext.CurrentContext.Test.Name];

        var homepageService = serviceFactory.CreateHomepageService(testPage);
        await homepageService.NavigateToUrlAndAcceptCookiesAsync(ConstantData.EpamHomepageUrl);

        // Act
        await homepageService.ClickHamburgerMenuAsync();
        var actualItems = await homepageService.GetHamburgerMenuListItemsAsync();

        // Assert
        actualItems.Should().BeEquivalentTo(expectedItems);
        logger.Info(TestContext.CurrentContext.Test.Name, $"Menu items are: {string.Join(", ", actualItems)}");
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
}
