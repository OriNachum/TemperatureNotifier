using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThermalNotifierWS.Service.NotifyTemperatureConditionProviders
{
    public class NotifyAlways : INotifyTemperatureProvider
    {
        public bool ShouldNotify(double temperature, double? previousTemperature)
        {
            return true;
        }

        public string GenerateMessage(double temperature) => $"Temperature is: {temperature}℃";
    }
}
