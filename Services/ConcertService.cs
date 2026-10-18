using Event_WeatherApp.Models;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Event_WeatherApp.Services
{
    public class ConcertService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "https://app.ticketmaster.com/discovery/v2/";

        public ConcertService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }




        //internal DTO mapping structures for concert data

        internal class TicketmasterAPIResponse
        {
            public EmbeddedEvents? Embedded { get; set; }
        }

        internal class EmbeddedEvents
        {
            public List<TmEvent>? Events { get; set; }
        }

        //matching all information retrieved from the API on JSON format
        internal class TmEvent
        {
            [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
            [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
            [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;
            [JsonPropertyName("dates")] public TmDates Dates { get; set; } = new();
            [JsonPropertyName("images")] public List<TmImage> Images { get; set; } = new();
            [JsonPropertyName("_embedded")] public TmEventEmbedded? Embedded { get; set; }
        }

        internal class TmDates
        {
            [JsonPropertyName("start")] public TmStart Start { get; set; } = new();
        }

        internal class TmStart
        {
            [JsonPropertyName("dateTime")] public DateTime DateTime { get; set; }
        }

        internal class TmEventEmbedded
        {
            [JsonPropertyName("venues")] public List<TmVenue>? Venues { get; set; }
        }

        internal class TmImage
        {
            [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;
        }

        internal class TmVenue
        {
            [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
            [JsonPropertyName("location")] public TmLocation? Location { get; set; }
        }

        internal class TmLocation
        {
            [JsonPropertyName("latitude")] public string Latitude { get; set; } = string.Empty;
            [JsonPropertyName("longitude")] public string Longitude { get; set; } = string.Empty;
        }

    }
}
