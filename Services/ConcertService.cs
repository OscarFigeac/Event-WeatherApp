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

        public async Task<List<ConcertPin>> GetDublinConcertsAsync()
        {
            //filtered URL for Dublin
            string url = $"https://app.ticketmaster.com/discovery/v2/events.json?city=Dublin&countryCode=IE&classificationName=Music&apikey={ApiKey}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<TicketmasterAPIResponse>(url);
                var pins = new List<ConcertPin>();

                if (response?.Embedded?.Events == null) return pins;

                foreach (var ev in response.Embedded.Events)
                {
                    var venue = ev.Embedded?.Venues?.FirstOrDefault();
                    if (venue?.Location.Latitude != null && venue?.Location?.Longitude != null)
                    {
                        pins.Add(new ConcertPin
                        {
                            Id = ev.Id,
                            Title = ev.Name,
                            ArtistName = ev.Name, 
                            VenueName = venue.Name,
                            Latitude = double.Parse(venue.Location.Latitude),
                            Longitude = double.Parse(venue.Location.Longitude),
                            EventDate = ev.Dates.Start.DateTime,
                            ImageUrl = ev.Images.FirstOrDefault()?.Url ?? string.Empty,
                            TicketUrl = ev.Url
                        });
                    }
                }

                return pins;
            } catch (Exception e)
            {
                //add proper logs?
                Console.WriteLine($"Could not fetchconcerts: {e.Message}");
                return new List<ConcertPin>();
            }
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
