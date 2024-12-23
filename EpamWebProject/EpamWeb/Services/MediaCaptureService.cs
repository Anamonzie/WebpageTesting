﻿using EpamWeb.Loggers;
using EpamWeb.Services.ServiceInterfaces;
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
            var baseDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots");
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                baseDirectory = Path.Combine("allure-results", "Screenshots");
            }
            
            var screenshotsDirectory = Path.Combine(baseDirectory, TestContext.CurrentContext.Test.Name);
            Directory.CreateDirectory(screenshotsDirectory);

            var screenshotPath = Path.Combine(screenshotsDirectory, $"screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.UtcNow:MMdd_HHmm}.png");
            await page.ScreenshotAsync(new()
            {
                Path = screenshotPath,
                FullPage = true,
            });

            logger.Info(TestContext.CurrentContext.Test.Name, $"Captured screenshot at {screenshotPath}");

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
