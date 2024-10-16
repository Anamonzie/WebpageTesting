using EpamWeb.PageServices;
using Microsoft.Playwright;

namespace EpamWeb.Pages
{
    public static class PageFactory
    {
        public static HomepageService CreateHomepageService(IPage page)
        {
            IHomepage homepage = new Homepage(page);
            return new HomepageService(homepage, page);
        }

        public static IInsightsPageService CreateInsightsPageService(IPage page)
        {
            IInsightsPage insightspage = new InsightsPage(page);
            return new InsightsPageService(insightspage, page);
        }
    }
}
