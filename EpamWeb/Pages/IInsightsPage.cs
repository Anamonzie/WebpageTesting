using Microsoft.Playwright;

namespace EpamWeb.Pages
{
    public interface IInsightsPage
    {
        ILocator FindButton { get; }
        ILocator SearchField { get; }
        ILocator SearchResult { get; }
    }
}
