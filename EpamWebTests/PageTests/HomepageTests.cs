using Allure.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb;
using EpamWeb.Factory;
using FluentAssertions;
using Microsoft.Playwright;

namespace EpamWebTests.PageTests;

[AllureNUnit]
[TestFixture]
[AllureSuite("EPAM Homepage Tests")]
public class Tests
{
    private IBrowserFactory factory;
    private IPageFactory pageFactory;

    private static readonly ThreadLocal<IBrowser> browser = new();
    private IBrowserContext context;
    private IPage page;

    [SetUp]
    public async Task Setup()
    {
        factory = BrowserFactory.Instance;
        pageFactory = new PageFactory();

        browser.Value = await factory.GetBrowser();
        context = await browser.Value.NewContextAsync();
        page = await context.NewPageAsync();
    }

    [Test]
    [AllureName("EPAM Homepage Title Check")]
    [AllureDescription("Checks if the title of the EPAM homepage is as expected.")]
    [AllureTag("Smoke", "Homepage")]
    public async Task EpamHomepage_TitleCheck()
    {
        // Arrange
        const string expectedTitle = TestData.ExpectedHomepageTitle;

        var homepageService = pageFactory.CreateHomepageService(page);
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

        var homepageService = pageFactory.CreateHomepageService(page);
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

