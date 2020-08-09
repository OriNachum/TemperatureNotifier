using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ThermometerWS.Service.ThermometerReader
{
    public class DebianThermometerReader : IThermometerReader
    {
        public async Task<double?> GetCurrentMeasurementAsync()
        {
            string sensorRead = await GetSensorOutputAsync();
            string valueInCelcious = sensorRead.Split(' ')[6];
            if (!double.TryParse(valueInCelcious, out double temperature))
            {
                // log
                return null;
            }

            return temperature;
        }

        private static async Task<string> GetSensorOutputAsync()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = "digitemp_DS9097 -q -t 0 -c .digitemprc", };
            using Process proc = new Process() { StartInfo = startInfo, };
            proc.Start();
            proc.WaitForExit();
            return await proc.StandardOutput.ReadToEndAsync();
        }
    }
}
