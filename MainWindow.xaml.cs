private void SortByBusId_Click(object sender, RoutedEventArgs e)
{
    var sortedBuses = new ObservableCollection<Bus>(ViewModel.Buses.OrderBy(b => b.BusId));
    ViewModel.Buses.Clear();
    foreach (var bus in sortedBuses)
    {
        ViewModel.Buses.Add(bus);
    }
} 