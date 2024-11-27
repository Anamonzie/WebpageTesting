namespace EpamWeb.Services.ServiceInterfaces
{
    public interface IBaseService
    {
        Task NavigateToUrlAsync(string url);
        Task NavigateToUrlAndAcceptCookiesAsync(string url);
        Task<string> GetPageTitleAsync();
    }
}
