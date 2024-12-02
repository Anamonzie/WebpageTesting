using AutoFixture;
using EpamWeb.Customizations;
using EpamWeb.Factory.FactoryInterfaces;

namespace EpamWeb.Factory
{
    public class AutoFixtureFactory : IAutoFixtureFactory
    {
        private AutoFixtureFactory() { }

        public static Fixture Instance => InitializeInstance();

        private static Fixture InitializeInstance()
        {
            var fixture = new Fixture();

            fixture.Customize(new PostModelCustomization());

            return fixture;
        }
    }
}
