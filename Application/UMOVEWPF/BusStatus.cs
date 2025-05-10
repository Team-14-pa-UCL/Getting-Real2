using System;

namespace UMOVEWPF
{
    public enum BusStatus
    {
        Inroute,
        Intercept, // Status for, hvis den er på vej til at erstatte en anden bus.
        Return,
        Free,
        Garage,
        Charging,
        Repair
    }
} 