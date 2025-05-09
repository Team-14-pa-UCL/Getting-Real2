using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Umove_AS_WPF.Models
{
    public class Bus : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private int _batteryLevel;
        private BusStatus _status;
        private string _location;
        private DateTime _lastMaintenance;
        private DateTime _nextMaintenance;
        private int _kilometers;

        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public int BatteryLevel
        {
            get => _batteryLevel;
            set
            {
                if (_batteryLevel != value)
                {
                    _batteryLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        public BusStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime LastMaintenance
        {
            get => _lastMaintenance;
            set
            {
                if (_lastMaintenance != value)
                {
                    _lastMaintenance = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime NextMaintenance
        {
            get => _nextMaintenance;
            set
            {
                if (_nextMaintenance != value)
                {
                    _nextMaintenance = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Kilometers
        {
            get => _kilometers;
            set
            {
                if (_kilometers != value)
                {
                    _kilometers = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Bus()
        {
            Id = Guid.NewGuid().ToString();
            Status = BusStatus.Available;
            LastMaintenance = DateTime.Now;
            NextMaintenance = DateTime.Now.AddMonths(1);
            BatteryLevel = 100;
            Kilometers = 0;
        }

        public void UpdateStatus(BusStatus newStatus)
        {
            Status = newStatus;
            OnPropertyChanged(nameof(Status));
        }
    }
} 