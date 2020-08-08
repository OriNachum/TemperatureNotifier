using System;
using System.Threading.Tasks;

namespace ThermometerWS.Service
{
    public class ThermometerService : IThermometerService
    {
        private const double RangeOfExpectedTempreture = 7;
        private const double MinimalTemprature = 32;

        private static Random _rng = new Random();

        public async Task<double> GetTempratureAsync()
        {
            var temperature = await Task.FromResult(_rng.NextDouble() * RangeOfExpectedTempreture + MinimalTemprature);
            return temperature;
        }
    }
}
