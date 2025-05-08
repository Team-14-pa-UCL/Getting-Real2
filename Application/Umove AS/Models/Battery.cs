using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.Models
{
    public class Battery
    {
        private double _maxCapacity; //Batteriets kapacitet i kWh ved 100%
        private double _stateOfChargeKwh; //Batteriets nuværende kapacitet i kWh (til intern brug)
        private double _stateOfCharge; //Batteriets nuværende kapacitet i %
        private double _batteryWear; //Batteriets nuværende slid i % - udregnes ud fra kilometerstand og årgang på bussen
        private double _currentConsumptionKwh; //Batteriets nuværende strømforbrug i kW
    }

}
