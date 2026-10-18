using System.Text.Json.Serialization;

namespace Event_WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "c997ef222c37bfb96e98c72d11ae15fd";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //internal DTO mapping structures for weather data

        internal class OpenWeatherAPIResponse
        {
            [JsonPropertyName("main")] public OwMain Main { get; set; } = new();
            [JsonPropertyName("weather")] public List<OwWeather> Weather { get; set; } = new();
            [JsonPropertyName("wind")] public OwWind Wind { get; set; } = new();
        }

        internal class OwMain
        {
            [JsonPropertyName("temp")] public double Temp { get; set; } //should be in celsius
        }

        internal class OwWeather
        {
            [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
            [JsonPropertyName("icon")] public string Icon { get; set; } = string.Empty;
        }

        public class OwWind
        {
            [JsonPropertyName("speed")] public double Speed { get; set; }
            [JsonPropertyName("gust")] public double Gust { get; set; }
        }
    }
}
