using System.Text.Json.Serialization;

namespace DependenciesMockingDemoProject.Client.Models
{
    public class Main
    {
        public float Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public float FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public float TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public float TempMax { get; set; }

        public float Pressure { get; set; }

        public float Humidity { get; set; }
    }
}