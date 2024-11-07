using EpamWeb.Loggers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace EpamWeb.Services
{
    public class MediaCaptureService : IMediaCaptureService
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

        public BrowserNewContextOptions StartVideoRecordingAsync()
        {
            var testName = TestContext.CurrentContext.Test.Name;

            var browserContextOptions = new BrowserNewContextOptions
            {
                BypassCSP = true,
                IgnoreHTTPSErrors = true,
                RecordVideoDir = $"videos/{testName}",
                RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
            };

            return browserContextOptions;
        }
    }
}
