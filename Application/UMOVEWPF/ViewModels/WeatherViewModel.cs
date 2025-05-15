using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using UMOVEWPF.Models;

namespace UMOVEWPF.ViewModels
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        public Weather Weather { get; }
        public ObservableCollection<Weather.Month> Months { get; }
        public Weather.Month SelectedMonth
        {
            get => Weather.SelectedMonth;
            set { Weather.SelectedMonth = value; OnPropertyChanged(nameof(SelectedMonth)); }
        }
        public bool IsRaining
        {
            get => Weather.IsRaining;
            set { Weather.IsRaining = value; OnPropertyChanged(nameof(IsRaining)); }
        }
        public ICommand OkCommand { get; }
        public event EventHandler OkClicked;

        public WeatherViewModel(Weather weather)
        {
            Weather = weather;
            Months = new ObservableCollection<Weather.Month>(Enum.GetValues(typeof(Weather.Month)).Cast<Weather.Month>());
            OkCommand = new RelayCommand(_ => OkClicked?.Invoke(this, EventArgs.Empty));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 