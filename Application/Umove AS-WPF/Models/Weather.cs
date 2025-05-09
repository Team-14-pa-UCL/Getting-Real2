using System;

namespace Umove_AS_WPF.Models
{
    public class Weather
    {
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public string Condition { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }

        public Weather(string location)
        {
            Location = location;
            Timestamp = DateTime.Now;
        }
    }
} 