using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThermalNotifierWS.Service
{
    public static class ThermalNotifierServiceTemperatureHistory
    {
        public static double? LastKnownTemperature { get; set; }
    }
}
