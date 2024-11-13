using EpamWeb.Loggers;
using EpamWeb.Pages;
using EpamWeb.Services;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class ServiceFactory : IServiceFactory
    {
        //private static readonly ThreadLocal<ServiceFactory?> threadLocalInstance = new();
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

        //public static IServiceFactory Instance(IPageFactory pageFactory, IPage page, ILoggerManager logger)
        //{
        //    if (threadLocalInstance.Value == null)
        //    {
        //        threadLocalInstance.Value = new ServiceFactory(pageFactory, page, logger);
        //    }

        //    return threadLocalInstance.Value;
        //}

        public IHomepageService CreateHomepageService(IPage page)
        {
            IHomepage homepage = pageFactory.CreateHomepage();
            return new HomepageService(homepage, page, logger);
        }

        public IInsightsPageService CreateInsightsPageService(IPage page)
        {
            IInsightsPage insightsPage = pageFactory.CreateInsightsPage();
            return new InsightsPageService(insightsPage, page, logger);
        }
    }
}