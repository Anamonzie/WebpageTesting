using Microsoft.Playwright;

namespace EpamWeb.Services.ServiceInterfaces
{
    public interface IMediaCaptureService
    {
        Task<string> CaptureScreenshot(IPage page);
        BrowserNewContextOptions StartVideoRecordingAsync();
    }
}
