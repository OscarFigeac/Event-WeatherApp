namespace Event_WeatherApp.Models
{
    public class ConcertPin
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
        public string VenueName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime EventDate { get; set; }
        public string ImageUrl { get; set; }
        public string TicketUrl { get; set; }

    }
}
