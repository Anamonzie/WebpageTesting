using EpamWeb.Services;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class ApiServiceFactory : IApiServiceFactory
    {
        public IApiService Create(string baseUrl)
        {
            var playwright = Playwright.CreateAsync().Result;
            var apiContext = playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL = baseUrl
            }).Result;

            return new ApiService(apiContext);
        }
    }
}
