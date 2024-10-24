using Microsoft.Playwright;

namespace Theory_inPractice.BrowserFactory
{
    public interface IBrowserFactory
    {
        Task<IBrowser> GetBrowser();
    }
}
