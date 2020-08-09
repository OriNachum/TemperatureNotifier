using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThermometerWS.Service.ThermometerReader
{
    public class WindowsDummyThermometerReader : IThermometerReader
    {
        private const double RangeOfExpectedTempreture = 7;
        private const double MinimalTemprature = 22;

        private static Random _rng = new Random();

        public async Task<double?> GetCurrentMeasurementAsync()
        {
            var temperature = await Task.FromResult(_rng.NextDouble() * RangeOfExpectedTempreture + MinimalTemprature);
            return temperature;
        }
    }
}
