using EpamWeb.Loggers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace EpamWeb.Services
{
    public class MediaCaptureService
    {
        private readonly ILoggerManager logger;

        public MediaCaptureService(ILoggerManager logger)
        {
            this.logger = logger;
        }

        public async Task<string> CaptureScreenshot(IPage page)
        {
            var screenshotsDirectory = Path.Combine("Screenshots", TestContext.CurrentContext.Test.Name);
            Directory.CreateDirectory(screenshotsDirectory);
            var screenshotPath = Path.Combine(screenshotsDirectory, $"screenshot_{DateTime.UtcNow:MMdd_HHmm}.png");

            await page.ScreenshotAsync(new()
            {
                Path = screenshotPath,
                FullPage = true,
            });

            logger.Info($"Captured screenshot at {screenshotPath}, ({TestContext.CurrentContext.Test.Name})");

            return screenshotPath;
        }

        public static BrowserNewContextOptions StartVideoRecordingAsync()
        {
            //logger.Info("Initializing video recording for the browser context.");

            //var context = await browser.NewContextAsync(new BrowserNewContextOptions
            //{
            //    RecordVideoDir = "videos/",
            //    RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
            //});

            var browserContextOptions = new BrowserNewContextOptions
            {
                BypassCSP = true,
                IgnoreHTTPSErrors = true,
                RecordVideoDir = "videos/",
                RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
            };

            return browserContextOptions;
        }
    }
}
