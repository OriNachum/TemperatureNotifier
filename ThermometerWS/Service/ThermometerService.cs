using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ThermometerWS.Service.ThermometerReader;

namespace ThermometerWS.Service
{
    public class ThermometerService : IThermometerService
    {
        private readonly Dictionary<Func<bool>, IThermometerReader> ThermometerConfigurations = new Dictionary<Func<bool>, IThermometerReader>
        {
            { () => RuntimeInformation.IsOSPlatform(OSPlatform.Windows), new WindowsDummyThermometerReader() },

            // Currently assuming we only have Debian
            { () => RuntimeInformation.IsOSPlatform(OSPlatform.Linux), new DebianThermometerReader() },
        };


        public async Task<double?> GetTempratureAsync()
        {
            IThermometerReader thermometerReader = GetThermometerReader();
            if (thermometerReader == null)
            {
                // Log error
                return null;
            }

            // logDebug thermometerFound
            double? temperature = await thermometerReader.GetCurrentMeasurementAsync();
            if (!temperature.HasValue)
            {
                // Log error
            }
            return temperature;
        }

        private IThermometerReader GetThermometerReader()
        {
            foreach (KeyValuePair<Func<bool>, IThermometerReader> thermometerConfiguration in ThermometerConfigurations)
            {
                if (thermometerConfiguration.Key.Invoke())
                {
                    return thermometerConfiguration.Value;
                }
            }

            return null;
        }
    }
}
