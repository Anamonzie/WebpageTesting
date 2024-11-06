using EpamWeb.Loggers;
using EpamWeb.Pages;
using Microsoft.Playwright;

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
            const int maxRetries = 3;
            const int retryDelayMs = 2000;
            int attempt = 0;

            while (true)
            {
                try
                {
                    logger.Info($"Navigating to {url} (Attempt {attempt + 1}/{maxRetries})");

                    await page.GotoAsync(url, new PageGotoOptions
                    {
                        Timeout = 60000,
                    });

                    logger.Info($"Successfully navigated to {url}.");
                    return; // Exit immediately on successful navigation
                }
                catch (PlaywrightException ex) when (ex.Message.Contains("net::ERR_ABORTED"))
                {
                    attempt++;
                    logger.Warn($"Navigation to {url} failed with 'net::ERR_ABORTED'. Retrying {attempt}/{maxRetries}...");

                    if (attempt >= maxRetries)
                    {
                        logger.Error($"Failed to navigate to {url} after {maxRetries} attempts due to net::ERR_ABORTED.", ex);
                        throw;
                    }

                    await Task.Delay(retryDelayMs); // Wait before retrying
                }
                catch (Exception ex)
                {
                    logger.Error($"An unexpected error occurred while navigating to {url}.", ex);
                    throw;
                }
            }
        }

        public async Task NavigateToUrlAndAcceptCookiesAsync(string url)
        {
            const int maxRetries = 3;
            const int retryDelayMs = 2000;
            int attempt = 0;

            while (true)
            {
                try
                {
                    logger.Info("Navigating to EPAM insights page");

                    await page.GotoAsync(url, new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.NetworkIdle
                    });

                    logger.Info("Clicking on 'Accept All' cookies button");
                    await homepage.CookiesAcceptButton.ClickAsync();

                    return;
                }
                catch (PlaywrightException ex) when (ex.Message.Contains("net::ERR_ABORTED"))
                {
                    attempt++;
                    logger.Warn($"Navigation to {url} failed with 'net::ERR_ABORTED'. Retrying {attempt}/{maxRetries}...");

                    if (attempt >= maxRetries)
                    {
                        logger.Error($"Failed to navigate to {url} after {maxRetries} attempts due to net::ERR_ABORTED.", ex);
                        throw;
                    }

                    await Task.Delay(retryDelayMs); // Wait before retrying
                }
                catch (Exception ex)
                {
                    logger.Error($"An unexpected error occurred while navigating to {url}.", ex);
                    throw;
                }
            }
        }

        public async Task<string> GetPageTitleAsync()
        {
            try
            {
                logger.Info($"Getting page title");
                return await page.TitleAsync();
            }
            catch (Exception ex)
            {
                logger.Error("Failed to get page title", ex);
                throw;
            }
        }

        public async Task ClickHamburgerMenuAsync()
        {
            try
            {
                logger.Info("Clicking on the Hamburger Menu Button");
                await homepage.HamburgerButton.ClickAsync();
            }
            catch (Exception ex)
            {
                logger.Error("Failed to click on the Hamburger Menu Button", ex);
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
                logger.Error("Couldn't get the items from Hamburger Menu list", ex);
                throw;
            }
        }

        public async Task ClickAcceptAllCookies()
        {
            try
            {
                logger.Info("Clicking on 'Accept All' cookies button");
                await homepage.CookiesAcceptButton.ClickAsync();
            }
            catch (Exception ex)
            {
                logger.Error("Failed to click on 'Accept All' cookies button", ex);
            }
        }
    }
}