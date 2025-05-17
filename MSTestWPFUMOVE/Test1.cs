using Microsoft.VisualStudio.TestTools.UnitTesting;
using UMOVEWPF.Models;
using UMOVEWPF.ViewModels;

namespace MSTestWPFUMOVE
{
    [TestClass]
    public class WPFBasicTests
    {
        [TestMethod]
        public void CanCreateBusAndSetProperties()
        {
            var bus = new Bus
            {
                BusId = "BUS001",
                Model = BusModel.YutongE12,
                Year = "2023",
                BatteryCapacity = 422,
                Consumption = 0.84,
                BatteryLevel = 100,
                Status = BusStatus.Garage
            };
            Assert.AreEqual("BUS001", bus.BusId);
            Assert.AreEqual(BusModel.YutongE12, bus.Model);
            Assert.AreEqual(422, bus.BatteryCapacity);
            Assert.AreEqual(BusStatus.Garage, bus.Status);
        }

        [TestMethod]
        public void CanAddBusToMainViewModel()
        {
            var vm = new MainViewModel();
            var bus = new Bus { BusId = "BUS002", Model = BusModel.BYDK9, Year = "2022" };
            vm.Buses.Add(bus);
            Assert.IsTrue(vm.Buses.Contains(bus));
        }
    }
}
