using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Umove_AS_WPF.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Umove_AS_WPF.ViewModels
{
    public class BusViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Bus> _buses;
        private Bus _selectedBus;

        public BusViewModel()
        {
            _buses = new ObservableCollection<Bus>();
            AddBusCommand = new RelayCommand(ExecuteAddBus);
            EditBusCommand = new RelayCommand(ExecuteEditBus, CanExecuteEditBus);
            DeleteBusCommand = new RelayCommand(ExecuteDeleteBus, CanExecuteDeleteBus);
            ViewDetailsCommand = new RelayCommand(ExecuteViewDetails, CanExecuteViewDetails);
        }

        public ObservableCollection<Bus> Buses
        {
            get => _buses;
            set
            {
                if (_buses != value)
                {
                    _buses = value;
                    OnPropertyChanged();
                }
            }
        }

        public Bus SelectedBus
        {
            get => _selectedBus;
            set
            {
                if (_selectedBus != value)
                {
                    _selectedBus = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddBusCommand { get; }
        public ICommand EditBusCommand { get; }
        public ICommand DeleteBusCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExecuteAddBus(object parameter)
        {
            // Implementation
        }

        private void ExecuteEditBus(object parameter)
        {
            // Implementation
        }

        private bool CanExecuteEditBus(object parameter)
        {
            return SelectedBus != null;
        }

        private void ExecuteDeleteBus(object parameter)
        {
            // Implementation
        }

        private bool CanExecuteDeleteBus(object parameter)
        {
            return SelectedBus != null;
        }

        private void ExecuteViewDetails(object parameter)
        {
            // Implementation
        }

        private bool CanExecuteViewDetails(object parameter)
        {
            return SelectedBus != null;
        }

        public List<Bus> GetAllBuses()
        {
            // Implementation - return list of buses
            return new List<Bus>();
        }

        public void AddBus(Bus bus)
        {
            _buses.Add(bus);
            OnPropertyChanged(nameof(Buses));
        }

        public void UpdateBus(Bus bus)
        {
            var existingBus = _buses.FirstOrDefault(b => b.Id == bus.Id);
            if (existingBus != null)
            {
                var index = _buses.IndexOf(existingBus);
                _buses[index] = bus;
                OnPropertyChanged(nameof(Buses));
            }
        }

        public void DeleteBus(string busId)
        {
            var bus = _buses.FirstOrDefault(b => b.Id == busId);
            if (bus != null)
            {
                _buses.Remove(bus);
                OnPropertyChanged(nameof(Buses));
            }
        }
    }
} 