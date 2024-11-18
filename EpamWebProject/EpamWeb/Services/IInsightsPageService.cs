namespace EpamWeb.Services
{
    public interface IInsightsPageService : IBaseService
    {
        Task ClickFindButtonAsync();
        Task InputTextInSearchFieldAsync();
        Task<string> GetSearchResultTextAsync();
    }
}
