using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Attachments;
using EpamWeb.Factory;
using EpamWeb.Services;
using EpamWeb.Utils;
using FluentAssertions;
using Microsoft.Playwright;
using SeverityLevel = Allure.Net.Commons.SeverityLevel;

namespace EpamWebTests.PageTests;

[AllureNUnit]
[TestFixture]
[AllureSuite("EPAM Homepage Tests")]
public class Tests : BaseTest
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
        browser.Value ??= await browserFactory.GetBrowser();

        //context = await browser.Value.NewContextAsync();
        context = await browser.Value.NewContextAsync(MediaCaptureService.StartVideoRecordingAsync());

        page = await context.NewPageAsync();
        pageFactory = PageFactory.Instance(page);
        serviceFactory = ServiceFactory.Instance(pageFactory, page, logger);
    }

    [Test]
    [AllureName("Google Title Check")]
    [AllureDescription("Checks the title to see if network error persists with Google as well.")]
    [AllureSeverity(SeverityLevel.minor)]
    public async Task Google_NetworkConnectionCheck()
    {
        // Arrange
        const string expectedTitle = TestData.ExpectedGoogleTitle;

        await page.GotoAsync(Constants.GoogleUrl);

        // Act
        var result = await page.TitleAsync();

        // Assert
        result.Should().Be(expectedTitle);
        logger.Info($"Checking page title; expected: {expectedTitle}, actual: {result}. (Google Tests: Title Check)");
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
        await homepageService.ClickAcceptAllCookies();

        var result = await homepageService.GetPageTitleAsync();

        // Assert
        result.Should().Be(expectedTitle);
        logger.Info($"Checking page title; expected: {expectedTitle}, actual: {result}. (Homepage Tests: Title Check)");
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
        await homepageService.ClickAcceptAllCookies();
        await homepageService.ClickHamburgerMenuAsync();
        var actualItems = await homepageService.GetHamburgerMenuListItemsAsync();

        // Assert
        actualItems.Should().BeEquivalentTo(expectedItems);
        logger.Info($"Menu items are: {string.Join(", ", expectedItems)}");
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

        if (context != null)
        {
            await context.CloseAsync();
        }
        // logger.Info("Page and context closed after test. (Homepage Tests)");
        // logger.CloseAndFlush();
        ////AllureAttachmentManager.AttachLogToAllure();
    }
}
