namespace EpamWeb.Services
{
    public interface IBaseService
    {
        Task NavigateToUrlAsync(string url);
        Task NavigateToUrlAndAcceptCookiesAsync(string url);
        Task<string> GetPageTitleAsync();
    }
}
