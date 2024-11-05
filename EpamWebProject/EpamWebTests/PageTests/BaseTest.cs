using EpamWeb.Factory;
using EpamWeb.Loggers;

namespace EpamWebTests.PageTests
{
    public abstract class BaseTest
    {
        protected static IBrowserFactory browserFactory;
        protected static ILoggerManager logger;

        [OneTimeSetUp]
        public static void GlobalSetup()
        {
            browserFactory = BrowserFactory.Instance;
            logger = LoggerManager.Instance;
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            //logger.CloseAndFlush();
            //AllureAttachmentManager.AttachLogToAllure();
        }
    }
}
