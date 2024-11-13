using Microsoft.Playwright;

namespace EpamWeb.Pages
{
    public interface IHomepage
    {
        ILocator HamburgerButton { get; }
        ILocator HamburgerMenuItems { get; }
        ILocator CookiesAcceptButton {  get; }
    }
}
