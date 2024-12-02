using AutoFixture;
using EpamWeb.Factory;
using EpamWeb.Factory.FactoryInterfaces;
using EpamWeb.Services;
using EpamWeb.Services.ServiceInterfaces;
using EpamWeb.Utils;
using Microsoft.Playwright;

namespace EpamWebTests.BaseTestClasses
{
    public abstract class ApiTestBase 
    {
        protected IApiServiceFactory apiServiceFactory;
        protected IAPIRequestContext api;
        protected IApiService apiService; 
        protected IFileService fileService;

        protected Fixture FixtureInstance { get; } = AutoFixtureFactory.Instance;

        [SetUp]
        public void ApiTestSetup()
        {
            apiServiceFactory = ApiServiceFactory.Instance;
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
