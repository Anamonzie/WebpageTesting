using Allure.Net.Commons;
using Microsoft.Playwright;

namespace EpamWeb.Attachments
{
    public class AllureAttachmentManager : IAllureAttachmentManager
    {
        private static readonly object logLock = new();

        public async Task AddScreenshotAttachment(string screenshotPath)
        {
            await Task.Run(() => AllureApi.AddAttachment("Screenshot", "image/png", screenshotPath));
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

        public void AttachLogToAllure()
        {
            lock (logLock) // Ensure only one thread can access this block at a time
            {
                var logDirectory = new DirectoryInfo("./logs");
                var latestLogFile = logDirectory.GetFiles("log-*.txt")
                                                 .OrderByDescending(f => f.LastWriteTime)
                                                 .FirstOrDefault();

                if (latestLogFile != null && latestLogFile.Exists)
                {
                    var logsPath = latestLogFile.FullName;
                    AllureApi.AddAttachment("Test Logs", "text/plain", logsPath);
                }
            }
        }
    }
}
