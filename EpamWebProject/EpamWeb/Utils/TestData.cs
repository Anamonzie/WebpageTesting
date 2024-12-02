using System.Collections.Immutable;

namespace EpamWeb.Utils
{
    public static class ConstantData
    {
        public const string EpamHomepageUrl = "https://www.epam.com";
        public const string EpamInsightsPageUrl = "https://www.epam.com/insights";
        public const string GoogleUrl = "https://www.google.com";
        public const string ApiUrl = "https://jsonplaceholder.typicode.com";
    }

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

        public const string ExpectedGoogleTitle = "Google";

        public const string ApiFilePost1 = "ExpectedData/Post1.json";
        public const string PathToApiGeneratedPost = "GeneratedData/NewPost.json";
    }
}
