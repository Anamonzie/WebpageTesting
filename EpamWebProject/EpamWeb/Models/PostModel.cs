using System.Text.Json.Serialization;

namespace EpamWeb.Models
{
    public class PostModel
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("body")]
        public string? Body { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Body: {Body}, UserId: {UserId}";
        }

        public override bool Equals(object? obj)
        {
            return obj is PostModel post &&
                Id == post.Id &&
                Title == post.Title &&
                Body == post.Body &&
                UserId == post.UserId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Body, UserId);
        }
    }
}
