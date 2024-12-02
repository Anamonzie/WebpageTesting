using EpamWeb.Factory.FactoryInterfaces;
using EpamWeb.Loggers;
using EpamWeb.Pages;
using EpamWeb.Services;
using EpamWeb.Services.ServiceInterfaces;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IPageFactory pageFactory;
        private readonly ILoggerManager logger;

        private ServiceFactory(IPageFactory pageFactory, IPage page, ILoggerManager logger)
        {
            this.pageFactory = pageFactory;
            this.logger = logger;
        }

        public static IServiceFactory CreateInstance(IPageFactory pageFactory, IPage page, ILoggerManager logger)
        {
            return new ServiceFactory(pageFactory, page, logger);
        }

        public IHomepageService CreateHomepageService(IPage page)
        {
            IHomepage homepage = pageFactory.CreateHomepage(page);
            return new HomepageService(homepage, page, logger);
        }

        public IInsightsPageService CreateInsightsPageService(IPage page)
        {
            IInsightsPage insightsPage = pageFactory.CreateInsightsPage(page);
            return new InsightsPageService(insightsPage, page, logger);
        }
    }
}
