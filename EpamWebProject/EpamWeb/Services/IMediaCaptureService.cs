using Microsoft.Playwright;

namespace EpamWeb.Services
{
    public interface IMediaCaptureService
    {
        Task<string> CaptureScreenshot(IPage page);
        BrowserNewContextOptions StartVideoRecordingAsync();
    }
}
