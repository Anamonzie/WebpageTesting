using EpamWeb.Factory;
using EpamWeb.Services;
using EpamWeb.Services.ServiceInterfaces;
using EpamWeb.Utils;
using Microsoft.Playwright;

namespace EpamWebTests.BaseTestClasses
{
    public abstract class ApiTestBase : BaseTest
    {
        protected IApiServiceFactory apiServiceFactory;
        protected IAPIRequestContext api; // playwright interface for managing the context of API requests
        protected IApiService apiService; // my abstraction
        protected IFileService fileService;

        [SetUp]
        public void ApiTestSetup()
        {
            apiServiceFactory = new ApiServiceFactory();
            fileService = new FileService();

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
