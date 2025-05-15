using System.Windows;
using UMOVEWPF.ViewModels;

namespace UMOVEWPF.Views
{
    public partial class BusReplacementWindow : Window
    {
        public BusReplacementWindow(BusReplacementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 