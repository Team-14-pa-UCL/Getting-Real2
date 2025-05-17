using System.Windows;
using UMOVEWPF.ViewModels;

namespace UMOVEWPF.Views
{
    public partial class WeatherWindow : Window
    {
        public WeatherWindow(WeatherViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 