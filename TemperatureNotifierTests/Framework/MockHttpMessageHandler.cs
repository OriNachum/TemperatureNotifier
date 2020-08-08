using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TemperatureNotifierTests.Framework
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private Queue<(string response, HttpStatusCode statusCode)> _responseQueue;

        public string Input { get; private set; }
        public int NumberOfCalls { get; private set; }


        public MockHttpMessageHandler()
        {
            _responseQueue = new Queue<(string response, HttpStatusCode statusCode)>();
        }

        public void EnqueueNextResponse(string response, HttpStatusCode statusCode)
        {
            _responseQueue.Enqueue((response, statusCode));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            NumberOfCalls++;
            if (request.Content != null) // Could be a GET-request without a body
            {
                Input = await request.Content.ReadAsStringAsync();
            }

            var (response, statusCode) = _responseQueue.Dequeue();
            return new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(response)
            };
        }
    }

}
