namespace EpamWeb.PageServices
{
    public interface IPageService
    {
        Task NavigateToUrlAsync(string url);
        Task<string> GetPageTitleAsync();
    }
}
