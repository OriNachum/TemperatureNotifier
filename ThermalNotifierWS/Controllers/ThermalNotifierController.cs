﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ThermalNotifierWS.Service;

namespace ThermalNotifierWS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThermalNotifierController : ControllerBase
    {
        private readonly ILogger<ThermalNotifierController> _logger;
        private readonly IThermalNotifierService _thermalNotifierService;

        public ThermalNotifierController(ILogger<ThermalNotifierController> logger, IThermalNotifierService thermalNotifierService)
        {
            _logger = logger;
            _thermalNotifierService = thermalNotifierService;
        }

        // Use: https://localhost:8001/ThermalNotifier
        [HttpGet]
        public async Task<bool> Get()
        {
            return await _thermalNotifierService.NotifyTemperatureAsync();
        }
    }
}
