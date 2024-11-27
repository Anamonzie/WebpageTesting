namespace EpamWeb.Services.ServiceInterfaces
{
    public interface IHomepageService : IBaseService
    {
        Task ClickHamburgerMenuAsync();
        Task<List<string>> GetHamburgerMenuListItemsAsync();

        event EventHandler<string> NavigationCompleted;
    }
}
