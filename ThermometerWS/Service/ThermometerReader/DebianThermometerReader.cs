using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ThermometerWS.Service.ThermometerReader
{
    public class DebianThermometerReader : IThermometerReader
    {
        const string ThermometerFileName = "digitemp_DS9097";
        const string SetupTemperatureSensorCommad = "-i -s /dev/ttyUSB0";
        const string ReadTemperatureCommad = "-q -t 0 -c .digitemprc";
        private bool SensorInitialized = false;
        public DebianThermometerReader()
        {
        }

        public async Task<double?> GetCurrentMeasurementAsync()
        {
            string sensorRead = await GetSensorOutputAsync();
            if (sensorRead == null || sensorRead.Split(' ').Count() < 7)
            {
                // log
                return null;
            }

            string valueInCelcious = sensorRead.Split(' ')[6];
            if (!double.TryParse(valueInCelcious, out double temperature))
            {
                // log
                return null;
            }

            return temperature;
        }

        private async Task<string> GetSensorOutputAsync()
        {
            if (!SensorInitialized)
            {
                await RunCommandAsync(SetupTemperatureSensorCommad);
                SensorInitialized = true;
            }
            string output = await RunCommandAsync(ReadTemperatureCommad);
            return output;
        }

        private static async Task<string> RunCommandAsync(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ThermometerFileName,
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
