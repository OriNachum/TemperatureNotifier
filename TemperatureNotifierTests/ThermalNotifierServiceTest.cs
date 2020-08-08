using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Theory]
        [InlineData(0, 1, false)]
        [InlineData(1, 2, false)]
        [InlineData(2, 2, true)]
        public async Task NotifyTemperature_OkResponses_CallsAtleastOneMoreAndTrueOnSuccess(int numberOfOKResponses, int numberOfCalls, bool result)
        {
            foreach (int index in Enumerable.Range(0, numberOfOKResponses))
            {
                _mockHttpMessageHandler.QueueNextResponse("32.45", HttpStatusCode.OK);
            }
            foreach (int index in Enumerable.Range(numberOfOKResponses, 2))
            {
                _mockHttpMessageHandler.QueueNextResponse("32.45", HttpStatusCode.NotFound);
            }
            bool actualResult = await _thermalNotifierService.NotifyTemperatureAsync();
            Assert.Equal(numberOfCalls, _mockHttpMessageHandler.NumberOfCalls);
            Assert.Equal(result, actualResult);
        }
    }
}
