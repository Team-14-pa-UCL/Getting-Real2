using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace UMOVEWPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Bus> Buses { get; set; } = new ObservableCollection<Bus>();
        private Bus _selectedBus;
        public Bus SelectedBus
        {
            get => _selectedBus;
            set { _selectedBus = value; OnPropertyChanged(); }
        }

        private string _currentView = "Bus Administration";
        public string CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        private bool _showOnlyCritical;
        public bool ShowOnlyCritical
        {
            get => _showOnlyCritical;
            set 
            { 
                _showOnlyCritical = value; 
                OnPropertyChanged();
                FilterBuses();
            }
        }

        private bool _showOnlyCriticalToggle;
        public bool ShowOnlyCriticalToggle
        {
            get => _showOnlyCriticalToggle;
            set { _showOnlyCriticalToggle = value; OnPropertyChanged(); FilterBuses(); OnPropertyChanged(nameof(CriticalButtonText)); }
        }

        public string CriticalButtonText => ShowOnlyCriticalToggle ? "Se alle busser" : "Se kun kritiske busser";

        private string _batteryLevelInput;
        public string BatteryLevelInput
        {
            get => _batteryLevelInput;
            set { _batteryLevelInput = value; OnPropertyChanged(); }
        }

        public ICommand AddBusCommand { get; }
        public ICommand EditBusCommand { get; }
        public ICommand RemoveBusCommand { get; }
        public ICommand ShowBusAdminCommand { get; }
        public ICommand ShowBatteryStatusCommand { get; }
        public ICommand ShowCriticalBusesCommand { get; }
        public ICommand ShowChargingPlanCommand { get; }
        public ICommand UpdateBatteryStatusCommand { get; }
        public ICommand UpdateBatteryLevelCommand { get; }
        public ICommand ToggleCriticalBusesCommand { get; }

        private readonly string _busFilePath = "buses.txt";

        public MainViewModel()
        {
            AddBusCommand = new RelayCommand(_ => AddBus());
            EditBusCommand = new RelayCommand(_ => EditBus(), _ => SelectedBus != null);
            RemoveBusCommand = new RelayCommand(_ => RemoveBus(), _ => SelectedBus != null);
            ShowBusAdminCommand = new RelayCommand(_ => ShowBusAdmin());
            ShowBatteryStatusCommand = new RelayCommand(_ => ShowBatteryStatus());
            ShowCriticalBusesCommand = new RelayCommand(_ => ShowCriticalBuses());
            ShowChargingPlanCommand = new RelayCommand(_ => ShowChargingPlan());
            UpdateBatteryStatusCommand = new RelayCommand(_ => UpdateBatteryStatus());
            UpdateBatteryLevelCommand = new RelayCommand(_ => UpdateBatteryLevel(), _ => SelectedBus != null && double.TryParse(BatteryLevelInput, out double _));
            ToggleCriticalBusesCommand = new RelayCommand(_ => ToggleCriticalBuses());

            LoadBuses();
            if (Buses.Count == 0)
            {
                // Simuler nogle test-data hvis filen ikke fandtes
                Buses.Add(new Bus { BusId = "BUS001", Year = "2023", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName._11, BatteryLevel = 85 });
                Buses.Add(new Bus { BusId = "BUS002", Year = "2022", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName._8A, BatteryLevel = 45 });
                Buses.Add(new Bus { BusId = "BUS003", Year = "2023", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName._85, BatteryLevel = 15 });
                Buses.Add(new Bus { BusId = "BUS004", Year = "2021", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName._11, BatteryLevel = 60 });
                Buses.Add(new Bus { BusId = "BUS005", Year = "2020", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName._8A, BatteryLevel = 30 });
                Buses.Add(new Bus { BusId = "BUS006", Year = "2022", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName._85, BatteryLevel = 10 });
                Buses.Add(new Bus { BusId = "BUS007", Year = "2023", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName._11, BatteryLevel = 99 });
                Buses.Add(new Bus { BusId = "BUS008", Year = "2021", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName._8A, BatteryLevel = 55 });
                Buses.Add(new Bus { BusId = "BUS009", Year = "2020", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName._85, BatteryLevel = 22 });
                Buses.Add(new Bus { BusId = "BUS010", Year = "2022", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName._11, BatteryLevel = 70 });
                SaveBuses();
            }
            // Lyt til ændringer på alle busser
            foreach (var bus in Buses)
                bus.PropertyChanged += Bus_PropertyChanged;
            Buses.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                    foreach (Bus bus in e.NewItems)
                        bus.PropertyChanged += Bus_PropertyChanged;
                if (e.OldItems != null)
                    foreach (Bus bus in e.OldItems)
                        bus.PropertyChanged -= Bus_PropertyChanged;
            };
        }

        private void Bus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Gem hvis relevante properties ændres
            if (e.PropertyName == nameof(Bus.Status) ||
                e.PropertyName == nameof(Bus.Route) ||
                e.PropertyName == nameof(Bus.BatteryLevel) ||
                e.PropertyName == nameof(Bus.Year) ||
                e.PropertyName == nameof(Bus.BatteryCapacity) ||
                e.PropertyName == nameof(Bus.Consumption))
            {
                SaveBuses();
            }
        }

        private void ShowBusAdmin()
        {
            CurrentView = "Bus Administration";
            ShowOnlyCritical = false;
        }

        private void ShowBatteryStatus()
        {
            CurrentView = "Batteri Status";
            ShowOnlyCritical = false;
        }

        private void ShowCriticalBuses()
        {
            CurrentView = "Kritiske Busser";
            ShowOnlyCritical = true;
        }

        private void ShowChargingPlan()
        {
            CurrentView = "Opladningsplan";
            ShowOnlyCritical = false;
        }

        private void FilterBuses()
        {
            if (ShowOnlyCriticalToggle)
            {
                var criticalBuses = Buses.Where(b => b.BatteryLevel < 30).ToList();
                Buses.Clear();
                foreach (var bus in criticalBuses)
                {
                    Buses.Add(bus);
                }
            }
            else
            {
                LoadBuses();
            }
        }

        private void UpdateBatteryStatus()
        {
            foreach (var bus in Buses)
            {
                if (bus.Status == BusStatus.Inroute || bus.Status == BusStatus.Intercept || bus.Status == BusStatus.Return)
                {
                    bus.BatteryLevel -= new Random().Next(1, 5);
                    bus.LastUpdate = DateTime.Now;
                    if (bus.BatteryLevel < 30)
                    {
                        System.Windows.MessageBox.Show($"Advarsel: {bus.BusId} har lavt batteriniveau ({bus.BatteryLevel:F1}%)!", "Lavt batteri", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void UpdateBatteryLevel()
        {
            if (SelectedBus == null) return;
            if (double.TryParse(BatteryLevelInput, out double newLevel))
            {
                if (newLevel < 0) newLevel = 0;
                if (newLevel > 100) newLevel = 100;
                SelectedBus.BatteryLevel = newLevel;
                SelectedBus.LastUpdate = DateTime.Now;
                if (newLevel < 30)
                {
                    System.Windows.MessageBox.Show($"Advarsel: {SelectedBus.BusId} har lavt batteriniveau ({newLevel:F1}%)!", "Lavt batteri", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
                BatteryLevelInput = string.Empty;
                SaveBuses();
            }
        }

        private void AddBus()
        {
            var win = new AddEditBusWindow();
            if (win.ShowDialog() == true)
            {
                Buses.Add(win.Bus);
                SaveBuses();
            }
        }

        private void EditBus()
        {
            if (SelectedBus == null) return;
            var win = new AddEditBusWindow(SelectedBus);
            if (win.ShowDialog() == true)
            {
                SelectedBus.BusId = win.Bus.BusId;
                SelectedBus.Year = win.Bus.Year;
                SelectedBus.BatteryCapacity = win.Bus.BatteryCapacity;
                SelectedBus.Consumption = win.Bus.Consumption;
                SelectedBus.Route = win.Bus.Route;
                SaveBuses();
                OnPropertyChanged(nameof(SelectedBus));
            }
        }

        private void RemoveBus()
        {
            if (SelectedBus == null) return;
            var win = new RemoveBusWindow();
            if (win.ShowDialog() == true)
            {
                Buses.Remove(SelectedBus);
                SaveBuses();
            }
        }

        private void SaveBuses()
        {
            using (var sw = new StreamWriter(_busFilePath, false))
            {
                foreach (var bus in Buses)
                {
                    sw.WriteLine($"{bus.BusId};{bus.Year};{bus.BatteryCapacity};{bus.Consumption};{bus.Route};{bus.BatteryLevel};{bus.Status};{bus.LastUpdate:O}");
                }
            }
        }

        private void LoadBuses()
        {
            Buses.Clear();
            if (!File.Exists(_busFilePath)) return;
            foreach (var line in File.ReadAllLines(_busFilePath))
            {
                var parts = line.Split(';');
                if (parts.Length >= 8)
                {
                    Buses.Add(new Bus
                    {
                        BusId = parts[0],
                        Year = parts[1],
                        BatteryCapacity = double.TryParse(parts[2], out var cap) ? cap : 0,
                        Consumption = double.TryParse(parts[3], out var cons) ? cons : 0,
                        Route = Enum.TryParse<RouteName>(parts[4], out var route) ? route : RouteName.None,
                        BatteryLevel = double.TryParse(parts[5], out var lvl) ? lvl : 0,
                        Status = Enum.TryParse<BusStatus>(parts[6], out var stat) ? stat : BusStatus.Garage,
                        LastUpdate = DateTime.TryParse(parts[7], out var dt) ? dt : DateTime.Now
                    });
                }
            }
        }

        private void ToggleCriticalBuses()
        {
            ShowOnlyCriticalToggle = !ShowOnlyCriticalToggle;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 