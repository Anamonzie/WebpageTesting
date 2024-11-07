using EpamWeb.Factory;

namespace EpamWebTests.PageTests
{
    public abstract class BaseTest
    {
        protected static IBrowserFactory browserFactory;


        [OneTimeSetUp]
        public static void GlobalSetup()
        {
            browserFactory = BrowserFactory.Instance;
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            //logger.CloseAndFlush();
            //AllureAttachmentManager.AttachLogToAllure();
        }
    }
}
