using Microsoft.Playwright;

namespace EpamWeb.Pages
{
    public class Homepage : BasePage, IHomepage
    {
        public Homepage(IPage page) : base(page) { }

        public ILocator HamburgerButton => 
            page.Locator("button.hamburger-menu__button[aria-expanded='false']");

        public ILocator HamburgerButtonNavigation =>
            page.GetByRole(AriaRole.Navigation, new() { Name = "Main navigation" }).First;

        public ILocator HamburgerMenuItems => 
            page
            .Locator("li.hamburger-menu__item")
            .GetByRole(AriaRole.Link);
    }
}
