using Allure.Net.Commons;
using Microsoft.Playwright;
using NUnit.Framework;

namespace EpamWeb.Attachments
{
    public class AllureAttachmentManager : IAllureAttachmentManager
    {
        public async Task AddScreenshotAttachment(string screenshotPath)
        {
            TestContext.AddTestAttachment(screenshotPath);
            await Task.Run(() => AllureApi.AddAttachment("Screenshot", "image/png", screenshotPath));
            //AllureLifecycle.Instance.AddAttachment("Screenshot", "image/png", screenshotPath);
        }

        public async Task AddVideoAttachment(IPage page)
        {
            if (page.Video != null)
            {
                var path = await page.Video.PathAsync();
                var videoPath = Path.Combine("videos", path);

                TestContext.AddTestAttachment(videoPath);
                AllureApi.AddAttachment("Test Video", "video/webm", videoPath);
            }
        }

        public void AttachLogToAllure(string logFilePath)
        {
            TestContext.AddTestAttachment(logFilePath);
            AllureApi.AddAttachment("Test Logs", "text/plain", logFilePath);
        }
    }
}
