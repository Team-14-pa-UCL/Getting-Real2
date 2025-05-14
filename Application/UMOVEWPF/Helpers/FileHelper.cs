using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UMOVEWPF.Models;

namespace UMOVEWPF.Helpers
{
    public static class FileHelper
    {
        private static readonly string BusesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "buses.txt");
        private static readonly string RoutesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "routes.txt");

        public static async Task SaveBusesAsync(IEnumerable<Bus> buses)
        {
            using (var sw = new StreamWriter(BusesFile, false))
            {
                foreach (var bus in buses)
                {
                    await sw.WriteLineAsync($"{bus.BusId};{bus.Year};{bus.BatteryCapacity};{bus.Consumption};{bus.Route};{bus.BatteryLevel};{bus.Status};{bus.LastUpdate:O}");
                }
            }
        }

        public static async Task<List<Bus>> LoadBusesAsync()
        {
            var buses = new List<Bus>();
            if (!File.Exists(BusesFile))
                return buses;

            using (var sr = new StreamReader(BusesFile))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length >= 8)
                    {
                        buses.Add(new Bus
                        {
                            BusId = parts[0],
                            Year = parts[1],
                            BatteryCapacity = double.TryParse(parts[2], out var cap) ? cap : 0,
                            Consumption = double.TryParse(parts[3], out var cons) ? cons : 0,
                            Route = Enum.TryParse<RouteName>(parts[4], out var route) ? route : RouteName.None,
                            BatteryLevel = double.TryParse(parts[5], out var lvl) ? lvl : 0,
                            Status = Enum.TryParse<BusStatus>(parts[6], out var stat) ? stat : BusStatus.Garage,
                            LastUpdate = DateTime.TryParse(parts[7], out var dt) ? dt : DateTime.Now
                        });
                    }
                }
            }
            return buses;
        }

        public static async Task SaveRoutesAsync(IEnumerable<Route> routes)
        {
            using (var sw = new StreamWriter(RoutesFile, false))
            {
                foreach (var route in routes)
                {
                    await sw.WriteLineAsync($"{route.Name};{route.Description};{route.Distance};{route.EstimatedTime}");
                }
            }
        }

        public static async Task<List<Route>> LoadRoutesAsync()
        {
            var routes = new List<Route>();
            if (!File.Exists(RoutesFile))
                return routes;

            using (var sr = new StreamReader(RoutesFile))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length >= 4)
                    {
                        routes.Add(new Route
                        {
                            Name = Enum.TryParse<RouteName>(parts[0], out var name) ? name : RouteName.None,
                            Description = parts[1],
                            Distance = double.TryParse(parts[2], out var dist) ? dist : 0,
                            EstimatedTime = int.TryParse(parts[3], out var time) ? time : 0
                        });
                    }
                }
            }
            return routes;
        }
    }
} 