using System.Collections.ObjectModel;
using System.ComponentModel;
using UMOVEWPF.Models;
using System;
using System.Linq;

namespace UMOVEWPF.ViewModels
{
    public class AddEditBusViewModel : INotifyPropertyChanged
    {
        public Bus Bus { get; set; }
        public ObservableCollection<RouteName> Routes { get; }
        public ObservableCollection<BusModel> Models { get; }

        public AddEditBusViewModel(Bus bus = null)
        {
            Bus = bus ?? new Bus { Model = BusModel.MBeCitaro, BatteryLevel = 100, Status = BusStatus.Garage };
            Routes = new ObservableCollection<RouteName>(Enum.GetValues(typeof(RouteName)).Cast<RouteName>().Where(r => r != RouteName.None));
            Models = new ObservableCollection<BusModel>(Enum.GetValues(typeof(BusModel)).Cast<BusModel>());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 