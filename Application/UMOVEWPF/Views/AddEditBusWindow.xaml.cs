using System.Windows;
using UMOVEWPF.Models;

namespace UMOVEWPF.Views
{
    public partial class AddEditBusWindow : Window
    {
        public Bus Bus { get; set; }

        public AddEditBusWindow()
        {
            InitializeComponent();
            Bus = new Bus
            {
                BatteryLevel = 100,
                Status = BusStatus.Garage
            };
            DataContext = Bus;
        }

        public AddEditBusWindow(Bus busToEdit)
        {
            InitializeComponent();
            Bus = new Bus
            {
                BusId = busToEdit.BusId,
                Year = busToEdit.Year,
                BatteryCapacity = busToEdit.BatteryCapacity,
                Consumption = busToEdit.Consumption,
                Route = busToEdit.Route
            };
            DataContext = Bus;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
} 