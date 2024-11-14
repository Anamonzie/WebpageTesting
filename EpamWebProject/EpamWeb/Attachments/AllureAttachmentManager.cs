using Allure.Net.Commons;
using Microsoft.Playwright;

namespace EpamWeb.Attachments
{
    public class AllureAttachmentManager : IAllureAttachmentManager
    {
        //private static readonly object logLock = new();

        public async Task AddScreenshotAttachment(string screenshotPath)
        {
            await Task.Run(() => AllureApi.AddAttachment("Screenshot", "image/png", screenshotPath));
            TestContext.AddTestAttachment(screenshotPath);
        }

        public async Task AddVideoAttachment(IPage page)
        {
            if (page.Video != null)
            {
                var path = await page.Video.PathAsync();
                var videoPath = Path.Combine("videos", path);

                AllureApi.AddAttachment("Test Video", "video/webm", videoPath);
            }
        }

        public void AttachLogToAllure(string logFilePath)
        {
            AllureApi.AddAttachment("Test Logs", "text/plain", logFilePath);

            //lock (logLock) // Ensure only one thread can access this block at a time
            //{
            //    if (logFilePath != null && logFilePath.Exists)
            //    {
            //        var logsPath = logFilePath.FullName;
            //        AllureApi.AddAttachment("Test Logs", "text/plain", logsPath);
            //    }
            //}
        }
    }
}
