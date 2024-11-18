using Allure.NUnit;
using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using EpamWeb.Utils;

using FluentAssertions;
using SeverityLevel = Allure.Net.Commons.SeverityLevel;

namespace EpamWebTests.PageTests;

[TestFixture]
[AllureFeature("EPAM Homepage Tests")]
public class Tests : BaseTest
{
    [Test]
    [AllureName("Google Title Check")]
    [AllureSeverity(SeverityLevel.minor)]
    [Category("Integration")]
    [AllureTag("GoogleTest")]
    public async Task Google_NetworkConnectionCheck()
    {
        AllureApi.SetSeverity(SeverityLevel.minor);
        AllureApi.SetOwner("John Doe");
        AllureApi.SetDescription("Checking Google Connection");
        // Arrange
        const string expectedTitle = TestData.ExpectedGoogleTitle;
        var testPage = await pageFactory.GetOrCreatePageAsync(TestContext.CurrentContext.Test.Name);

        await testPage.GotoAsync(ConstantData.GoogleUrl);

        // Act
        var result = await testPage.TitleAsync();

        // Assert
        result.Should().Be(expectedTitle);
        logger.Info(TestContext.CurrentContext.Test.Name, $"Checking page title; expected: {expectedTitle}, actual: {result}.");
    }

    [Test]
    [AllureName("EPAM Homepage Title Check")]
    [Category("Integration")]
    [AllureTag("HamburgerMenu")]
    [AllureSeverity(SeverityLevel.minor)]
    public async Task EpamHomepage_TitleCheck()
    {
        // Arrange
        const string expectedTitle = TestData.ExpectedHomepageTitle;
        var testPage = await pageFactory.GetOrCreatePageAsync(TestContext.CurrentContext.Test.Name);

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
    [Category("Smoke")]
    [AllureTag("HamburgerMenu")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task EpamHomepage_HamburgerMenu()
    {
        // Arrange
        var expectedItems = TestData.ExpectedHamburgerMenuItems;
        var testPage = await pageFactory.GetOrCreatePageAsync(TestContext.CurrentContext.Test.Name);

        var homepageService = serviceFactory.CreateHomepageService(testPage);
        await homepageService.NavigateToUrlAndAcceptCookiesAsync(ConstantData.EpamHomepageUrl);

        // Act
        await homepageService.ClickHamburgerMenuAsync();
        var actualItems = await homepageService.GetHamburgerMenuListItemsAsync();

        // Assert
        actualItems.Should().BeEquivalentTo(expectedItems);
        logger.Info(TestContext.CurrentContext.Test.Name, $"Menu items are: {string.Join(", ", actualItems)}");
    }
}
