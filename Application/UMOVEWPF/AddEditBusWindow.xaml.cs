using System.Windows;

namespace UMOVEWPF
{
    public partial class AddEditBusWindow : Window
    {
        public Bus Bus { get; set; }

        public AddEditBusWindow()
        {
            InitializeComponent();
            Bus = new Bus();
            DataContext = Bus;
        }

        public AddEditBusWindow(Bus busToEdit)
        {
            InitializeComponent();
            Bus = new Bus
            {
                BusId = busToEdit.BusId,
                Model = busToEdit.Model,
                Year = busToEdit.Year
            };
            BusIdTextBox.Text = Bus.BusId;
            ModelTextBox.Text = Bus.Model;
            YearTextBox.Text = Bus.Year;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Bus.BusId = BusIdTextBox.Text;
            Bus.Model = ModelTextBox.Text;
            Bus.Year = YearTextBox.Text;
            DialogResult = true;
            Close();
        }
    }
} 