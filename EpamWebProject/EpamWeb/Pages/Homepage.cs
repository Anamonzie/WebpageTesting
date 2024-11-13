using Microsoft.Playwright;

namespace EpamWeb.Pages
{
    public class Homepage : BasePage, IHomepage
    {
        public Homepage(IPage page) : base(page) { }

        public ILocator HamburgerButton => 
            page.Locator("button.hamburger-menu__button[aria-expanded='false']");

        public ILocator HamburgerMenuItems => 
            page
            .Locator("li.hamburger-menu__item")
            .GetByRole(AriaRole.Link); //first-level-link (in class name, will also include Industries)

        public ILocator CookiesAcceptButton => page.Locator("#onetrust-accept-btn-handler");

    }
}
