using Allure.NUnit;
using Allure.NUnit.Attributes;
using EpamWeb.Attachments;
using EpamWeb.Factory;
using EpamWeb.Loggers;
using EpamWeb.Services;
using EpamWeb.Utils;
using Microsoft.Playwright;

namespace EpamWebTests.PageTests
{
    [AllureNUnit]
    public abstract class BaseTest
    {
        protected static IBrowserFactory browserFactory;
        protected static ILoggerManager logger;
        protected static readonly ThreadLocal<IBrowser> browser = new();

        protected IPageFactory pageFactory;
        protected IServiceFactory serviceFactory;
        protected IBrowserContext context;

        protected IMediaCaptureService mediaCaptureService;
        protected IAllureAttachmentManager allureAttachmentManager;

        protected IApiServiceFactory apiServiceFactory;
        protected IAPIRequestContext api; // playwright interface for managing the context of API requests
        protected IApiService apiService; // my abstraction

        [OneTimeSetUp]
        public static void GlobalSetup()
        {
            browserFactory = BrowserFactory.Instance;
            logger = LoggerManager.Instance;
        }

        [OneTimeTearDown]
        public async Task GlobalTearDown()
        {
            await api.DisposeAsync();
        }

        [SetUp]
        [AllureBefore("Setup session")]
        public async Task Setup()
        {
            // for API tests
            apiService = apiServiceFactory.Create(ConstantData.ApiUrl);

            // For old tests
            var testName = TestContext.CurrentContext.Test.Name;
            logger.InitializeLogFilePath(testName);

            mediaCaptureService = new MediaCaptureService(logger);
            allureAttachmentManager = new AllureAttachmentManager();

            browser.Value = await browserFactory.GetBrowser();
            context = await browser.Value.NewContextAsync(mediaCaptureService.StartVideoRecordingAsync());
            pageFactory = new PageFactory(context);

            var page = await pageFactory.GetOrCreatePageAsync(testName);
            serviceFactory = ServiceFactory.CreateInstance(pageFactory, page, logger);
        }

        [TearDown]
        [AllureAfter("Dispose session")]
        public async Task TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var page = await pageFactory.GetOrCreatePageAsync(testName);

            if (!page.IsClosed)
            {
                var screenshotPath = await mediaCaptureService.CaptureScreenshot(page);
                await allureAttachmentManager.AddScreenshotAttachment(screenshotPath);

                await page.CloseAsync();
                await page.Context.CloseAsync();

                await allureAttachmentManager.AddVideoAttachment(page);
            }

            await pageFactory.RemovePageAsync(testName);

            logger.CloseAndFlush(testName);

            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs", testName);
            var logFilePath = Path.Combine(logDirectory, $"{testName}-log.txt");

            if (File.Exists(logFilePath))
            {
                allureAttachmentManager.AttachLogToAllure(logFilePath);
            }
        }
    }
}
