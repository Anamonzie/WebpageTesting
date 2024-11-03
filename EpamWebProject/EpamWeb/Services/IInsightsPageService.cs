namespace EpamWeb.Services
{
    public interface IInsightsPageService : IPageService
    {
        Task ClickFindButtonAsync();
        Task InputTextInSearchFieldAsync();
        Task<string> GetSearchResultTextAsync();
        Task ClickAcceptAllCookies();
    }
}
