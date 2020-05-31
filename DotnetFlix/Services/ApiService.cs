using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetFlix.Services
{
    public class ApiService
    {
        private readonly HttpClient _clientFactory;

        // Set headers and values
        const string ACCEPT_HEADER = "Accept";
        const string USER_AGENT_HEADER = "User-Agent";
        const string USER_AGENT_VALUE = "WebshopProject";
        const string ACCEPT_VALUE = "application/json";

        public ApiService(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory.CreateClient();
        }

        /// <summary>
        /// Request single object of T from API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="webApiPath"></param>
        /// <returns>An object of T</returns>
        public async Task<T> GetAsync<T>(string webApiPath)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, webApiPath);

            var response = await _clientFactory.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return await DeserializeJsonAsync<T>(response);

            return default;
        }

        /// <summary>
        /// Request a collection (IEnumerable) of T from API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="webApiPath"></param>
        /// <returns>An IEnumerable of T</returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(string webApiPath)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, webApiPath);

            var response = await _clientFactory.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return await DeserializeJsonAsync<List<T>>(response);

            return null;
        }

        public async Task<T> PostAsync<T>(T model, string webApiPath)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, webApiPath);

            var postJson = JsonSerializer.Serialize(model);
            request.Content = new StringContent(postJson, Encoding.UTF8, ACCEPT_VALUE);

            // Send and receive request
            var result = await SendRequestAsync(request);
            // var response = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                return await DeserializeJsonAsync<T>(result);
            }

            return default;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            request = SetHeaders(request);
            return await _clientFactory.SendAsync(request);
        }

        private HttpRequestMessage SetHeaders(HttpRequestMessage request)
        {
            request.Headers.Add(ACCEPT_HEADER, ACCEPT_VALUE);
            request.Headers.Add(USER_AGENT_HEADER, USER_AGENT_VALUE);
            return request;
        }

        private async Task<T> DeserializeJsonAsync<T>(HttpResponseMessage content)
        {
            var json = await content.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }
    }
}