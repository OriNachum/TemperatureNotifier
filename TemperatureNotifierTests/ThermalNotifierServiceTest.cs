using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TemperatureNotifierTests.Framework;
using ThermalNotifierWS.Service;
using Xunit;

namespace TemperatureNotifierTests
{
    public class ThermalNotifierServiceTest
    {
        private ILogger _logger;
        private ThermalNotifierService _thermalNotifierService;
        private MockHttpMessageHandler _mockHttpMessageHandler;

        public ThermalNotifierServiceTest()
        {
            _mockHttpMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(_mockHttpMessageHandler);

            _logger = A.Fake<ILogger>();
            _thermalNotifierService = new ThermalNotifierService(httpClient, _logger);
        }

        [Fact]
        public async Task NotifyTest()
        {
            _mockHttpMessageHandler.QueueNextResponse("32.45", HttpStatusCode.OK);
            await _thermalNotifierService.NotifyTemperatureAsync();

            Assert.Equal(2, _mockHttpMessageHandler.NumberOfCalls);
        }
    }
}
