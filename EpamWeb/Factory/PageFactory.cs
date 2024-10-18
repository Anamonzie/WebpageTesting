using EpamWeb.Pages;
using EpamWeb.PageServices;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class PageFactory : IPageFactory
    {
        public IHomepageService CreateHomepageService(IPage page)
        {
            IHomepage homepage = new Homepage(page);
            return new HomepageService(homepage, page);
        }

        public IInsightsPageService CreateInsightsPageService(IPage page)
        {
            IInsightsPage insightspage = new InsightsPage(page);
            return new InsightsPageService(insightspage, page);
        }
    }
}
