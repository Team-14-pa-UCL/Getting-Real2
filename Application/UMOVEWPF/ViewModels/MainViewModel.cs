using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UMOVEWPF.Models;
using UMOVEWPF.Helpers;
using UMOVEWPF.Views;
using UMOVEWPF; // For SimulationService

namespace UMOVEWPF.ViewModels
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

        private string _batteryLevelInput;
        public string BatteryLevelInput
        {
            get => _batteryLevelInput;
            set { _batteryLevelInput = value; OnPropertyChanged(); }
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
            set { _showOnlyCritical = value; OnPropertyChanged(); FilterBuses(); }
        }

        private bool _showOnlyCriticalToggle;
        public bool ShowOnlyCriticalToggle
        {
            get => _showOnlyCriticalToggle;
            set { _showOnlyCriticalToggle = value; OnPropertyChanged(); FilterBuses(); OnPropertyChanged(nameof(CriticalButtonText)); }
        }

        public string CriticalButtonText => ShowOnlyCriticalToggle ? "Se alle busser" : "Se kun kritiske busser";

        /// <summary>
        /// Collection der indeholder de filtrerede busser baseret på søgetekst og kritiske busser filter
        /// </summary>
        private ObservableCollection<Bus> _filteredBuses;
        public ObservableCollection<Bus> FilteredBuses
        {
            get => _filteredBuses;
            set
            {
                _filteredBuses = value;
                OnPropertyChanged(nameof(FilteredBuses));
            }
        }

        /// <summary>
        /// Søgetekst der bruges til at filtrere busserne
        /// Når denne ændres, opdateres FilteredBuses automatisk
        /// </summary>
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterBuses();
            }
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

        private SimulationService _simService;

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
            FilteredBuses = new ObservableCollection<Bus>();
            _simService = new SimulationService(Buses);
            _simService.Start(); // Start live simulering automatisk
        }

        public async Task InitializeAsync()
        {
            await LoadBusesAsync();

            if (Buses.Count == 0)
            {
                Buses.Add(new Bus { BusId = "BUS001", Year = "2023", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName.R1A, BatteryLevel = 85 });
                Buses.Add(new Bus { BusId = "BUS002", Year = "2022", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName.R11, BatteryLevel = 45 });
                Buses.Add(new Bus { BusId = "BUS003", Year = "2023", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName.R1A, BatteryLevel = 15 });
                Buses.Add(new Bus { BusId = "BUS004", Year = "2021", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName.R13, BatteryLevel = 60 });
                Buses.Add(new Bus { BusId = "BUS005", Year = "2020", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName.R137, BatteryLevel = 30 });
                Buses.Add(new Bus { BusId = "BUS006", Year = "2022", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName.R1A, BatteryLevel = 10 });
                Buses.Add(new Bus { BusId = "BUS007", Year = "2023", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName.R139, BatteryLevel = 99 });
                Buses.Add(new Bus { BusId = "BUS008", Year = "2021", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName.R139, BatteryLevel = 55 });
                Buses.Add(new Bus { BusId = "BUS009", Year = "2020", BatteryCapacity = 393, Consumption = 1.2, Route = RouteName.R1A, BatteryLevel = 22 });
                Buses.Add(new Bus { BusId = "BUS010", Year = "2022", BatteryCapacity = 393, Consumption = 2.0, Route = RouteName.R10, BatteryLevel = 70 });
                await SaveBusesAsync();
            }

            // Initialiser FilteredBuses med alle busser
            FilteredBuses = new ObservableCollection<Bus>(Buses);

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
                
                // Opdater FilteredBuses når Buses ændres
                FilterBuses();
            };
        }

        private void Bus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Bus.Status) ||
                e.PropertyName == nameof(Bus.Route) ||
                e.PropertyName == nameof(Bus.BatteryLevel) ||
                e.PropertyName == nameof(Bus.Year) ||
                e.PropertyName == nameof(Bus.BatteryCapacity) ||
                e.PropertyName == nameof(Bus.Consumption))
            {
                SaveBusesAsync().GetAwaiter().GetResult();
            }
        }

        private void ShowBusAdmin() => CurrentView = "Bus Administration";
        private void ShowBatteryStatus() => CurrentView = "Batteri Status";
        private void ShowCriticalBuses() => CurrentView = "Kritiske Busser";
        private void ShowChargingPlan() => CurrentView = "Opladningsplan";

        /// <summary>
        /// Filtrerer busserne baseret på søgetekst og kritiske busser filter
        /// Køres automatisk når SearchText ændres eller ShowOnlyCriticalToggle ændres
        /// </summary>
        private void FilterBuses()
        {
            var filteredList = Buses.AsEnumerable();

            // Filtrer baseret på søgetekst - case-insensitive søgning
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredList = filteredList.Where(b => b.BusId.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrer baseret på kritiske busser (batteriniveau < 30%)
            if (ShowOnlyCriticalToggle)
            {
                filteredList = filteredList.Where(b => b.BatteryLevel < 30);
            }

            // Opdater FilteredBuses med de filtrerede resultater
            FilteredBuses.Clear();
            foreach (var bus in filteredList)
            {
                FilteredBuses.Add(bus);
            }
        }

        private void UpdateBatteryStatus()
        {
            double averageSpeedKmh = 47;
            double hours = 0.5; // 30 minutter
            foreach (var bus in Buses)
            {
                if (bus.Status == BusStatus.Inroute || bus.Status == BusStatus.Intercept || bus.Status == BusStatus.Returning)
                {
                    double distance = averageSpeedKmh * hours;
                    double consumptionKWh = distance * bus.Consumption;
                    double percentUsed = (consumptionKWh / bus.BatteryCapacity) * 100.0;
                    bus.BatteryLevel -= percentUsed;
                    if (bus.BatteryLevel < 0) bus.BatteryLevel = 0;
                    bus.LastUpdate = DateTime.Now;
                }
            }
        }

        private void UpdateBatteryLevel()
        {
            if (SelectedBus == null) return;
            if (!double.TryParse(BatteryLevelInput, out double newLevel))
            {
                MessageBox.Show("Ugyldigt input. Indtast et tal.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (newLevel < 0 || newLevel > 100)
            {
                MessageBox.Show("Batteriniveau skal være mellem 0 og 100.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SelectedBus.BatteryLevel = newLevel;
            SelectedBus.LastUpdate = DateTime.Now;
            if (newLevel < 30)
            {
                MessageBox.Show($"Advarsel: {SelectedBus.BusId} har lavt batteriniveau ({newLevel:F1}%)!", "Lavt batteri", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            BatteryLevelInput = string.Empty;
            SaveBusesAsync().GetAwaiter().GetResult();
        }

        private void AddBus()
        {
            var win = new AddEditBusWindow();
            if (win.ShowDialog() == true)
            {
                var newBusId = win.Bus.BusId?.Trim();
                if (string.IsNullOrWhiteSpace(newBusId))
                {
                    MessageBox.Show("Bus ID er ugyldigt.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Buses.Any(b => b.BusId.Equals(newBusId, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show($"Bus med ID '{newBusId}' findes allerede.", "Dublet ID", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Buses.Add(win.Bus);
                SaveBusesAsync().GetAwaiter().GetResult();
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
                SelectedBus.Model = win.Bus.Model;
                SelectedBus.BatteryCapacity = win.Bus.BatteryCapacity;
                SelectedBus.Consumption = win.Bus.Consumption;
                SelectedBus.Route = win.Bus.Route;
                SaveBusesAsync().GetAwaiter().GetResult();
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
                SaveBusesAsync().GetAwaiter().GetResult();
            }
        }

        private async Task SaveBusesAsync()
        {
            await FileHelper.SaveBusesAsync(Buses);
        }

        private async Task LoadBusesAsync()
        {
            Buses.Clear();
            var loadedBuses = await FileHelper.LoadBusesAsync();
            foreach (var bus in loadedBuses)
            {
                Buses.Add(bus);
            }
        }

        private void ToggleCriticalBuses()
        {
            ShowOnlyCriticalToggle = !ShowOnlyCriticalToggle;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
