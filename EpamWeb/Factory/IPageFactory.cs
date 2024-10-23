using EpamWeb.Pages;

namespace EpamWeb.Factory
{
    public interface IPageFactory
    {
        IHomepage CreateHomepage();
        IInsightsPage CreateInsightsPage();
    }
}