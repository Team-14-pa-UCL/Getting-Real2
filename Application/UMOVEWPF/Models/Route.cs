using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UMOVEWPF.Models
{
    public class Route : INotifyPropertyChanged
    {
        private RouteName _name;
        public RouteName Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private double _distance;
        public double Distance
        {
            get => _distance;
            set { _distance = value; OnPropertyChanged(); }
        }

        private int _estimatedTime;
        public int EstimatedTime
        {
            get => _estimatedTime;
            set { _estimatedTime = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 