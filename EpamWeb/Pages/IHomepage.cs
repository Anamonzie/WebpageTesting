using Microsoft.Playwright;

namespace EpamWeb.Pages
{
    public interface IHomepage
    {
        ILocator HamburgerButton { get; }
        ILocator HamburgerButtonNavigation { get; }
        ILocator HamburgerMenuItems { get; }
    }
}
