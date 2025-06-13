using System.Text.Json.Serialization;

namespace SpotiGuessAPI.Models
{
    public class ImageObject
    {
        [JsonPropertyName("url")]
        public required string Url { get; set; }

        [JsonPropertyName("height")]
        public required int Height { get; set; }

        [JsonPropertyName("width")]
        public required int Width { get; set; }
    }
}