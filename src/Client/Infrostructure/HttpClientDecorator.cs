using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Client.Infrostructure
{
    public class HttpClientDecorator
    {
        private readonly HttpClient _httpClient;

        public HttpClientDecorator()
        {
            _httpClient = new HttpClient();
        }

        public async Task<T?> GetAsync<T>(string url, IDictionary<string, string>? headers = null, IDictionary<string, string>? queryParams = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BuildUri(url, queryParams));
            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            var response = await _httpClient.SendAsync(request);

            //TODO handle statusCode

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GetResponce: {content}");
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T?> PostAsync<T, TU>(string url, TU data, IDictionary<string, string>? headers = null, IDictionary<string, string>? queryParams = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BuildUri(url, queryParams));
            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);

            //TODO handle statusCode

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"PostResponce: {content}");
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T?> PatchAsync<T, TU>(string url, TU data, IDictionary<string, string>? headers = null, IDictionary<string, string>? queryParams = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, BuildUri(url, queryParams));
            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);

            //TODO handle statusCode

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"PostResponce: {content}");
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task DeleteAsync(string url, IDictionary<string, string>? headers = null, IDictionary<string, string>? queryParams = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, BuildUri(url, queryParams));
            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            var responce = await _httpClient.SendAsync(request);

            //TODO handle statusCode

            //TODO Handle responce
        }

        private static Uri BuildUri(string url, IDictionary<string, string>? queryParams)
        {
            if (queryParams is not null)
            {
                var query = string.Join("&", queryParams.Select(x => $"{x.Key}={x.Value}"));
                url += $"?{query}";
            }

            return new Uri(url);
        }
    }
}
