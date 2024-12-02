using EpamWeb.Factory.FactoryInterfaces;
using EpamWeb.Services;
using EpamWeb.Services.ServiceInterfaces;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public class ApiServiceFactory : IApiServiceFactory
    {
        private static readonly Lazy<ApiServiceFactory> instance = new Lazy<ApiServiceFactory>(() => new ApiServiceFactory());

        private ApiServiceFactory() { }

        public static ApiServiceFactory Instance => instance.Value;

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
