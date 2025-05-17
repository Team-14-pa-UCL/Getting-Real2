using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UMOVEWPF.Helpers;
using UMOVEWPF.Models;

namespace UMOVEWPF.ViewModels
{
    public class BusDetailViewModel : BaseViewModel
    {
        private Bus _bus;
        public Bus Bus
        {
            get => _bus;
            set => SetProperty(ref _bus, value);
        }

        private ObservableCollection<Route> _availableRoutes;
        public ObservableCollection<Route> AvailableRoutes
        {
            get => _availableRoutes;
            set => SetProperty(ref _availableRoutes, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public BusDetailViewModel(Bus bus = null)
        {
            Bus = bus ?? new Bus();
            AvailableRoutes = new ObservableCollection<Route>();
            SaveCommand = new RelayCommand(async _ => await SaveBusAsync());
            CancelCommand = new RelayCommand(_ => Cancel());

            LoadRoutesAsync().ConfigureAwait(false);
        }

        private async Task LoadRoutesAsync()
        {
            var routes = await FileHelper.LoadRoutesAsync();
            AvailableRoutes.Clear();
            foreach (var route in routes)
            {
                AvailableRoutes.Add(route);
            }
        }

        private async Task SaveBusAsync()
        {
            if (Bus != null)
            {
                // Update the bus's last update time
                Bus.LastUpdate = DateTime.Now;
                
                // Save the bus
                await FileHelper.SaveBusesAsync(new[] { Bus });
            }
        }

        private void Cancel()
        {
            // This will be handled by the view to close the window
        }
    }
} 