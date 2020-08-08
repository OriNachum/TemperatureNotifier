using SlackNotifierWS.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TemperatureNotifierTests
{
    public class SlackNotifierServiceTest
    {
        private readonly SlackNotifierService _slackNotifierService;
        private string SlackEndpoint = "https://hooks.slack.com/services/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";

        public SlackNotifierServiceTest()
        {
            _slackNotifierService = new SlackNotifierService(new HttpClient());
        }

        [Fact]
        public async Task TestNotificationAsync()
        {
            await _slackNotifierService.NotifyAsync(SlackEndpoint, "Test");
        }
    }
}
