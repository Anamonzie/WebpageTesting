namespace EpamWeb.PageServices
{
    public interface IInsightsPageService : IPageService
    {
        Task ClickFindButtonAsync();
        Task InputTextInSearchFieldAsync();
        Task<string> GetSearchResultTextAsync();
    }
}
