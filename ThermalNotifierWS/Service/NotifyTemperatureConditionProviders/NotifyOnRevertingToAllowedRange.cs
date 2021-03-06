﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThermalNotifierWS.Service.NotifyTemperatureConditionProviders
{
    public class NotifyOnRevertingToAllowedRange : INotifyTemperatureProvider
    {
        private readonly double _minTemperature;
        private readonly double _maxTemperature;

        public NotifyOnRevertingToAllowedRange(double minTemperature, double maxTemperature)
        {
            _minTemperature = minTemperature;
            _maxTemperature = maxTemperature;
        }

        public bool ShouldNotify(double temperature, double? previousTemperature)
        {
            return TemperatureIsInRange(temperature) &&
                   (!previousTemperature.HasValue || !TemperatureIsInRange(previousTemperature.Value));
        }

        private bool TemperatureIsInRange(double temperature)
        {
            return temperature >= _minTemperature && temperature <= _maxTemperature;
        }

        public string GenerateMessage(double temperature) => $"Temperature back to allowed range: {temperature}℃";
    }
}
