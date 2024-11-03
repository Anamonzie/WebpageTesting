using Microsoft.Playwright;

namespace EpamWeb.Pages
{
    public abstract class BasePage
    {
        public IPage page;

        protected BasePage(IPage page)
        {
            this.page = page;
        }
    }
}
