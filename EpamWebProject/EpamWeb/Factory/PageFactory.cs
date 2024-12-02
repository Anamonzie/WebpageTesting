using EpamWeb.Factory.FactoryInterfaces;
using EpamWeb.Pages;
using Microsoft.Playwright;
using System.Collections.Concurrent;

namespace EpamWeb.Factory
{
    public class PageFactory : IPageFactory
    {
        private readonly IBrowserContext context;
        private readonly ConcurrentDictionary<string, IPage> pages;

        public PageFactory(IBrowserContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            pages = new ConcurrentDictionary<string, IPage>();
        }

        public async Task<IPage> GetOrCreatePageAsync(string testName)
        {
            if (!pages.TryGetValue(testName, out var page))
            {
                page = await context.NewPageAsync();
                pages[testName] = page;
            }
            return page;
        }

        public async Task RemovePageAsync(string testName)
        {
            if (pages.TryRemove(testName, out var page) && !page.IsClosed)
            {
                await page.CloseAsync();
            }
        }

        public IHomepage CreateHomepage(IPage page)
        {
            return new Homepage(page);
        }


        public IInsightsPage CreateInsightsPage(IPage page)
        {
            return new InsightsPage(page);
        }
    }
}
