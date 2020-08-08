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

        private const double MinTemperature = 22;
        private const double MaxTemperature = 27;

        public ThermalNotifierService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> AlertTemperatureAsync()
        {
            return await NotifyTemperatureIfNeeded(new INotifyTemperatureProvider[] { new NotifyOnBreachingAllowedRange(MinTemperature, MaxTemperature), new NotifyOnRevertingToAllowedRange(MinTemperature, MaxTemperature) });
        }

        public async Task<bool> NotifyTemperatureAsync()
        {
            return await NotifyTemperatureIfNeeded(new [] { new NotifyAlways() });
        }

        private async Task<bool> NotifyTemperatureIfNeeded(INotifyTemperatureProvider[] notifyTemperatureProviders)
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

            double? previouslyKnownTemperature = ThermalNotifierServiceTemperatureHistory.LastKnownTemperature;
            ThermalNotifierServiceTemperatureHistory.LastKnownTemperature = temperature;

            INotifyTemperatureProvider notifyTemperatureProvider = FindNotificationProvider(notifyTemperatureProviders, temperature, previouslyKnownTemperature);

            if (notifyTemperatureProvider == null)
            {
                _logger.LogDebug($"{requestTemperatureUrl} succeeded, but temperature is OK: {temperature}℃");
                return true;
            }

            string requestNotificationUrl = "https://localhost:6001/SlackNotifier";
            var notificationRequestUrlBuilder = new UriBuilder(requestNotificationUrl);
            string encodedMessage = UrlEncoder.Default.Encode(notifyTemperatureProvider.GenerateMessage(temperature));
            notificationRequestUrlBuilder.Query = $"message={encodedMessage}";
            HttpResponseMessage responseNotification = await _httpClient.GetAsync(notificationRequestUrlBuilder.ToString());
            if (responseNotification == null || responseNotification.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"{requestNotificationUrl} failed with status {responseNotification?.StatusCode}");
                return false;
            }
            return true;
        }

        private static INotifyTemperatureProvider FindNotificationProvider(INotifyTemperatureProvider[] notifyTemperatureProviders, double temperature, double? previousTemperature)
        {
            INotifyTemperatureProvider chosenNotifyTemperatureProvider = null;
            foreach (INotifyTemperatureProvider notifyTemperatureProvider in notifyTemperatureProviders)
            {
                if (notifyTemperatureProvider.ShouldNotify(temperature, previousTemperature))
                {
                    chosenNotifyTemperatureProvider = notifyTemperatureProvider;
                    break;
                }
            }

            return chosenNotifyTemperatureProvider;
        }
    }
}
