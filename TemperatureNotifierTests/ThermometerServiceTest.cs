using System;
using System.Linq;
using System.Threading.Tasks;
using ThermometerWS.Service;
using Xunit;

namespace TemperatureNotifierTests
{
    public class ThermometerServiceTest
    {
        private readonly ThermometerService _thermometerService;

        public ThermometerServiceTest()
        {
            _thermometerService = new ThermometerService();
        }

        [Fact]
        public async Task TestWorking()
        {
            foreach (var index in Enumerable.Range(1, 1000))
            {
                double temperature = await _thermometerService.GetTempratureAsync();
                Assert.True(temperature >= 22 && temperature <= 29);
            }
        }
    }
}
