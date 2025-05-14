using System.Windows;
using UMOVEWPF.Models;

namespace UMOVEWPF.Views
{
    public partial class AddEditBusWindow : Window
    {
        public Bus Bus { get; set; } //Bus instance med ID, Batteriniveau.

        public AddEditBusWindow()
        {
            InitializeComponent(); //Indl�ser Xaml, der h�rer til vundet.
            Bus = new Bus
            {
                BatteryLevel = 100, //Starter med 100%
                Status = BusStatus.Garage //Starter i Garagen
            };
            DataContext = Bus; //Binder et til vinduets UI, s� busobjeketet opdateres automatisk.
        }

        /// <summary>
        /// Opdater en valgt bus.
        /// </summary>
        /// <param name="busToEdit"></param>
        public AddEditBusWindow(Bus busToEdit)
        {
            InitializeComponent();
            Bus = busToEdit; // Brug samme instans!
            DataContext = Bus;
        }

        //N�r man trykker p� tilf�j. S� gemmer den og lukket vinduet.
        private void OnSave(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Bus.BusId))
            {
                MessageBox.Show("Bus ID må ikke være tomt.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Bus.Year))
            {
                MessageBox.Show("År skal udfyldes.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Bus.BatteryCapacity <= 0)
            {
                MessageBox.Show("Batterikapacitet skal være større end 0.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Bus.Consumption <= 0)
            {
                MessageBox.Show("Forbrug skal være større end 0.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

    }
} 