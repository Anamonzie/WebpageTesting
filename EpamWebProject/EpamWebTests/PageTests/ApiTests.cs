using System.Text.Json;

namespace EpamWebTests.PageTests
{
    [TestFixture]
    public class ApiTests : BaseTest
    {
        [Test]
        public async Task GetPosts_ShouldReturnListOfPosts()
        {
            //var response = await _api.GetAsync("/posts");
            //Assert.That(response.Status, Is.EqualTo(200));

            //var posts = JsonSerializer.Deserialize<List<JsonElement>>(await response.TextAsync());
            //Assert.That(posts.Count > 0, "Expected at least one post.");

            var posts = await _apiService.GetAsync<List<JsonElement>>("/posts");
            Assert.That(posts.Count, Is.GreaterThan(0), "Expected at least one post.");

        }

        [Test]
        public async Task GetPostById_ShouldReturnCorrectPost()
        {
            //    int postId = 1;
            //    var response = await _api.GetAsync($"/posts/{postId}");
            //    Assert.That(response.Status, Is.EqualTo(200));

            //    var post = JsonSerializer.Deserialize<JsonElement>(await response.TextAsync());

            //    Assert.That(post.GetProperty("id").GetInt32(), Is.EqualTo(postId), "Post ID does not match.");

            int postId = 1;
            var post = await _apiService.GetAsync<JsonElement>($"/posts/{postId}");
            Assert.That(post.GetProperty("id").GetInt32(), Is.EqualTo(postId), "Post ID does not match.");

        }

        [Test]
        public async Task CreatePost_ShouldReturnNewPost()
        {
            var newPost = new
            {
                title = "foo",
                body = "bar",
                userId = 1
            };

            //    var response = await _api.PostAsync("/posts", new APIRequestContextOptions
            //    {
            //        DataObject = newPost
            //    });

            //    Assert.That(response.Status, Is.EqualTo(201)); // Created

            //    var createdPost = JsonSerializer.Deserialize<JsonElement>(await response.TextAsync());
            //    Assert.That(createdPost.GetProperty("title").GetString(), Is.EqualTo(newPost.title), "Title mismatch.");
            //    Assert.That(createdPost.GetProperty("body").GetString(), Is.EqualTo(newPost.body), "Body mismatch.");
            //    Assert.That(createdPost.GetProperty("userId").GetInt32(), Is.EqualTo(newPost.userId), "User ID mismatch.");
            //}

            var createdPost = await _apiService.PostAsync<JsonElement>("/posts", newPost);
            Assert.That(createdPost.GetProperty("title").GetString(), Is.EqualTo(newPost.title), "Title mismatch.");
            Assert.That(createdPost.GetProperty("body").GetString(), Is.EqualTo(newPost.body), "Body mismatch.");
            Assert.That(createdPost.GetProperty("userId").GetInt32(), Is.EqualTo(newPost.userId), "User ID mismatch.");
        }
    }
}
