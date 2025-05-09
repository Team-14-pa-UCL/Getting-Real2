using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Umove_AS_WPF.Models;
using Umove_AS_WPF.ViewModels;

namespace Umove_AS_WPF.Views
{
    public partial class MainWindow : Window
    {
        private List<Bus> _buses;
        private BusViewModel _busViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _busViewModel = new BusViewModel();
            LoadBuses();
        }

        private void LoadBuses()
        {
            _buses = _busViewModel.GetAllBuses();
            BusGrid.ItemsSource = _buses;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            var filteredBuses = _buses.Where(b => 
                b.Name.ToLower().Contains(searchText) ||
                b.Location.ToLower().Contains(searchText) ||
                b.Status.ToString().ToLower().Contains(searchText)
            ).ToList();
            
            BusGrid.ItemsSource = filteredBuses;
        }

        private void AddBusButton_Click(object sender, RoutedEventArgs e)
        {
            var addBusWindow = new AddBusWindow();
            if (addBusWindow.ShowDialog() == true)
            {
                var newBus = addBusWindow.NewBus;
                if (newBus != null)
                {
                    _busViewModel.AddBus(newBus);
                    LoadBuses();
                }
            }
        }

        private void EditBusButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var bus = button.DataContext as Bus;
            if (bus != null)
            {
                var editBusWindow = new AddBusWindow(bus);
                if (editBusWindow.ShowDialog() == true)
                {
                    var updatedBus = editBusWindow.NewBus;
                    if (updatedBus != null)
                    {
                        _busViewModel.UpdateBus(updatedBus);
                        LoadBuses();
                    }
                }
            }
        }

        private void DeleteBusButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var bus = button.DataContext as Bus;
            if (bus != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete bus {bus.Name}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _busViewModel.DeleteBus(bus.Id);
                    LoadBuses();
                }
            }
        }

        private void BusGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedBus = BusGrid.SelectedItem as Bus;
            if (selectedBus != null)
            {
                // Handle bus selection if needed
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBuses();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
} 