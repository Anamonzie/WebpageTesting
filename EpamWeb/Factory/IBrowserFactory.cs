using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public interface IBrowserFactory
    {
        Task<IBrowser> GetBrowser();
    }
}
