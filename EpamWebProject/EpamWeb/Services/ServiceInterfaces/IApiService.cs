using Microsoft.Playwright;

namespace EpamWeb.Services.ServiceInterfaces
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, object data);
        Task<IAPIResponse> GetRawAsync(string endpoint);
        Task<IAPIResponse> PostRawAsync(string endpoint, object data);
    }
}
