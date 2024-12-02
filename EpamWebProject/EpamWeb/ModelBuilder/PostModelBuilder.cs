using EpamWeb.Models;

namespace EpamWeb.ModelBuilder
{
    public class PostModelBuilder
    {
        private readonly PostModel postModel;

        public PostModelBuilder()
        {
            postModel = new PostModel()
            {
                Id = 0,
                Title = $"Default Title {Guid.NewGuid()}",
                Body = $"Default Body  {Guid.NewGuid()}",
                UserId = 1
            };
        }

        public PostModel Build()
        {
            return postModel;
        }

        public PostModelBuilder WithId(int id)
        {
            postModel.Id = id;
            return this;
        }

        public PostModelBuilder WithTitle(string title)
        {
            postModel.Title = title;
            return this;
        }

        public PostModelBuilder WithBody(string body)
        {
            postModel.Body = body;
            return this;
        }

        public PostModelBuilder WithUserId(int userId)
        {
            postModel.UserId = userId;
            return this;
        }
    }
}
