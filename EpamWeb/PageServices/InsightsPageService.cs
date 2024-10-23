using EpamWeb.Pages;
using Microsoft.Playwright;

namespace EpamWeb.PageServices
{
    public class InsightsPageService : IInsightsPageService
    {
        private readonly IInsightsPage insightsPage;
        private readonly IPage page;

        public InsightsPageService(IInsightsPage insightsPage, IPage page)
        {
            this.insightsPage = insightsPage;
            this.page = page;
        }

        ///  * METHODS * ///
        public async Task NavigateToUrlAsync(string url)
        {
            await page.GotoAsync(url);
        }

        public async Task<string> GetPageTitleAsync()
        {
            return await page.TitleAsync();
        }

        public async Task ClickFindButtonAsync()
        {
            await insightsPage.FindButton.ClickAsync();
        }

        public async Task InputTextInSearchFieldAsync()
        {
            await insightsPage.SearchField.FillAsync("Cloud");
        }

        public async Task<string> GetSearchResultTextAsync()
        {
            return await insightsPage.SearchResult.TextContentAsync() ?? string.Empty;
        }
    }
}
