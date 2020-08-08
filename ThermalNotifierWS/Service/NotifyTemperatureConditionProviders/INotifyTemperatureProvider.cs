﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThermalNotifierWS.Service.NotifyTemperatureConditionProviders
{
    public interface INotifyTemperatureProvider
    {
        bool ShouldNotify(double temperature);

        string GenerateMessage(double temperature);
    }
}
