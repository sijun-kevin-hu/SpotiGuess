using System.Text.Json.Serialization;

namespace SpotiGuessAPI.Models
{
    public class Album
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("release-date")]
        public required string ReleaseDate { get; set; }

        [JsonPropertyName("images")]
        public List<ImageObject>? Images { get; set; }
    }
}