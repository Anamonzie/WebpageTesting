using Allure.NUnit;
using EpamWeb.Attachments;
using EpamWeb.Factory;
using EpamWeb.Loggers;
using EpamWeb.Services;
using Microsoft.Playwright;
using System.Collections.Concurrent;

namespace EpamWebTests.PageTests
{
    [AllureNUnit]
    public abstract class BaseTest
    {
        protected static IBrowserFactory browserFactory;
        protected static ILoggerManager logger;
        protected static readonly ThreadLocal<IBrowser> browser = new();
        protected static readonly ConcurrentDictionary<string, IPage> Pages = new();

        protected IPageFactory pageFactory;
        protected IServiceFactory serviceFactory;
        protected IBrowserContext context;

        protected IMediaCaptureService mediaCaptureService;
        protected IAllureAttachmentManager allureAttachmentManager;

        [OneTimeSetUp]
        public static void GlobalSetup()
        {
            browserFactory = BrowserFactory.Instance;
            logger = LoggerManager.Instance;
        }

        [SetUp]
        public async Task Setup()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            logger.InitializeLogFilePath(testName);

            mediaCaptureService = new MediaCaptureService(logger);
            allureAttachmentManager = new AllureAttachmentManager();

            browser.Value = await browserFactory.GetBrowser();
            context = await browser.Value.NewContextAsync(mediaCaptureService.StartVideoRecordingAsync());

            var page = await context.NewPageAsync();
            Pages[testName] = page;

            pageFactory = new PageFactory(page);
            serviceFactory = ServiceFactory.CreateInstance(pageFactory, page, logger);
        }

        [TearDown]
        public async Task TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;

            if (Pages.TryRemove(testName, out var testPage) && !testPage.IsClosed)
            {
                var screenshotPath = await mediaCaptureService.CaptureScreenshot(testPage);
                await allureAttachmentManager.AddScreenshotAttachment(screenshotPath);

                await testPage.CloseAsync();
                await testPage.Context.CloseAsync();

                await allureAttachmentManager.AddVideoAttachment(testPage);
            }

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
