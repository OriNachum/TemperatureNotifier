using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SlackNotifierWS.Service
{
    public class SlackNotifierService : ISlackNotifierService
    {
        private readonly HttpClient _httpClient;

        public SlackNotifierService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task NotifyAsync(string url, string message)
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
            {
                request.Content = new StringContent($"payload={{\"text\": \"{message}\"}}\n");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                var response = await _httpClient.SendAsync(request);
            }
        }
    }
}
