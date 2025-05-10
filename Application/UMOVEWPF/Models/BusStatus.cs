using System;

namespace UMOVEWPF.Models
{
    /// <summary>
    /// Mulige statusser for en bus.
    /// Bruges til at angive hvor bussen befinder sig eller hvad den laver.
    /// </summary>
    public enum BusStatus
    {
        Inroute,
        
        Intercept, // Status for, hvis den er på vej til at erstatte en anden bus.
        
        Returning, // Bus der er blevet aflyst og på vej tilbage for at charge.
        
        Free,
        
        Garage,
        
        Charging,
        
        Repair
    }
} 