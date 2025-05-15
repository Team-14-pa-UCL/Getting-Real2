using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UMOVEWPF.Models;

namespace UMOVEWPF.ViewModels
{
    public class BusReplacementViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Bus> _allBuses;
        private readonly Bus _lowBatteryBus;
        private Bus _selectedReplacementBus;
        private bool _canPostpone = true;
        private int _postponeCount = 0;

        public Bus LowBatteryBus => _lowBatteryBus;
        public ObservableCollection<Bus> AvailableBuses { get; }
        public Bus SelectedReplacementBus
        {
            get => _selectedReplacementBus;
            set
            {
                _selectedReplacementBus = value;
                OnPropertyChanged();
            }
        }

        public bool CanPostpone
        {
            get => _canPostpone;
            set
            {
                _canPostpone = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectBusCommand { get; }
        public ICommand PostponeCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler<Bus> BusSelected;
        public event EventHandler Postponed;
        public event EventHandler Cancelled;

        public BusReplacementViewModel(ObservableCollection<Bus> allBuses, Bus lowBatteryBus)
        {
            _allBuses = allBuses;
            _lowBatteryBus = lowBatteryBus;
            AvailableBuses = new ObservableCollection<Bus>(
                allBuses.Where(b => b.Status == BusStatus.Garage && b.BatteryLevel >= 50)
            );

            SelectBusCommand = new RelayCommand(_ => OnBusSelected());
            PostponeCommand = new RelayCommand(_ => OnPostponed());
            CancelCommand = new RelayCommand(_ => OnCancelled());
        }

        private void OnBusSelected()
        {
            if (SelectedReplacementBus == null) return;

            // Set the replacement bus to intercept status
            SelectedReplacementBus.Status = BusStatus.Intercept;
            SelectedReplacementBus.Route = _lowBatteryBus.Route;

            // Set the low battery bus to returning status
            _lowBatteryBus.Status = BusStatus.Returning;

            BusSelected?.Invoke(this, SelectedReplacementBus);
        }

        private void OnPostponed()
        {
            _postponeCount++;
            if (_postponeCount >= 2)
            {
                CanPostpone = false;
            }
            Postponed?.Invoke(this, EventArgs.Empty);
        }

        private void OnCancelled()
        {
            Cancelled?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 