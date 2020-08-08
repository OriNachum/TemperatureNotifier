using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ThermalNotifierWS.Service.NotifyTemperatureConditionProviders;

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
            return await NotifyTemperatureIfNeeded(new AlwaysNotifyTemperature());
        }

        private async Task<bool> NotifyTemperatureIfNeeded(INotifyTemperatureProvider shouldNotifyOnTemperatureProvider)
        {
            string requestTemperatureUrl = "https://localhost:7001/Thermometer";
            HttpResponseMessage responseTemperature = await _httpClient.GetAsync(requestTemperatureUrl);
            if (responseTemperature == null || responseTemperature.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"{requestTemperatureUrl} failed with status {responseTemperature?.StatusCode}");
                return false;
            }
            string temperatureText = await responseTemperature.Content.ReadAsStringAsync();

            if (!double.TryParse(temperatureText, out double temperature))
            {
                _logger.LogError($"{requestTemperatureUrl} failed with non-double temperature {temperatureText}");
                return false;
            }
            if (!shouldNotifyOnTemperatureProvider.ShouldNotify(temperature))
            {
                _logger.LogDebug($"{requestTemperatureUrl} succeeded, but temperature is OK: {temperature}℃");
                return true;
            }

            string requestNotificationUrl = "https://localhost:6001/SlackNotifier";
            var notificationRequestUrlBuilder = new UriBuilder(requestNotificationUrl);
            string encodedMessage = UrlEncoder.Default.Encode(shouldNotifyOnTemperatureProvider.GenerateMessage(temperature));
            notificationRequestUrlBuilder.Query = $"message={encodedMessage}";
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
