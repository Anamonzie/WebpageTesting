using EpamWeb.Loggers;
using EpamWeb.Pages;
using Microsoft.Playwright;
using NUnit.Framework;
using Polly;
using Polly.Retry;

namespace EpamWeb.Services
{
    public class HomepageService : IHomepageService
    {
        private readonly IHomepage homepage;
        private readonly IPage page;
        private readonly ILoggerManager logger;
        private readonly AsyncRetryPolicy retryPolicy;

        public HomepageService(IHomepage homepage, IPage page, ILoggerManager logger)
        {
            this.homepage = homepage;
            this.page = page;
            this.logger = logger;

            retryPolicy = Policy
                .Handle<PlaywrightException>(ex => ex.Message.Contains("net::ERR_ABORTED"))
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(3000),
                (exception, timeSpan, retryCount, context) =>
                {
                    logger.Warn(TestContext.CurrentContext.Test.Name,
                        $"Attempt {retryCount}: Navigation failed with '{exception.Message}'. Retrying in {timeSpan}.");
                });
        }

        /// * METHODS * ///
        public async Task NavigateToUrlAsync(string url)
        {
            await retryPolicy.ExecuteAsync(async () =>
            {
                await page.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                logger.Info(TestContext.CurrentContext.Test.Name, $"Successfully navigated to {url}.");
            });
        }

        public async Task NavigateToUrlAndAcceptCookiesAsync(string url)
        {
            await retryPolicy.ExecuteAsync(async () =>
            {
                await page.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                logger.Info(TestContext.CurrentContext.Test.Name, $"Successfully navigated to {url}.");
            });

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