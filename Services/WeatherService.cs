using Event_WeatherApp.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace Event_WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeatherMap:ApiKey"] ?? string.Empty;
        }

        public async Task<WeatherInfo?> GetWeatherForCoordinatesAsync(double lat, double lon)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={_apiKey}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<OpenWeatherAPIResponse>(url);
                if (response == null) return null;

                return new WeatherInfo
                {
                    TempInCelsius = response.Main.Temp,
                    ConditionInText = response.Weather.FirstOrDefault()?.Description ?? "Unknown",
                    IconUrl = $"https://openweathermap.org/img/wn/{response.Weather.FirstOrDefault()?.Icon}@2x.png",
                    RainProbability = 0,
                    WindSpeedKmH = response.Wind.Speed * 3.6, //from m/s to km/h
                    WindGustsKmH = response.Wind.Gust * 3.6,
                    ForecastTime = DateTime.UtcNow
                };
            } catch (Exception e)
            {
                Console.WriteLine($"Could not fetch weather: {e.Message}");
                return null;
            }
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

        internal class OwWind //change to internal  
        {
            [JsonPropertyName("speed")] public double Speed { get; set; }
            [JsonPropertyName("gust")] public double Gust { get; set; }
        }
    }
}
