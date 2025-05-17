using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace UMOVEWPF.Models
{
    public class Weather : INotifyPropertyChanged
    {
        
        public enum Month
        {
            Januar,
            Februar,
            Marts,
            April,
            Maj,
            Juni,
            Juli,
            August,
            September,
            Oktober,
            November,
            December
        }

        private Month _selectedMonth = Month.Januar;
        public Month SelectedMonth
        {
            get => _selectedMonth;
            set { _selectedMonth = value; OnPropertyChanged(nameof(SelectedMonth)); OnPropertyChanged(nameof(ConsumptionMultiplier)); }
        }

        private bool _isRaining; //Tilføj evt "wiperOn" betingelse i bus, skal påvirke baserate, fx +0,1. Udkommentér hvis undlades.

        public bool IsRaining
        {
            get => _isRaining;
            set { _isRaining = value; OnPropertyChanged(nameof(IsRaining)); OnPropertyChanged(nameof(ConsumptionMultiplier)); }
        }

        public double ConsumptionMultiplier => GetConsumptionMultiplier();

        public double GetConsumptionMultiplier()
        {
            double baseMultiplier = 1.0;
            switch (SelectedMonth)
            {
                case Month.December:
                case Month.Januar:
                case Month.Februar:
                    baseMultiplier = 1.4;
                    break;
                case Month.Marts:
                case Month.November:
                    baseMultiplier = 1.2;
                    break;
                case Month.April:
                case Month.Oktober:
                    baseMultiplier = 1.0;
                    break;
                case Month.Maj:
                case Month.September:
                    baseMultiplier = 0.9;
                    break;
                case Month.Juni:
                case Month.Juli:
                case Month.August:
                    baseMultiplier = 0.8;
                    break;
            }
            if (IsRaining)
                baseMultiplier += 0.1;
            return baseMultiplier;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path, false))
            {
                sw.WriteLine($"{SelectedMonth};{IsRaining}");
            }
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) return;
            using (var sr = new StreamReader(path))
            {
                var line = sr.ReadLine();
                if (line != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 2)
                    {
                        if (Enum.TryParse(parts[0], out Month month))
                            SelectedMonth = month;
                        if (bool.TryParse(parts[1], out bool rain))
                            IsRaining = rain;
                    }
                }
            }
        }
    }
}
