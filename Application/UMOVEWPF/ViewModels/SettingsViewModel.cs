using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UMOVEWPF.Helpers;
using UMOVEWPF.Models;

namespace UMOVEWPF.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private ObservableCollection<Route> _routes;
        public ObservableCollection<Route> Routes
        {
            get => _routes;
            set => SetProperty(ref _routes, value);
        }

        private Route _selectedRoute;
        public Route SelectedRoute
        {
            get => _selectedRoute;
            set => SetProperty(ref _selectedRoute, value);
        }

        public ICommand AddRouteCommand { get; }
        public ICommand EditRouteCommand { get; }
        public ICommand DeleteRouteCommand { get; }
        public ICommand SaveCommand { get; }

        public SettingsViewModel()
        {
            Routes = new ObservableCollection<Route>();
            AddRouteCommand = new RelayCommand(_ => AddRoute());
            EditRouteCommand = new RelayCommand(_ => EditRoute(), _ => SelectedRoute != null);
            DeleteRouteCommand = new RelayCommand(_ => DeleteRoute(), _ => SelectedRoute != null);
            SaveCommand = new RelayCommand(async _ => await SaveRoutesAsync());

            LoadRoutesAsync().ConfigureAwait(false);
        }

        private async Task LoadRoutesAsync()
        {
            var routes = await FileHelper.LoadRoutesAsync();
            Routes.Clear();
            foreach (var route in routes)
            {
                Routes.Add(route);
            }
        }

        private void AddRoute()
        {
            var newRoute = new Route
            {
                Name = RouteName.None,
                Description = "New Route",
                Distance = 0,
                EstimatedTime = 0
            };
            Routes.Add(newRoute);
            SelectedRoute = newRoute;
        }

        private void EditRoute()
        {
            // This will be handled by the view
        }

        private void DeleteRoute()
        {
            if (SelectedRoute != null)
            {
                Routes.Remove(SelectedRoute);
            }
        }

        private async Task SaveRoutesAsync()
        {
            await FileHelper.SaveRoutesAsync(Routes);
        }
    }
} 