using System.Windows;
using System.Windows.Input;
using UMOVEWPF.ViewModels;

namespace UMOVEWPF
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }
    }

    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand ShowBusListCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        public MainViewModel()
        {
            ShowBusListCommand = new RelayCommand(_ => ShowBusList());
            ShowSettingsCommand = new RelayCommand(_ => ShowSettings());

            // Show bus list by default
            ShowBusList();
        }

        private void ShowBusList()
        {
            CurrentView = new BusListViewModel();
        }

        private void ShowSettings()
        {
            CurrentView = new SettingsViewModel();
        }
    }
} 