namespace EpamWeb.Services.ServiceInterfaces
{
    public interface IInsightsPageService : IBaseService
    {
        Task ClickFindButtonAsync();
        Task InputTextInSearchFieldAsync();
        Task<string> GetSearchResultTextAsync();
    }
}
