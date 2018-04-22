using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RobinHood
{
    class StandardHttpClient : IHttpClient
    {
        private readonly HttpClient _client;

        public StandardHttpClient()
        {
            _client = new HttpClient();
        }

        public async Task<string> GetAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }
            HttpResponseMessage response = await _client.SendAsync(requestMessage);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            if (!string.IsNullOrEmpty(authorizationToken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
            return await _client.SendAsync(requestMessage).Result.Content.ReadAsStringAsync();
        }
    }
}
