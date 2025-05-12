using System.Windows;
using UMOVEWPF.Models;

namespace UMOVEWPF.Views
{
    public partial class AddEditBusWindow : Window
    {
        public Bus Bus { get; set; } //Bus instance med ID, Batteriniveau.

        public AddEditBusWindow()
        {
            InitializeComponent(); //Indlæser Xaml, der hører til vundet.
            Bus = new Bus
            {
                BatteryLevel = 100, //Starter med 100%
                Status = BusStatus.Garage //Starter i Garagen
            };
            DataContext = Bus; //Binder et til vinduets UI, så busobjeketet opdateres automatisk.
        }

        /// <summary>
        /// Opdater en valgt bus.
        /// </summary>
        /// <param name="busToEdit"></param>
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

        //Når man trykker på tilføj. Så gemmer den og lukket vinduet.
        private void OnSave(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
} 