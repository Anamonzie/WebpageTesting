using Microsoft.Playwright;

namespace EpamWeb.Factory.FactoryInterfaces
{
    public interface IBrowserFactory
    {
        Task<IBrowser> GetBrowser();
    }
}
