using System;

namespace Umove_AS_WPF.Models
{
    public class Block
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string BusId { get; set; }
        public string DriverId { get; set; }
        public string RouteId { get; set; }

        public Block(string id, string name, DateTime startTime, DateTime endTime)
        {
            Id = id;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
} 