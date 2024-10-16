using EpamWeb;
using EpamWeb.Factory;
using EpamWeb.Pages;
using FluentAssertions;
using Microsoft.Playwright;

namespace EpamWebTests;

[TestFixture]
//[Parallelizable(ParallelScope.All)]
public class Tests
{
    private IBrowserFactory factory;

    private static readonly ThreadLocal<IBrowser> browser = new();
    private IBrowserContext context;
    private IPage page;

    [SetUp]
    public async Task Setup()
    {
        factory = BrowserFactory.Instance;

        browser.Value = await factory.GetBrowser();
        context = await browser.Value.NewContextAsync();
        page = await context.NewPageAsync();
    }

    [Test]
    public async Task EpamHomepage_TitleCheck()
    {
        // Arrange
        const string expectedTitle = TestData.ExpectedHomepageTitle;

        var homepageService = PageFactory.CreateHomepageService(page);
        await homepageService.NavigateToUrlAsync(Constants.EpamHomepageUrl);

        // Act
        var result = await homepageService.GetPageTitleAsync();

        // Assert
        result.Should().Be(expectedTitle);
    }

    [Test]
    public async Task EpamHomepage_HamburgerMenu()
    {
        // Arrange
        var expectedItems = TestData.ExpectedHamburgerMenuItems;

        var homepageService = PageFactory.CreateHomepageService(page);
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

