using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMOVEWPF.Models
{
    public class Weather
    {
 
        //Forslag til viewModel, aka Martin: 

        /*     public double GetBaseRate()
        {
            // Træk 1 fra, fordi DateTime.Month går fra 1-12, enum går fra 0-11
            Month måned = (Month)(DateTime.Now.Month - 1);

            switch (måned)
            {
                case Month.December:
                case Month.January:
                case Month.February:
                    return 1.4;

                case Month.March:
                case Month.November:
                    return 1.2;

                case Month.April:
                case Month.October:
                    return 1.0;

                case Month.May:
                case Month.September:
                    return 0.9;

                case Month.June:
                case Month.July:
                case Month.August:
                    return 0.8;

                default:
                    return 1.0;
            }
        }*/
        public enum Month
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        private bool _isRaining; //Tilføj evt "wiperOn" betingelse i bus, skal påvirke baserate, fx +0,1. Udkommentér hvis undlades.

        public bool IsRaining
        {
            get { return _isRaining; }
            set
            {
                _isRaining = value;
            }
        }

    }
}
