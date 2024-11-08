using EpamWeb.Factory;
using Microsoft.Playwright;

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


    }
}
