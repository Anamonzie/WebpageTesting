using EpamWeb.Pages;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class PageFactory : IPageFactory
    {
        private readonly IPage page;

        public PageFactory(IPage page)
        {
            this.page = page;
        }

        public IHomepage CreateHomepage()
        {
            return new Homepage(page);
        }

        public IInsightsPage CreateInsightsPage()
        {
            return new InsightsPage(page);
        }
    }
}
