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
            set { _selectedBus = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentConsumption)); OnPropertyChanged(nameof(MonthAndFactor)); OnPropertyChanged(nameof(WiperStatus)); }
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

        public double CurrentConsumption => SelectedBus != null ? Math.Round(SelectedBus.Consumption * Weather.ConsumptionMultiplier, 2) : 0.0;
        public string MonthAndFactor => $"Måned: {Weather.SelectedMonth} ({Weather.GetConsumptionMultiplier():0.##}x)";
        public string WiperStatus => Weather.IsRaining ? "Vinduesvisker: Tændt" : "Vinduesvisker: Slukket";

        public Weather Weather { get; set; } = new Weather();
        public ICommand ShowWeatherCommand { get; }

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
            ShowWeatherCommand = new RelayCommand(_ => ShowWeatherWindow());
            FilteredBuses = new ObservableCollection<Bus>();

            Weather.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Weather.SelectedMonth) || e.PropertyName == nameof(Weather.IsRaining))
                {
                    OnPropertyChanged(nameof(CurrentConsumption));
                    OnPropertyChanged(nameof(MonthAndFactor));
                    OnPropertyChanged(nameof(WiperStatus));
                }
            };
        }

        public async Task InitializeAsync()
        {
            await LoadBusesAsync();

            // Opret demo-busser hvis listen stadig er tom efter indlæsning
            if (Buses.Count == 0)
            {
                var random = new Random();
                var newBuses = new List<Bus>();
                for (int i = 1; i <= 80; i++)
                {
                    bool isGarage = i <= 30;
                    var bus = new Bus
                    {
                        BusId = $"BUS{i:D3}",
                        Year = (2020 + (i % 4)).ToString(),
                        BatteryCapacity = 422,
                        Consumption = 0.84,
                        Model = BusModel.YutongE12,
                        Route = (RouteName)(i % (Enum.GetValues(typeof(RouteName)).Length - 1) + 1), // Skip None
                        BatteryLevel = isGarage ? 100 : random.Next(35, 101),
                        Status = isGarage ? BusStatus.Garage : BusStatus.Inroute,
                        LastUpdate = DateTime.Now.AddMinutes(-random.Next(0, 300)),
                        StatusChangedAt = DateTime.Now.AddMinutes(-random.Next(0, 300))
                    };
                    newBuses.Add(bus);
                }
                Buses = new ObservableCollection<Bus>(newBuses);
                FilteredBuses = new ObservableCollection<Bus>(Buses);
                await SaveBusesAsync();
            }

            // Bind PropertyChanged only once for each bus
            foreach (var bus in Buses)
                bus.PropertyChanged += Bus_PropertyChanged;

            // Bind CollectionChanged only once
            Buses.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                    foreach (Bus bus in e.NewItems)
                        bus.PropertyChanged += Bus_PropertyChanged;
                if (e.OldItems != null)
                    foreach (Bus bus in e.OldItems)
                        bus.PropertyChanged -= Bus_PropertyChanged;
                FilterBuses();
            };

            // Kald kun FilterBuses én gang efter initialisering
            FilterBuses();

            // Opret og start SimulationService EFTER Buses er sat
            _simService = new SimulationService(Buses, Weather);
            _simService.Start();
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
            double consumptionMultiplier = Weather.ConsumptionMultiplier;
            DateTime simulatedNow = DateTime.Now;
            foreach (var bus in Buses)
            {
                if (bus.Status == BusStatus.Inroute || bus.Status == BusStatus.Intercept || bus.Status == BusStatus.Returning)
                {
                    double distance = averageSpeedKmh * hours;
                    double consumptionKWh = distance * bus.Consumption * consumptionMultiplier;
                    double percentUsed = (consumptionKWh / bus.BatteryCapacity) * 100.0;
                    bus.BatteryLevel -= percentUsed;
                    if (bus.BatteryLevel < 0) bus.BatteryLevel = 0;
                    bus.LastUpdate = bus.LastUpdate.AddMinutes(30); // Simuleret tid
                    simulatedNow = bus.LastUpdate; // Bruges til status-tjek
                }
                // Opladning af busser i Charging
                if (bus.Status == BusStatus.Charging)
                {
                    double chargePowerKWh = 150 * hours; // 75 kWh per 30 min
                    double percentCharged = (chargePowerKWh / bus.BatteryCapacity) * 100.0;
                    bus.BatteryLevel += percentCharged;
                    if (bus.BatteryLevel >= 100)
                    {
                        bus.BatteryLevel = 100;
                        bus.Status = BusStatus.Garage;
                        bus.StatusChangedAt = bus.LastUpdate.AddMinutes(30);
                    }
                    bus.LastUpdate = bus.LastUpdate.AddMinutes(30);
                }
                // Vis kun advarsel hvis bus er Inroute og under 30%
                if (bus.Status == BusStatus.Inroute && bus.BatteryLevel < 30)
                {
                    ShowBusReplacementDialog(bus);
                }
            }

            // Simuler status-skift baseret på simuleret tid
            foreach (var bus in Buses.ToList())
            {
                // Hvis bus er Intercept og der er gået 30 min siden StatusChangedAt
                if (bus.Status == BusStatus.Intercept && (bus.LastUpdate - bus.StatusChangedAt).TotalMinutes >= 30)
                {
                    // Find den bus der skal returnere (den der blev afløst)
                    var replacedBus = Buses.FirstOrDefault(b => b.Status == BusStatus.Returning && (bus.Route == b.Route));
                    bus.Status = BusStatus.Inroute;
                    bus.StatusChangedAt = bus.LastUpdate; // Simuleret tid
                    if (replacedBus != null)
                    {
                        replacedBus.Status = BusStatus.Returning;
                        replacedBus.StatusChangedAt = replacedBus.LastUpdate; // Simuleret tid
                    }
                }
                // Hvis bus er Returning og der er gået 30 min siden StatusChangedAt
                if (bus.Status == BusStatus.Returning && (bus.LastUpdate - bus.StatusChangedAt).TotalMinutes >= 30)
                {
                    bus.Status = BusStatus.Charging;
                    bus.StatusChangedAt = bus.LastUpdate; // Simuleret tid
                }
            }
            SaveBusesAsync().GetAwaiter().GetResult();
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
                ShowBusReplacementDialog(SelectedBus);
            }
            BatteryLevelInput = string.Empty;
            SaveBusesAsync().GetAwaiter().GetResult();
        }

        private void ShowBusReplacementDialog(Bus lowBatteryBus)
        {
            var viewModel = new BusReplacementViewModel(Buses, lowBatteryBus);
            var window = new BusReplacementWindow(viewModel);

            viewModel.BusSelected += (s, replacementBus) =>
            {
                window.Close();
                // Set simulated time for status change
                replacementBus.Status = BusStatus.Intercept;
                replacementBus.StatusChangedAt = replacementBus.LastUpdate;
                replacementBus.Route = lowBatteryBus.Route;
                lowBatteryBus.Status = BusStatus.Returning;
                lowBatteryBus.StatusChangedAt = lowBatteryBus.LastUpdate;
                StartBusReplacementProcess(lowBatteryBus, replacementBus);
            };

            viewModel.Postponed += (s, e) =>
            {
                window.Close();
                // Schedule the warning to show again in 30 minutes
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromMinutes(30);
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    ShowBusReplacementDialog(lowBatteryBus);
                };
                timer.Start();
            };

            viewModel.Cancelled += (s, e) =>
            {
                window.Close();
            };

            window.ShowDialog();
        }

        private void StartBusReplacementProcess(Bus lowBatteryBus, Bus replacementBus)
        {
            // Set the replacement bus to intercept status immediately
            replacementBus.Status = BusStatus.Intercept;
            SaveBusesAsync().GetAwaiter().GetResult();

            // Start a timer for the replacement bus to reach the low battery bus (30 minutes)
            var interceptTimer = new System.Windows.Threading.DispatcherTimer();
            interceptTimer.Interval = TimeSpan.FromMinutes(30);
            interceptTimer.Tick += (s, e) =>
            {
                interceptTimer.Stop();
                // After 30 min: replacement bus goes Inroute, low battery bus goes Returning
                replacementBus.Status = BusStatus.Inroute;
                lowBatteryBus.Status = BusStatus.Returning;
                SaveBusesAsync().GetAwaiter().GetResult();

                // Start a timer for the low battery bus to return to garage (30 minutes)
                var returnTimer = new System.Windows.Threading.DispatcherTimer();
                returnTimer.Interval = TimeSpan.FromMinutes(30);
                returnTimer.Tick += (s2, e2) =>
                {
                    returnTimer.Stop();
                    lowBatteryBus.Status = BusStatus.Charging;
                    SaveBusesAsync().GetAwaiter().GetResult();
                };
                returnTimer.Start();
            };
            interceptTimer.Start();
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

        private void ShowWeatherWindow()
        {
            var vm = new WeatherViewModel(Weather);
            var win = new Views.WeatherWindow(vm);
            vm.OkClicked += (s, e) => win.Close();
            win.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
