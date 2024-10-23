using EpamWeb.Pages;
using Microsoft.Playwright;

namespace EpamWeb.PageServices
{
    public class HomepageService : IHomepageService
    {
        private readonly IHomepage homepage;
        private readonly IPage page;

        public HomepageService(IHomepage homepage, IPage page)
        {
            this.homepage = homepage;
            this.page = page;
        }

        /// * METHODS * ///

        public async Task NavigateToUrlAsync(string url)
        {
            await page.GotoAsync(url);
        }

        public async Task<string> GetPageTitleAsync()
        {
            return await page.TitleAsync();
        }

        public async Task ClickHamburgerMenuAsync()
        {
            await homepage.HamburgerButton.ClickAsync();
        }

        public async Task<List<string>> GetHamburgerMenuListItemsAsync()
        {
            var menuVisible = homepage.HamburgerButtonNavigation;

            await menuVisible.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

            var menuItems = await homepage.HamburgerMenuItems.AllAsync();
            var menuItemsList = new List<string>();

            foreach (var item in menuItems)
            {
                menuItemsList.Add(await item.InnerTextAsync());
            }

            return menuItemsList;
        }
    }
}