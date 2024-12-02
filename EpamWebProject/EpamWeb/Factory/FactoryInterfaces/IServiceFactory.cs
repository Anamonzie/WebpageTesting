using EpamWeb.Services.ServiceInterfaces;
using Microsoft.Playwright;

namespace EpamWeb.Factory.FactoryInterfaces
{
    public interface IServiceFactory
    {
        IHomepageService CreateHomepageService(IPage page);
        IInsightsPageService CreateInsightsPageService(IPage page);
    }
}
