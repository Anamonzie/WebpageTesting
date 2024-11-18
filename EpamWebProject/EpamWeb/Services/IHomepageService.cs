namespace EpamWeb.Services
{
    public interface IHomepageService : IBaseService
    {
        Task ClickHamburgerMenuAsync();
        Task<List<string>> GetHamburgerMenuListItemsAsync();
    }
}
