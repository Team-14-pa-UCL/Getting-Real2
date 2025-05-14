using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UMOVEWPF.Models
{
    public class BatteryInfo : INotifyPropertyChanged
    {
        private double _capacity;
        public double Capacity
        {
            get => _capacity;
            set { _capacity = value; OnPropertyChanged(); }
        }

        private double _level;
        public double Level
        {
            get => _level;
            set 
            { 
                _level = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCritical));
            }
        }

        private double _consumption;
        public double Consumption
        {
            get => _consumption;
            set { _consumption = value; OnPropertyChanged(); }
        }

        public bool IsCritical => Level < 20;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 