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

                    logger.Info($"Successfully navigated to {url}.");
                    break; // Exit immediately on successful navigation
                }
                catch (PlaywrightException ex) when (ex.Message.Contains("net::ERR_ABORTED"))
                {
                    logger.Warn($"Navigation to {url} failed with 'net::ERR_ABORTED'. Retrying, attempt: {i}");

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

                    logger.Info($"Successfully navigated to {url}.");
                    break; // Exit immediately on successful navigation
                }
                catch (PlaywrightException ex) when (ex.Message.Contains("net::ERR_ABORTED"))
                {
                    logger.Warn($"Navigation to {url} failed with 'net::ERR_ABORTED'. Retrying, attempt: {i}");

                    await Task.Delay(retryDelayMs); // Wait before retrying
                }
            }

            if (await insightsPage.CookiesAcceptButton.IsVisibleAsync())
            {
                logger.Info("Clicking on 'Accept All' cookies button");
                await insightsPage.CookiesAcceptButton.ClickAsync();
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
