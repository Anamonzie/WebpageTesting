namespace EpamWeb.Services
{
    public interface IHomepageService : IPageService
    {
        Task ClickHamburgerMenuAsync();
        Task<List<string>> GetHamburgerMenuListItemsAsync();
        Task ClickAcceptAllCookies();
    }
}
