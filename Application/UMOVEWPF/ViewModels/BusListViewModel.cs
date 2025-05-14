using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UMOVEWPF.Helpers;
using UMOVEWPF.Models;
using System;
using System.Linq;

namespace UMOVEWPF.ViewModels
{
    public class BusListViewModel : BaseViewModel
    {
        private ObservableCollection<Bus> _buses;
        public ObservableCollection<Bus> Buses
        {
            get => _buses;
            set => SetProperty(ref _buses, value);
        }

        private Bus _selectedBus;
        public Bus SelectedBus
        {
            get => _selectedBus;
            set => SetProperty(ref _selectedBus, value);
        }

        private string _batteryLevelInput;
        public string BatteryLevelInput
        {
            get => _batteryLevelInput;
            set => SetProperty(ref _batteryLevelInput, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddBusCommand { get; }
        public ICommand EditBusCommand { get; }
        public ICommand DeleteBusCommand { get; }
        public ICommand UpdateBatteryStatusCommand { get; }
        public ICommand UpdateBatteryLevelCommand { get; }

        public BusListViewModel()
        {
            Buses = new ObservableCollection<Bus>();
            RefreshCommand = new RelayCommand(async _ => await LoadBusesAsync());
            AddBusCommand = new RelayCommand(_ => AddBus());
            EditBusCommand = new RelayCommand(_ => EditBus(), _ => SelectedBus != null);
            DeleteBusCommand = new RelayCommand(_ => DeleteBus(), _ => SelectedBus != null);
            UpdateBatteryStatusCommand = new RelayCommand(_ => UpdateBatteryStatus());
            UpdateBatteryLevelCommand = new RelayCommand(_ => UpdateBatteryLevel(), _ => SelectedBus != null && double.TryParse(BatteryLevelInput, out double _));

            // Load buses immediately when the ViewModel is created
            LoadBusesAsync().GetAwaiter().GetResult();
        }

        private async Task LoadBusesAsync()
        {
            var loadedBuses = await FileHelper.LoadBusesAsync();

            // Fjern busser der ikke længere findes
            for (int i = Buses.Count - 1; i >= 0; i--)
            {
                var bus = Buses[i];
                if (!loadedBuses.Any(b => b.BusId == bus.BusId))
                    Buses.RemoveAt(i);
            }

            // Opdater eksisterende og tilføj nye
            foreach (var loadedBus in loadedBuses)
            {
                var existing = Buses.FirstOrDefault(b => b.BusId == loadedBus.BusId);
                if (existing != null)
                {
                    // Opdater alle properties
                    existing.Year = loadedBus.Year;
                    existing.BatteryCapacity = loadedBus.BatteryCapacity;
                    existing.Consumption = loadedBus.Consumption;
                    existing.Route = loadedBus.Route;
                    existing.BatteryLevel = loadedBus.BatteryLevel;
                    existing.Status = loadedBus.Status;
                    existing.LastUpdate = loadedBus.LastUpdate;
                }
                else
                {
                    Buses.Add(loadedBus);
                }
            }
        }

        private void AddBus()
        {
            var newBus = new Bus
            {
                BusId = "New Bus",
                Model = "New Model",
                Year = DateTime.Now.Year.ToString(),
                Status = BusStatus.Free
            };
            Buses.Add(newBus);
            SelectedBus = newBus;
        }

        private void EditBus()
        {
            // This will be handled by the BusDetailViewModel
        }

        private async void DeleteBus()
        {
            if (SelectedBus != null)
            {
                Buses.Remove(SelectedBus);
                await FileHelper.SaveBusesAsync(Buses);
            }
        }

        private void UpdateBatteryStatus()
        {
            var rand = new Random();
            foreach (var bus in Buses)
            {
                if (bus.Status == BusStatus.Inroute || bus.Status == BusStatus.Intercept || bus.Status == BusStatus.Returning)
                {
                    bus.BatteryLevel -= rand.Next(1, 5); //Reducer batteriet med et random på 1-5%
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
                
                // Update both BatteryLevel and BatteryInfo.Level to ensure all views update
                SelectedBus.BatteryLevel = newLevel;
                if (SelectedBus.BatteryInfo != null)
                {
                    SelectedBus.BatteryInfo.Level = newLevel;
                }
                SelectedBus.LastUpdate = DateTime.Now;
                
                if (newLevel < 30)
                {
                    System.Windows.MessageBox.Show($"Advarsel: {SelectedBus.BusId} har lavt batteriniveau ({newLevel:F1}%)!", "Lavt batteri", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
                BatteryLevelInput = string.Empty;
                // Gem busser hvis nødvendigt
                FileHelper.SaveBusesAsync(Buses);
            }
        }
    }
} 