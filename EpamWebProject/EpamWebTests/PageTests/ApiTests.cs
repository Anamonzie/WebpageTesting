using Allure.NUnit.Attributes;
using EpamWeb.ModelBuilder;
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
            // Arrange
            var endpoint = ApiEndpoints.Posts;

            // Act
            var posts = await apiService.GetAsync<List<PostModel>>(endpoint);

            // Assert
            posts.Should().NotBeEmpty("Expected at least one post.");
        }

        [Test]
        [AllureName("GET post by ID")]
        public async Task GetPostById_ShouldReturnCorrectPost()
        {
            // Arrange
            int postId = 1;
            var expectedPost = fileService.ReadFromFile<PostModel>(TestData.ApiFilePost1);
            var endpoint = string.Format(ApiEndpoints.PostById, postId);

            // Act
            var post = await apiService.GetAsync<PostModel>(endpoint);

            // Assert
            post.Should().BeEquivalentTo(expectedPost, "Expected post should match.");
        }

        [Test]
        [AllureName("POST a post")]
        public async Task CreatePost_ShouldReturnNewPost()
        {
            // Arrange

            ///var newPost = PostModelCustomization.CreateRandomPost();
            ///var newPost = FixtureInstance.Create<PostModel>();
            ///fileService.WriteToFile(TestData.PathToApiGeneratedPost, newPost);

            var expectedPost = new PostModelBuilder()
                .WithTitle("Custom Title")
                .WithBody("Custom Body")
                .WithUserId(123)
                .Build();

            var endpoint = ApiEndpoints.Posts;

            // Act

            var createdPost = await apiService.PostAsync<PostModel>(endpoint, expectedPost);

            ///  *Creating endpoint for newly created Post and GETting it*
            ////var getEndpoint = $"{endpoint}/{createdPost.Id}";
            ////var retrievedPost = await apiService.GetAsync<PostModel>(getEndpoint);

            // Assert

            Assert.Multiple(() =>
            {
                createdPost.Should()
                    .BeEquivalentTo(expectedPost, options => options.Excluding(p => p.Id),
                        "the created post should match the expected data except for the Id");

                //retrievedPost.Should()
                //    .BeEquivalentTo(createdPost, "the retrieved post should match the post returned by the POST request");
            });
        }
    }
}
