using EpamWeb.Loggers;
using EpamWeb.Pages;
using Microsoft.Playwright;
using NUnit.Framework;

namespace EpamWeb.Services
{
    public class HomepageService : IHomepageService
    {
        private readonly IHomepage homepage;
        private readonly IPage page;
        private readonly ILoggerManager logger;

        public HomepageService(IHomepage homepage, IPage page, ILoggerManager logger)
        {
            this.homepage = homepage;
            this.page = page;
            this.logger = logger;
        }

        /// * METHODS * ///

        public async Task NavigateToUrlAsync(string url)
        {
            int retryTimes = 3;
            int retryDelayMs = 2000;

            for (int i = 0; i < retryTimes; i++)
            {
                try
                {
                    await page.GotoAsync(url, new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.NetworkIdle
                    });

                    logger.Info(TestContext.CurrentContext.Test.Name, $"Successfully navigated to {url}.");
                    break; // Exit immediately on successful navigation
                }
                catch (PlaywrightException ex) when (ex.Message.Contains("net::ERR_ABORTED"))
                {
                    logger.Warn(TestContext.CurrentContext.Test.Name, $"Navigation to {url} failed with 'net::ERR_ABORTED'. Retrying, attempt: {i}");

                    await Task.Delay(retryDelayMs); // Wait before retrying
                }
            }
        }

        public async Task NavigateToUrlAndAcceptCookiesAsync(string url)
        {
            int retryTimes = 3;
            int retryDelayMs = 2000;

            for (int i = 0; i < retryTimes; i++)
            {
                try
                {
                    await page.GotoAsync(url, new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.NetworkIdle
                    });

                    logger.Info(TestContext.CurrentContext.Test.Name, $"Successfully navigated to {url}.");
                    break;
                }
                catch (PlaywrightException ex) when (ex.Message.Contains("net::ERR_ABORTED"))
                {
                    logger.Warn(TestContext.CurrentContext.Test.Name, $"Navigation to {url} failed with 'net::ERR_ABORTED'. Retrying, attempt: {i}");

                    await Task.Delay(retryDelayMs); 
                }
            }
            if (await homepage.CookiesAcceptButton.IsVisibleAsync())
            {
                logger.Info(TestContext.CurrentContext.Test.Name, "Clicking on 'Accept All' cookies button");
                await homepage.CookiesAcceptButton.ClickAsync();
            }
        }

        public async Task<string> GetPageTitleAsync()
        {
            try
            {
                logger.Info(TestContext.CurrentContext.Test.Name, $"Getting page title");
                return await page.TitleAsync();
            }
            catch (Exception ex)
            {
                logger.Error(TestContext.CurrentContext.Test.Name, "Failed to get page title", ex);
                throw;
            }
        }

        public async Task ClickHamburgerMenuAsync()
        {
            try
            {
                logger.Info(TestContext.CurrentContext.Test.Name, "Clicking on the Hamburger Menu Button");
                await homepage.HamburgerButton.ClickAsync();
            }
            catch (Exception ex)
            {
                logger.Error(TestContext.CurrentContext.Test.Name, "Failed to click on the Hamburger Menu Button", ex);
                throw;
            }
        }

        public async Task<List<string>> GetHamburgerMenuListItemsAsync()
        {
            try
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
            catch (Exception ex)
            {
                logger.Error(TestContext.CurrentContext.Test.Name, "Couldn't get the items from Hamburger Menu list", ex);
                throw;
            }
        }

        public async Task ClickAcceptAllCookies()
        {
            try
            {
                logger.Info(TestContext.CurrentContext.Test.Name, "Clicking on 'Accept All' cookies button");
                await homepage.CookiesAcceptButton.ClickAsync();
            }
            catch (Exception ex)
            {
                logger.Error(TestContext.CurrentContext.Test.Name, "Failed to click on 'Accept All' cookies button", ex);
            }
        }
    }
}