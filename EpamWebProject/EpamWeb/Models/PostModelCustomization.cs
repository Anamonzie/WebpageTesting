using AutoFixture;
using EpamWeb.Models;

namespace EpamWeb.Factories
{
    public class PostModelCustomization : ICustomization
    {
        //private static readonly Fixture Fixture = new();

        public void Customize(IFixture fixture)
        {
            fixture.Customize<PostModel>(composer => composer
                .Without(p => p.Id) // Exclude Id since it's usually set by the API.
                .With(p => p.Title, $"A new post {Guid.NewGuid()}")
                .With(p => p.Body, $"bar {Guid.NewGuid()}"));
        }

        /// Creates a random PostModel with default values.
        //public static PostModel CreateRandomPost()
        //{
        //    return Fixture.Build<PostModel>()
        //        .Without(p => p.Id) // Exclude Id since it's usually set by the API.
        //        .With(p => p.Title, $"A new post {Guid.NewGuid()}")
        //        .With(p => p.Body, $"bar {Guid.NewGuid()}")
        //        .Create();
        //}
    }
}
