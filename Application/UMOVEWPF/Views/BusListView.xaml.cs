using System.Windows.Controls;

namespace UMOVEWPF.Views
{
    public partial class BusListView : UserControl
    {
        public BusListView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is UMOVEWPF.ViewModels.BusListViewModel vm)
                await vm.InitializeAsync();
        }
    }
} 