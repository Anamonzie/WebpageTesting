using AutoFixture;
using EpamWeb.Models;

namespace EpamWeb.Factories
{
    public static class PostModelFactory
    {
        private static readonly Fixture Fixture = new();

        /// <summary>
        /// Creates a random PostModel with default values.
        /// </summary>
        public static PostModel CreateRandomPost()
        {
            return Fixture.Build<PostModel>()
                .Without(p => p.Id) // Exclude Id since it's usually set by the API.
                .With(p => p.Title, $"A new post {Guid.NewGuid()}")
                .With(p => p.Body, $"bar {Guid.NewGuid()}")
                .Create();
        }

        /// <summary>
        /// Creates a generic PostModel using AutoFixture defaults.
        /// </summary>
        public static PostModel CreateDefaultPost()
        {
            return Fixture.Create<PostModel>();
        }
    }
}
