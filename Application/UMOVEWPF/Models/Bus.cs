using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UMOVEWPF.Models;

namespace UMOVEWPF.Models
{
    /// <summary>
    /// Modelklasse for en bus, med alle relevante egenskaber og notifikationer til UI.
    /// </summary>
    public enum RouteName
    {
        None,
        _11,
        _8A,
        _85
    }

    /// <summary>
    /// Repræsenterer en bus og dens egenskaber.
    /// Implementerer INotifyPropertyChanged for at UI kan opdatere automatisk.
    /// </summary>
    public class Bus : INotifyPropertyChanged
    {
        // Unikt ID for bussen
        private string _busId;
        public string BusId
        {
            get => _busId;
            set { _busId = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        // Modelnavn (kan evt. fjernes hvis ikke brugt)
        private string _model;
        public string Model
        {
            get => _model;
            set { _model = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        // Produktionsår
        private string _year;
        public string Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        // Batterikapacitet i kWh
        private double _batteryCapacity;
        public double BatteryCapacity
        {
            get => _batteryCapacity;
            set { _batteryCapacity = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        // Nuværende batteriniveau i procent
        private double _batteryLevel;
        public double BatteryLevel
        {
            get => _batteryLevel;
            set 
            { 
                _batteryLevel = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(DisplayText));
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(IsCritical));
            }
        }

        // Forbrug i kWh/km
        private double _consumption;
        public double Consumption
        {
            get => _consumption;
            set { _consumption = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        // Sidste opdateringstidspunkt
        private DateTime _lastUpdate;
        public DateTime LastUpdate
        {
            get => _lastUpdate;
            set { _lastUpdate = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        // Status for bussen (enum)
        private BusStatus _status;
        public BusStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Angiver om bussen er kritisk (batteri < 20%)
        /// </summary>
        public bool IsCritical => BatteryLevel < 20;

        /// <summary>
        /// Tekst til visning i lister mv.
        /// </summary>
        public string DisplayText => $"{BusId} | {Model} | {BatteryLevel:F1}%";

        // Rute som bussen kører på
        private RouteName _route;
        public RouteName Route
        {
            get => _route;
            set { _route = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Angiver om bussen er i drift (bruges til sortering/filter)
        /// </summary>
        public bool IsInService => Status == BusStatus.Inroute || Status == BusStatus.Intercept || Status == BusStatus.Return;

        /// <summary>
        /// Event for property changed (INotifyPropertyChanged)
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Kaldes når en property ændres, så UI opdateres
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 