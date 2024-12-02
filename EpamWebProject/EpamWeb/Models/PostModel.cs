using System.Text.Json.Serialization;

namespace EpamWeb.Models
{
    public class PostModel
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Title")]
        public string? Title { get; set; }

        [JsonPropertyName("Body")]
        public string? Body { get; set; }

        [JsonPropertyName("UserId")]
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Body: {Body}, UserId: {UserId}";
        }

        public bool Equals(PostModel obj)
        {
            return Id == obj.Id &&
                Title == obj.Title &&
                Body == obj.Body &&
                UserId == obj.UserId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Body, UserId);
        }
    }
}
