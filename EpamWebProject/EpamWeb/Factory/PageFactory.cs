using EpamWeb.Pages;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class PageFactory : IPageFactory
    {
        private static readonly ThreadLocal<PageFactory?> threadLocalInstance = new();
        private readonly IPage page;

        private PageFactory(IPage page)
        {
            this.page = page;
        }

        public static IPageFactory Instance(IPage page)
        {
            if (threadLocalInstance.Value == null)
            {
                threadLocalInstance.Value = new PageFactory(page);
            }

            return threadLocalInstance.Value;
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
