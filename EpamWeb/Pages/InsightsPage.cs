using Microsoft.Playwright;

namespace EpamWeb.Pages
{
    public class InsightsPage : BasePage, IInsightsPage
    {
        public InsightsPage(IPage page) : base(page) { }

        public ILocator FindButton => page
            .GetByRole(AriaRole.Button, (new() { Name = "Find"}));

        public ILocator SearchField => page.GetByPlaceholder("Cybersecurity");

        public ILocator SearchResult => page.GetByRole(AriaRole.Heading, new() { Name = "results for" });
    }
}
