using Allure.NUnit.Attributes;
using AutoFixture;
using EpamWeb.Factory;
using EpamWeb.Models;
using EpamWeb.Utils;
using FluentAssertions;
using System.Text.Json;

namespace EpamWebTests.PageTests
{
    [TestFixture]
    public class ApiTests : BaseTest
    {
        public ApiTests()
        {
            apiServiceFactory = new ApiServiceFactory(); // Initialize once
        }

        [Test]
        [AllureName("GET posts")]
        public async Task GetPosts_ShouldReturnListOfPosts()
        {
            var posts = await apiService.GetAsync<List<PostModel>>(ApiEndpoints.Posts);

            posts.Should().NotBeEmpty("Expected at least one post.");
        }

        [Test]
        [AllureName("GET post by ID")]
        public async Task GetPostById_ShouldReturnCorrectPost()
        {
            int postId = 1;

            var expectedPost = ReadFromFile<PostModel>("ExpectedData/Post1.json");
            var post = await apiService.GetAsync<PostModel>(string.Format(ApiEndpoints.PostById, postId));

            post.Should().BeEquivalentTo(expectedPost, "Expected post should match.");
        }

        [Test]
        [AllureName("POST a post")]
        public async Task CreatePost_ShouldReturnNewPost()
        {
            var fixture = new Fixture();
            fixture.Customize<PostModel>(c => c
                .Without(p => p.Id)
                .With(p => p.Title, $"A new post {Guid.NewGuid()}")
                .With(p => p.Body, $"bar {Guid.NewGuid()}"));

            var newPost = fixture.Create<PostModel>();

            WriteToFile("GeneratedData/NewPost.json", newPost);

            var createdPost = await apiService.PostAsync<PostModel>(ApiEndpoints.Posts, newPost);

            createdPost.Should().BeEquivalentTo(newPost, options => options.Excluding(p => p.Id));
        }

        private void WriteToFile<T>(string filePath, T data)
        {
            var directory = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Write JSON data to the file
            var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }

        private T ReadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var jsonData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonData)
                ?? throw new InvalidOperationException($"Failed to deserialize data from {filePath}"); ;
        }
    }
}
