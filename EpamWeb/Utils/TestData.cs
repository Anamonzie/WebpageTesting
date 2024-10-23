using System.Collections.Immutable;

namespace EpamWeb.Utils
{
    public static class TestData
    {
        // Homepage Data //

        public const string ExpectedHomepageTitle = "EPAM | Software Engineering & Product Development Services";

        public static readonly ImmutableList<string> ExpectedHamburgerMenuItems =
            ["Services", "About", "Insights", "Careers"];

        // Insights Page Data //

        public const string ExpectedInsightsPageTitle = "Discover our Latest Insights | EPAM";
        public const string ExpectedSearchPageTitle = "Search Our Website | EPAM";
        public const string SearchInput = "Cloud";
    }
}
