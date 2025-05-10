using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UMOVEWPF;

namespace UMOVEWPF
{
    public enum RouteName
    {
        None,
        _11,
        _8A,
        _85
    }

    public class Bus : INotifyPropertyChanged
    {
        private string _busId;
        public string BusId
        {
            get => _busId;
            set { _busId = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private string _model;
        public string Model
        {
            get => _model;
            set { _model = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private string _year;
        public string Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private double _batteryCapacity;
        public double BatteryCapacity
        {
            get => _batteryCapacity;
            set { _batteryCapacity = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

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

        private double _consumption;
        public double Consumption
        {
            get => _consumption;
            set { _consumption = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private DateTime _lastUpdate;
        public DateTime LastUpdate
        {
            get => _lastUpdate;
            set { _lastUpdate = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private BusStatus _status;
        public BusStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public bool IsCritical => BatteryLevel < 20;

        public string DisplayText => $"{BusId} | {Model} | {BatteryLevel:F1}%";

        private RouteName _route;
        public RouteName Route
        {
            get => _route;
            set { _route = value; OnPropertyChanged(); }
        }

        public bool IsInService => Status == BusStatus.Inroute || Status == BusStatus.Intercept || Status == BusStatus.Return;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 