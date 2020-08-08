using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SlackNotifierWS.Service;

namespace SlackNotifierWS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlackNotifierController : ControllerBase
    {
        private readonly ILogger<SlackNotifierController> _logger;
        private readonly ISlackNotifierService _slackNotifierService;
        private string SlackEndpoint = "https://hooks.slack.com/services/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";


        public SlackNotifierController(ILogger<SlackNotifierController> logger, ISlackNotifierService slackNotifierService)
        {
            _logger = logger;
            _slackNotifierService = slackNotifierService;
        }

        // Use: https://localhost:6001/SlackNotifier?message=TestBrowser
        [HttpGet]
        public async Task Get(string message)
        {
            await _slackNotifierService.NotifyAsync(SlackEndpoint, message);
        }
    }
}
