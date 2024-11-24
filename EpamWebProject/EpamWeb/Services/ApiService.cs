using Microsoft.Playwright;
using System.Text.Json;

namespace EpamWeb.Services
{
    public class ApiService
    {
        private readonly IAPIRequestContext _apiContext;

        public ApiService(IAPIRequestContext apiContext)
        {
            _apiContext = apiContext;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _apiContext.GetAsync(endpoint);
            EnsureSuccessStatus(response);
            var jsonResponse = await response.TextAsync();

            return JsonSerializer.Deserialize<T>(jsonResponse);
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var response = await _apiContext.PostAsync(endpoint, new APIRequestContextOptions
            {
                DataObject = data
            });

            EnsureSuccessStatus(response);
            var jsonResponse = await response.TextAsync();

            return JsonSerializer.Deserialize<T>(jsonResponse);
        }

        public async Task<IAPIResponse> GetRawAsync(string endpoint)
        {
            var response = await _apiContext.GetAsync(endpoint);
            
            EnsureSuccessStatus(response);

            return response;
        }

        public async Task<IAPIResponse> PostRawAsync(string endpoint, object data)
        {
            var response = await _apiContext.PostAsync(endpoint, new APIRequestContextOptions
            {
                DataObject = data
            });

            EnsureSuccessStatus(response);

            return response;
        }

        private void EnsureSuccessStatus(IAPIResponse response)
        {
            if (response.Status < 200 || response.Status >= 300)
            {
                throw new Exception($"HTTP request failed with status code {response.Status}");
            }
        }
    }
}
