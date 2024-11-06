using EpamWeb.Loggers;
using EpamWeb.Pages;
using Microsoft.Playwright;

namespace EpamWeb.Services
{
    public class InsightsPageService : IInsightsPageService
    {
        private readonly IInsightsPage insightsPage;
        private readonly IPage page;
        private readonly ILoggerManager logger;

        public InsightsPageService(IInsightsPage insightsPage, IPage page, ILoggerManager logger)
        {
            this.insightsPage = insightsPage;
            this.page = page;
            this.logger = logger;
        }

        ///  * METHODS * ///  

        public async Task NavigateToUrlAsync(string url)
        {
            const int maxRetries = 3;
            const int retryDelayMs = 2000;
            int attempt = 0;

            while (true)
            {
                try
                {
                    await page.GotoAsync(url, new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.NetworkIdle
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
                        Timeout = 60000,
                        WaitUntil = WaitUntilState.NetworkIdle
                    });

                    logger.Info("Clicking on 'Accept All' cookies button");
                    await insightsPage.CookiesAcceptButton.ClickAsync();

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
                logger.Info("Getting page title");
                return await page.TitleAsync();
            }
            catch (Exception ex)
            {
                logger.Error("Failed to get page title", ex);
                throw;
            }
        }

        public async Task ClickFindButtonAsync()
        {
            try
            {
                logger.Info("Clicking on Find button");
                await insightsPage.FindButton.ClickAsync();
            }
            catch (Exception ex)
            {
                logger.Error("Failed to click on Find button", ex);
            }
        }

        public async Task InputTextInSearchFieldAsync()
        {
            try
            {
                logger.Info("Inputting some text in the search field");
                await insightsPage.SearchField.FillAsync("Cloud");
            }
            catch (Exception ex)
            {
                logger.Error("Couldn't input text in search field", ex);
            }
        }

        public async Task<string> GetSearchResultTextAsync()
        {
            try
            {
                return await insightsPage.SearchResult.TextContentAsync() ?? string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error($"Found {ex}");
                return string.Empty;
            }
        }
    }
}
