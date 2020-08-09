using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ThermometerWS.Service.ThermometerReader
{
    public class DebianThermometerReader : IThermometerReader
    {
        const string SetupTemperatureSensorCommad = "digitemp_DS9097 -i -s /dev/ttyUSB0";
        const string ReadTemperatureCommad = "digitemp_DS9097 -q -t 0 -c .digitemprc";

        public DebianThermometerReader()
        {
            RunCommandAsync(SetupTemperatureSensorCommad).Wait();
        }

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
            string output = await RunCommandAsync(ReadTemperatureCommad);
            return output;
        }

        private static async Task<string> RunCommandAsync(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = command,
                RedirectStandardOutput = true,
            };
            Process proc = new Process() { StartInfo = startInfo, };
            proc.Start();
            proc.WaitForExit();
            return await proc.StandardOutput.ReadToEndAsync();
        }
    }
}
