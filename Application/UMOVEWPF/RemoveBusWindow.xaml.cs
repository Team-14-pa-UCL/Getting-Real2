using System.Windows;

namespace UMOVEWPF
{
    public partial class RemoveBusWindow : Window
    {
        public RemoveBusWindow()
        {
            InitializeComponent();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 