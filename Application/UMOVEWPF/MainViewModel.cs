using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace UMOVEWPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<UMOVEWPF.Bus> Buses { get; set; } = new ObservableCollection<UMOVEWPF.Bus>();

        private UMOVEWPF.Bus? _selectedBus; // Explicitly specify the namespace to resolve ambiguity
        public UMOVEWPF.Bus? SelectedBus
        {
            get => _selectedBus;
            set { _selectedBus = value; OnPropertyChanged(); }
        }

        public ICommand AddBusCommand { get; }
        public ICommand EditBusCommand { get; }
        public ICommand RemoveBusCommand { get; }

        public MainViewModel()
        {
            AddBusCommand = new RelayCommand(_ => AddBus());
            EditBusCommand = new RelayCommand(_ => EditBus(), _ => SelectedBus != null);
            RemoveBusCommand = new RelayCommand(_ => RemoveBus(), _ => SelectedBus != null);
        }

        private void AddBus()
        {
            var win = new AddEditBusWindow();
            if (win.ShowDialog() == true)
                Buses.Add(win.Bus);
        }

        private void EditBus()
        {
            if (SelectedBus == null) return;
            var win = new AddEditBusWindow(SelectedBus);
            if (win.ShowDialog() == true)
            {
                SelectedBus.BusId = win.Bus.BusId;
                SelectedBus.Model = win.Bus.Model;
                SelectedBus.Year = win.Bus.Year;
                OnPropertyChanged(nameof(SelectedBus));
            }
        }

        private void RemoveBus()
        {
            if (SelectedBus == null) return;
            var win = new RemoveBusWindow();
            if (win.ShowDialog() == true)
                Buses.Remove(SelectedBus);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 