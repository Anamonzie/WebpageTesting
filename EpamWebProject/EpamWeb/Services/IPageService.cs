namespace EpamWeb.Services
{
    public interface IPageService
    {
        Task NavigateToUrlAsync(string url);
        Task NavigateToUrlAndAcceptCookiesAsync(string url);
        Task<string> GetPageTitleAsync();
    }
}
