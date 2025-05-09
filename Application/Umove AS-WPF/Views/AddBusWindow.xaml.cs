using System;
using System.Windows;
using Umove_AS_WPF.Models;

namespace Umove_AS_WPF.Views
{
    public partial class AddBusWindow : Window
    {
        public Bus NewBus { get; private set; }
        private bool _isEditMode;

        public AddBusWindow(Bus existingBus = null)
        {
            InitializeComponent();
            _isEditMode = existingBus != null;
            
            // Initialize status combo box
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(BusStatus));
            
            if (_isEditMode)
            {
                // Edit mode
                HeaderText.Text = "Edit Bus";
                NameTextBox.Text = existingBus.Name;
                StatusComboBox.SelectedItem = existingBus.Status;
                BatterySlider.Value = existingBus.BatteryLevel;
                LocationTextBox.Text = existingBus.Location;
                NewBus = existingBus;
            }
            else
            {
                // Add mode
                StatusComboBox.SelectedItem = BusStatus.Available;
            }

            // Add event handler for battery slider
            BatterySlider.ValueChanged += BatterySlider_ValueChanged;
        }

        private void BatterySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BatteryLevelText.Text = $"{Math.Round(BatterySlider.Value)}%";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Please enter a bus name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(LocationTextBox.Text))
            {
                MessageBox.Show("Please enter a location.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create or update bus
            if (!_isEditMode)
            {
                NewBus = new Bus
                {
                    Id = Guid.NewGuid(),
                    Name = NameTextBox.Text,
                    Status = (BusStatus)StatusComboBox.SelectedItem,
                    BatteryLevel = (int)BatterySlider.Value,
                    Location = LocationTextBox.Text
                };
            }
            else
            {
                NewBus.Name = NameTextBox.Text;
                NewBus.Status = (BusStatus)StatusComboBox.SelectedItem;
                NewBus.BatteryLevel = (int)BatterySlider.Value;
                NewBus.Location = LocationTextBox.Text;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 