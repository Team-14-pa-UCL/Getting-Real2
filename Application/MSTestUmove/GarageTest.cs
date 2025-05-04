using Microsoft.VisualStudio.TestTools.UnitTesting;
using Umove_AS.Models;
using Umove_AS.Services;
using Umove_AS.Type;
using System;
using System.Linq;
using Umove_AS.Type;

namespace MSTestUmove
{
    [TestClass]
    public class GarageTest
    {
        [TestMethod]
        public void AddBus_AddbusSucces()
        {
            // Arrange
            var shiftplan = new ShiftPlan("nat");
            var busService = new BusService();
            var shiftPlanService = new ShiftPlanService();
            var garageService = new GarageService(busService, shiftPlanService);
            var bus = new Bus("Bus1", 300, 2.5, shiftplan);

            // Act
            busService.AddBus(bus);

            // Assert
            var allBuses = busService.GetAllBusses();
            Assert.AreEqual(1, allBuses.Count);
            Assert.AreEqual("Bus1", allBuses[0].ID);
        }

        [TestMethod]
        public void AddBus_Duplicate_ThrowsException()
        {
            // Arrange
            var shiftplan = new ShiftPlan("nat");
            var busService = new BusService();
            var shiftPlanService = new ShiftPlanService();
            var garageService = new GarageService(busService, shiftPlanService);

            // Act
            busService.AddBus(new Bus("Bus1", 300, 2.5, shiftplan));

            // Assert + expect no exception yet
            Assert.ThrowsException<ArgumentException>(() => busService.AddBus(new Bus("Bus1", 300, 2.5, shiftplan)));
        }

        [TestMethod]
        public void EditBus_UpdateData()
        {
            // Arrange
            var shiftplan = new ShiftPlan("nat");
            var busService = new BusService();
            var shiftPlanService = new ShiftPlanService();
            var garageService = new GarageService(busService, shiftPlanService);
            busService.AddBus(new Bus("Bus1", 300, 2.5, shiftplan));

            // Act
            busService.EditBus("Bus1", 350, 3.0);
            var bus = busService.GetBusById("Bus1");

            // Assert
            Assert.AreEqual(350, bus.BatteryCapacity);
            Assert.AreEqual(3.0, bus.KmPerKWh);
        }

        [TestMethod]
        public void RemoveBus_Delete()
        {
            // Arrange
            var shiftplan = new ShiftPlan("nat");
            var busService = new BusService();
            var shiftPlanService = new ShiftPlanService();
            var garageService = new GarageService(busService, shiftPlanService);
            busService.AddBus(new Bus("Bus1", 300, 2.5, shiftplan));

            // Act
            busService.RemoveBus("Bus1");

            // Assert
            var buses = busService.GetAllBusses();
            Assert.AreEqual(0, buses.Count);
        }

        [TestMethod]
        public void UpdateBusStatus_CorrectStatus()
        {
            // Arrange
            var shiftplan = new ShiftPlan("nat");
            var busService = new BusService();
            var shiftPlanService = new ShiftPlanService();
            var garageService = new GarageService(busService, shiftPlanService);
            busService.AddBus(new Bus("Bus1", 300, 2.5, shiftplan));

            // Act
            busService.UpdateBusStatus("Bus1", BusStatus.Charging);
            var bus = busService.GetBusById("Bus1");

            // Assert
            Assert.AreEqual(BusStatus.Charging, bus.Status);
        }
    }
}
