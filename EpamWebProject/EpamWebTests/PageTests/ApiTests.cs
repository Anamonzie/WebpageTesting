using Allure.NUnit.Attributes;
using AutoFixture;
using EpamWeb.Factories;
using EpamWeb.Models;
using EpamWeb.Utils;
using EpamWebTests.BaseTestClasses;
using FluentAssertions;

namespace EpamWebTests.PageTests
{
    [TestFixture]
    [Category("ApiTest")]
    [AllureFeature("API Tests")]
    public class ApiTests : ApiTestBase
    {
        [Test]
        [AllureName("GET posts")]
        public async Task GetPosts_ShouldReturnListOfPosts()
        {
            var endpoint = ApiEndpoints.Posts;

            var posts = await apiService.GetAsync<List<PostModel>>(endpoint);

            posts.Should().NotBeEmpty("Expected at least one post.");
        }

        [Test]
        [AllureName("GET post by ID")]
        public async Task GetPostById_ShouldReturnCorrectPost()
        {
            int postId = 1;
            var expectedPost = fileService.ReadFromFile<PostModel>(TestData.ApiFilePost1);
            var endpoint = string.Format(ApiEndpoints.PostById, postId);

            var post = await apiService.GetAsync<PostModel>(endpoint);

            post.Should().BeEquivalentTo(expectedPost, "Expected post should match.");
        }

        [Test]
        [AllureName("POST a post")]
        public async Task CreatePost_ShouldReturnNewPost()
        {
            // Arrange
            var newPost = PostModelFactory.CreateRandomPost();

            fileService.WriteToFile(TestData.PathToApiGeneratedPost, newPost);
            var endpoint = ApiEndpoints.Posts;

            // Act
            var createdPost = await apiService.PostAsync<PostModel>(endpoint, newPost);

            // Assert
            createdPost.Should().BeEquivalentTo(newPost, options => options.Excluding(p => p.Id));
        }
    }
}
