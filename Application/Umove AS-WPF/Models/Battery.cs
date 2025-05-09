using System;

namespace Umove_AS_WPF.Models
{
    public class Battery
    {
        public string Id { get; set; }
        public double Capacity { get; set; }
        public double CurrentCharge { get; set; }
        public int ChargePercentage { get; set; }
        public double ChargerSpeed { get; set; }
        public DateTime LastCharged { get; set; }
        public DateTime NextCharging { get; set; }
        public string Status { get; set; }

        public Battery(string id, double capacity)
        {
            Id = id;
            Capacity = capacity;
            CurrentCharge = 0;
            ChargePercentage = 0;
            Status = "Disconnected";
        }
    }
} 