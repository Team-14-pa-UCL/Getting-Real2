using System;

namespace UMOVEWPF.Models
{
    /// <summary>
    /// Mulige statusser for en bus.
    /// Bruges til at angive hvor bussen befinder sig eller hvad den laver.
    /// </summary>
    public enum BusStatus
    {
        /// <summary>
        /// Bussen er i rute (kører passagerer)
        /// </summary>
        Inroute,
        /// <summary>
        /// På vej til at erstatte en anden bus
        /// </summary>
        Intercept, // Status for, hvis den er på vej til at erstatte en anden bus.
        /// <summary>
        /// På vej tilbage til garage
        /// </summary>
        Return,
        /// <summary>
        /// Fri/ledig
        /// </summary>
        Free,
        /// <summary>
        /// Holder i garage
        /// </summary>
        Garage,
        /// <summary>
        /// Lader op
        /// </summary>
        Charging,
        /// <summary>
        /// Til reparation
        /// </summary>
        Repair
    }
} 