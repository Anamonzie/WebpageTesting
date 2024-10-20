using EpamWeb.Pages;
using EpamWeb.PageServices;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IPageFactory pageFactory;
        private readonly IPage page;

        public ServiceFactory(IPageFactory pageFactory, IPage page)
        {
            this.pageFactory = pageFactory;
            this.page = page;
        }

        public IHomepageService CreateHomepageService()
        {
            IHomepage homepage = pageFactory.CreateHomepage();
            return new HomepageService(homepage, page);
        }

        public IInsightsPageService CreateInsightsPageService()
        {
            IInsightsPage insightsPage = pageFactory.CreateInsightsPage();
            return new InsightsPageService(insightsPage, page);
        }
    }
}
