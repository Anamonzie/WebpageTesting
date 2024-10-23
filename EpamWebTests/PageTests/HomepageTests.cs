using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Config;
using EpamWeb.Factory;
using EpamWeb.Utils;
using FluentAssertions;
using Microsoft.Playwright;
using SeverityLevel = Allure.Net.Commons.SeverityLevel;

namespace EpamWebTests.PageTests;

[AllureNUnit]
[TestFixture]
[AllureSuite("EPAM Homepage Tests")]
public class Tests
{
    private static IBrowserFactory factory;
    private IPageFactory pageFactory;
    private IServiceFactory serviceFactory;
    private static IConfigurationManager configurationManager;

    private static readonly ThreadLocal<IBrowser> browser = new();
    private IBrowserContext context;
    private IPage page;

    [OneTimeSetUp]
    public static void GlobalSetup()
    {
        configurationManager = new ConfigurationManager();
        factory = BrowserFactory.Instance(configurationManager);
    }

    [SetUp]
    public async Task Setup()
    {
        browser.Value = await factory.GetBrowser();
        context = await browser.Value.NewContextAsync();
        page = await context.NewPageAsync();

        pageFactory = PageFactory.Instance(page);
        serviceFactory = ServiceFactory.Instance(pageFactory, page);
    }

    [Test]
    [AllureName("EPAM Homepage Title Check")]
    [AllureDescription("Checks if the title of the EPAM homepage is as expected.")]
    [AllureSeverity(SeverityLevel.critical)]
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
    [AllureTag("Smoke", "HamburgerMenu")]
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
        if (browser.Value != null)
        {
            await browser.Value.CloseAsync();
            browser.Value = null;
        }
    }
}
