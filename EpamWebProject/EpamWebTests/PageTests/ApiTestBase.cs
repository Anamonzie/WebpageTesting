using EpamWeb.Factory;
using EpamWeb.Services;
using EpamWeb.Utils;
using Microsoft.Playwright;

namespace EpamWebTests.PageTests
{
    public abstract class ApiTestBase : BaseTest
    {
        protected IApiServiceFactory apiServiceFactory;
        protected IAPIRequestContext api; // playwright interface for managing the context of API requests
        protected IApiService apiService; // my abstraction

        [SetUp]
        public void ApiTestSetup()
        {
            apiService = apiServiceFactory.Create(ConstantData.ApiUrl);
        }

        [TearDown]
        public async Task ApiTestTearDown()
        {
            if (api != null)
            {
                await api.DisposeAsync();
                api = null;
            }
        }
    }
}
