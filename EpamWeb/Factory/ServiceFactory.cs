using EpamWeb.Pages;
using EpamWeb.PageServices;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class ServiceFactory : IServiceFactory
    {
        private static readonly ThreadLocal<ServiceFactory?> threadLocalInstance = new();
        private readonly IPageFactory pageFactory;
        private readonly IPage page;

        private ServiceFactory(IPageFactory pageFactory, IPage page)
        {
            this.pageFactory = pageFactory;
            this.page = page;
        }

        public static IServiceFactory Instance(IPageFactory pageFactory, IPage page)
        {
            if (threadLocalInstance.Value == null)
            {
                threadLocalInstance.Value = new ServiceFactory(pageFactory, page);
            }

            return threadLocalInstance.Value;
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