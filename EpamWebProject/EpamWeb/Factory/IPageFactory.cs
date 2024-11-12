using EpamWeb.Pages;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public interface IPageFactory
    {
        IPage GetPage();
        IHomepage CreateHomepage();
        IInsightsPage CreateInsightsPage();
    }
}