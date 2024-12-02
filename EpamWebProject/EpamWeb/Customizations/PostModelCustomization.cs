using AutoFixture;
using EpamWeb.Models;

namespace EpamWeb.Customizations
{
    public class PostModelCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<PostModel>(composer => composer
                .Without(p => p.Id) // Exclude Id since it's usually set by the API.
                .With(p => p.Title, $"A new post {Guid.NewGuid()}")
                .With(p => p.Body, $"bar {Guid.NewGuid()}"));
        }
    }
}
