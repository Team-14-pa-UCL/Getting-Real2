using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Markup;
using UMOVEWPF.ViewModels;

namespace UMOVEWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml 
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Variabler til at holde styr på sorteringsretning for hver kolonne
        /// </summary>
        private bool _sortByIdDescending = true;
        private bool _sortByRouteDescending = true;
        private bool _sortByBatteryDescending = true;
        private bool _sortByStatusDescending = true;
        private bool _sortByInServiceDescending = true;
        private string _sortedColumn = "";
        private bool _isSortDescending = true;

        /// <summary>
        /// Nem adgang til ViewModel via DataContext. Som betyder at de arbejder med samme Data.
        /// </summary>
        private MainViewModel ViewModel => DataContext as MainViewModel;

        /// <summary>
        /// Initialiserer vinduet
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainViewModel();
            DataContext = viewModel;
            Loaded += async (_, __) => await viewModel.InitializeAsync();
        }

        /// <summary>
        /// Opdaterer sorteringsikoner på kolonneoverskrifter
        /// </summary>
        private void UpdateSortIcons()
        {
            BtnSortId.Content = _sortedColumn == "ID" ? $"ID {(_isSortDescending ? "▼" : "▲")}" : "ID";
            BtnSortRoute.Content = _sortedColumn == "Rute" ? $"Rute {(_isSortDescending ? "▼" : "▲")}" : "Rute";
            BtnSortBattery.Content = _sortedColumn == "Batteri" ? $"Batteri {(_isSortDescending ? "▼" : "▲")}" : "Batteri";
            BtnSortStatus.Content = _sortedColumn == "Status" ? $"Status {(_isSortDescending ? "▼" : "▲")}" : "Status";
            BtnSortInService.Content = _sortedColumn == "I drift" ? $"I drift {(_isSortDescending ? "▼" : "▲")}" : "I drift";
        }

        /// <summary>
        /// Sorterer busser efter ID når kolonneoverskriften klikkes
        /// </summary>
        private void SortByBusId_Click(object sender, RoutedEventArgs e)
        {
            // Sorter listen og opdater ViewModel
            var sorted = _sortByIdDescending
                ? ViewModel.Buses.OrderByDescending(b => b.BusId).ToList()
                : ViewModel.Buses.OrderBy(b => b.BusId).ToList();
            ViewModel.Buses.Clear();
            foreach (var bus in sorted)
                ViewModel.Buses.Add(bus);
            _sortedColumn = "ID";
            _isSortDescending = _sortByIdDescending;
            _sortByIdDescending = !_sortByIdDescending;
            UpdateSortIcons();
        }

        /// <summary>
        /// Sorterer busser efter rute
        /// </summary>
        private void SortByRoute_Click(object sender, RoutedEventArgs e)
        {
            var sorted = _sortByRouteDescending
                ? ViewModel.Buses.OrderByDescending(b => b.Route).ToList()
                : ViewModel.Buses.OrderBy(b => b.Route).ToList();
            ViewModel.Buses.Clear();
            foreach (var bus in sorted)
                ViewModel.Buses.Add(bus);
            _sortedColumn = "Rute";
            _isSortDescending = _sortByRouteDescending;
            _sortByRouteDescending = !_sortByRouteDescending;
            UpdateSortIcons();
        }

        /// <summary>
        /// Sorterer busser efter batteriniveau
        /// </summary>
        private void SortByBattery_Click(object sender, RoutedEventArgs e)
        {
            var sorted = _sortByBatteryDescending
                ? ViewModel.Buses.OrderByDescending(b => b.BatteryLevel).ToList()
                : ViewModel.Buses.OrderBy(b => b.BatteryLevel).ToList();
            ViewModel.Buses.Clear();
            foreach (var bus in sorted)
                ViewModel.Buses.Add(bus);
            _sortedColumn = "Batteri";
            _isSortDescending = _sortByBatteryDescending;
            _sortByBatteryDescending = !_sortByBatteryDescending;
            UpdateSortIcons();
        }

        /// <summary>
        /// Sorterer busser efter status
        /// </summary>
        private void SortByStatus_Click(object sender, RoutedEventArgs e)
        {
            var sorted = _sortByStatusDescending
                ? ViewModel.Buses.OrderByDescending(b => b.BatteryLevel).ToList()
                : ViewModel.Buses.OrderBy(b => b.BatteryLevel).ToList();
            ViewModel.Buses.Clear();
            foreach (var bus in sorted)
                ViewModel.Buses.Add(bus);
            _sortedColumn = "Status";
            _isSortDescending = _sortByStatusDescending;
            _sortByStatusDescending = !_sortByStatusDescending;
            UpdateSortIcons();
        }

        /// <summary>
        /// Sorterer busser efter om de er i drift
        /// </summary>
        private void SortByInService_Click(object sender, RoutedEventArgs e)
        {
            var sorted = _sortByInServiceDescending
                ? ViewModel.Buses.OrderByDescending(b => b.Status.ToString()).ToList()
                : ViewModel.Buses.OrderBy(b => b.Status.ToString()).ToList();
            ViewModel.Buses.Clear();
            foreach (var bus in sorted)
                ViewModel.Buses.Add(bus);
            _sortedColumn = "I drift";
            _isSortDescending = _sortByInServiceDescending;
            _sortByInServiceDescending = !_sortByInServiceDescending;
            UpdateSortIcons();
        }
    }
}