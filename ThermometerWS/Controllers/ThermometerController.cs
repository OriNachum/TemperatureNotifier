using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ThermometerWS.Service;

namespace ThermometerWS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThermometerController : ControllerBase
    {
        private readonly ILogger<ThermometerController> _logger;
        private readonly IThermometerService _tempratureService;

        public ThermometerController(ILogger<ThermometerController> logger, IThermometerService tempratureService)
        {
            _logger = logger;
            _tempratureService = tempratureService;
        }

        // Use: https://localhost:7001/Thermometer
        [HttpGet]
        public async Task<double> Get()
        {
            double temperature = await _tempratureService.GetTempratureAsync();
            return temperature;
        }
    }
}
