namespace Event_WeatherApp.Models
{
    public class WeatherInfo
    {
        public double TempInCelsius { get; set; }
        public string ConditionInText { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public double RainProbability { get; set; }
        public double WindSpeedKmH { get; set; }
        public double WindGustsKmH { get; set; }
        public DateTime ForecastTime { get; set; }

    }
}
