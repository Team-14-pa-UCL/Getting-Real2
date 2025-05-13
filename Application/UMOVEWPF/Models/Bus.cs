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
        R1A,
        R10,
        R11,
        R13,
        R132,
        R133,
        R137,
        R139
    }

    /// <summary>
    /// Repræsenterer en bus og dens egenskaber.
    /// Implementerer INotifyPropertyChanged for at UI kan opdatere automatisk.
    /// </summary>
    public class Bus : INotifyPropertyChanged
    {
        // Unikt ID for bussen (med get & set)
        private string _busId;
        public string BusId
        {
            get => _busId;
            set { _busId = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); } //OnPropertyChanged kalders når værden ændres og opdater displayet samtidig.
        }

        // Modelnavn (kan evt. fjernes hvis ikke brugt)
        private string _model;
        public string Model
        {
            get => _model;
            set { _model = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); } //OnPropertyChanged kalders når værden ændres og opdater displayet samtidig.
        }

        // Produktionsår (med get & set)
        private string _year;
        public string Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); } //OnPropertyChanged kalders når værden ændres og opdater displayet samtidig.
        }

        // Batterikapacitet i kWh (med get & set)
        private double _batteryCapacity;
        public double BatteryCapacity
        {
            get => _batteryCapacity;
            set { _batteryCapacity = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); } //OnPropertyChanged kalders når værden ændres og opdater displayet samtidig.
        }

        // Nuværende batteriniveau i procent (med get & set)
        private double _batteryLevel;
        public double BatteryLevel
        {
            get => _batteryLevel;
            set 
            { 
                _batteryLevel = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(DisplayText)); // UI Viser korrekt Tekst
                OnPropertyChanged(nameof(Status)); // Så bussens batterylevel opdates til en ny status
                OnPropertyChanged(nameof(IsCritical)); // Vis hvis kritisk.
            }
        }

        // Forbrug i kWh/km (med get & set)
        private double _consumption;
        public double Consumption
        {
            get => _consumption;
            set { _consumption = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); } //OnPropertyChanged kalders når værden ændres og opdater displayet samtidig.
        }

        // Sidste opdateringstidspunkt (med get & set)
        private DateTime _lastUpdate;
        public DateTime LastUpdate
        {
            get => _lastUpdate;
            set { _lastUpdate = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); } //OnPropertyChanged kalders når værden ændres og opdater displayet samtidig.
        }

        // Status for bussen (enum) (med get & set)
        private BusStatus _status;
        public BusStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); } //OnProperChanged kaldes når værden ændres og opdateres i systemt. (Enum opdater resten / dropdown)
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
        /// Angiver om bussen er i drift (bruges til til simuleringen)
        /// </summary>
        public bool IsInService => Status == BusStatus.Inroute || Status == BusStatus.Intercept || Status == BusStatus.Returning;

        /// <summary>
        /// Event for property changed (INotifyPropertyChanged)
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Kaldes når en property ændres, så UI opdateres. Har sammenhæng til den atrribute man prøver at opdatere i xaml kode)
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 