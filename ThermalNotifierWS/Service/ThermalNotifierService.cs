using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ThermalNotifierWS.Service
{
    public class ThermalNotifierService : IThermalNotifierService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ThermalNotifierService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public /*async*/ Task AlertTemperatureAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> NotifyTemperatureAsync()
        {
            string requestTemperatureUrl = "https://localhost:7001/Thermometer";
            HttpResponseMessage responseTemperature = await _httpClient.GetAsync(requestTemperatureUrl);
            if (responseTemperature == null || responseTemperature.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"{requestTemperatureUrl} failed with status {responseTemperature?.StatusCode}");
                return false;
            }
            string temperature = await responseTemperature.Content.ReadAsStringAsync();

            string requestNotificationUrl = "https://localhost:6001/SlackNotifier";
            var notificationRequestUrlBuilder = new UriBuilder(requestNotificationUrl);
            notificationRequestUrlBuilder.Query = $"message={UrlEncoder.Default.Encode($"Temperature is: {temperature}℃")}";
            HttpResponseMessage responseNotification = await _httpClient.GetAsync(notificationRequestUrlBuilder.ToString());
            if (responseNotification == null || responseNotification.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"{requestNotificationUrl} failed with status {responseNotification?.StatusCode}");
                return false;
            }
            return true;
        }
    }
}
