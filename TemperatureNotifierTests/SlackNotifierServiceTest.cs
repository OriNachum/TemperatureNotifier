using SlackNotifierWS.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TemperatureNotifierTests.Framework;
using Xunit;

namespace TemperatureNotifierTests
{
    public class SlackNotifierServiceTest
    {
        private readonly SlackNotifierService _slackNotifierService;
        private readonly MockHttpMessageHandler _mockHttpMessageHandler;
        private const string SlackEndpoint = "https://hooks.slack.com/services/TNEVER5GONNA/GIVE3YOU3UP7NEVER2GONNA5LET5YOU4DOWN";

        public SlackNotifierServiceTest()
        {
            _mockHttpMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(_mockHttpMessageHandler);

            _slackNotifierService = new SlackNotifierService(httpClient);
        }

        [Fact]
        public async Task NotifyAsync_SendTest_GetCallWithTest()
        {
            _mockHttpMessageHandler.EnqueueNextResponse("Sababa", HttpStatusCode.OK);
            await _slackNotifierService.NotifyAsync(SlackEndpoint, "Test");
            Assert.Equal(1, _mockHttpMessageHandler.NumberOfCalls);
            Assert.Equal("payload={\"text\": \"Test\"}\n", _mockHttpMessageHandler.Input);
        }
    }
}
