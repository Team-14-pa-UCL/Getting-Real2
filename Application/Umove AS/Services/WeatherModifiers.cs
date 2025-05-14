using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Models;

namespace Umove_AS.Services
{
    public class WeatherModifiers //forhen WeatherService
    {
        public static readonly Dictionary<WeatherType, double> WeatherCondition = new Dictionary<WeatherType, double>
        {
            {WeatherType.WarmDry, 0.9},
            {WeatherType.WarmWet, 1.2},
            {WeatherType.AverageDry, 1.0},
            {WeatherType.AverageWet, 1.3},
            {WeatherType.ColdDry, 1.3},
            {WeatherType.ColdWet, 1.5},
            {WeatherType.FreezeDry, 2.0},
            {WeatherType.FreezeWet, 2.8},
        };
    }
}
