using Microsoft.Playwright;

namespace EpamWeb.Attachments
{
    public interface IAllureAttachmentManager
    {
        Task AddScreenshotAttachment(string screenshotPath);
        Task AddVideoAttachment(IPage page);
        void AttachLogToAllure(string logFilePath);
    }
}
