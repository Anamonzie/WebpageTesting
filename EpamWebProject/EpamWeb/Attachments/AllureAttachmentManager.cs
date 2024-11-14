using Allure.Net.Commons;
using Microsoft.Playwright;
using NUnit.Framework;

namespace EpamWeb.Attachments
{
    public class AllureAttachmentManager : IAllureAttachmentManager
    {
        public async Task AddScreenshotAttachment(string screenshotPath)
        {
            await Task.Run(() => AllureApi.AddAttachment("Screenshot", "image/png", screenshotPath));
            TestContext.AddTestAttachment(screenshotPath, "image/png");
        }

        public async Task AddVideoAttachment(IPage page)
        {
            if (page.Video != null)
            {
                var path = await page.Video.PathAsync();
                var videoPath = Path.Combine("videos", path);

                AllureApi.AddAttachment("Test Video", "video/webm", videoPath);
                TestContext.AddTestAttachment(videoPath, "video/webm");
            }
        }

        public void AttachLogToAllure(string logFilePath)
        {
            AllureApi.AddAttachment("Test Logs", "text/plain", logFilePath);
            TestContext.AddTestAttachment(logFilePath, "text/plain");
        }
    }
}
