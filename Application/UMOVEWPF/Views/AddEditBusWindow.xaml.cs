using System.Windows;
using UMOVEWPF.Models;
using UMOVEWPF.ViewModels;

namespace UMOVEWPF.Views
{
    public partial class AddEditBusWindow : Window
    {
        public AddEditBusViewModel ViewModel { get; set; }

        public AddEditBusWindow()
        {
            InitializeComponent(); //Indlser Xaml, der hrer til vundet.
            ViewModel = new AddEditBusViewModel();
            DataContext = ViewModel;
        }

        /// <summary>
        /// Opdater en valgt bus.
        /// </summary>
        /// <param name="busToEdit"></param>
        public AddEditBusWindow(Bus busToEdit)
        {
            InitializeComponent();
            ViewModel = new AddEditBusViewModel(busToEdit);
            DataContext = ViewModel;
        }

        //Nr man trykker p tilfj. S gemmer den og lukket vinduet.
        private void OnSave(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.Bus.BusId))
            {
                MessageBox.Show("Bus ID må ikke være tomt.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(ViewModel.Bus.Year))
            {
                MessageBox.Show("År skal udfyldes.", "Validering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
} 