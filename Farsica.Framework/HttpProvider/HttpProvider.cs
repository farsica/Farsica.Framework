namespace Farsica.Framework.HttpProvider
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Farsica.Framework.Core;

    public sealed class HttpProvider(Lazy<IHttpClientFactory> httpClientFactory) : IHttpProvider
    {
        private readonly Lazy<IHttpClientFactory> httpClientFactory = httpClientFactory;

        public async Task<TResponse?> DeleteAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse
        {
            var client = httpClientFactory.Value.CreateHttpClient(request.ForceTls13);
            if (request.Timeout.HasValue)
            {
                client.Timeout = TimeSpan.FromMilliseconds(request.Timeout.Value);
            }

            if (string.IsNullOrEmpty(request.BaseAddress) is false)
            {
                client.BaseAddress = new Uri(request.BaseAddress);
            }

            if (request.HeaderParameters?.Count > 0)
            {
                for (var i = 0; i < request.HeaderParameters.Count; i++)
                {
                    client.DefaultRequestHeaders.Add(request.HeaderParameters[i].Key, request.HeaderParameters[i].Value);
                }
            }

            HttpResponseMessage response;

            if (request.Body is null)
            {
                response = await client.DeleteAsync(request.Uri);
            }
            else
            {
                using var req = new HttpRequestMessage(HttpMethod.Delete, request.Uri);
                if (request.Body is List<KeyValuePair<string, string?>> keyValuePairBody)
                {
                    req.Content = new FormUrlEncodedContent(keyValuePairBody);
                }
                else
                {
                    req.Content = JsonContent.Create(request.Body);
                }

                response = await client.SendAsync(req);
            }

            if (postCallHandler is not null)
            {
                await postCallHandler(response);
            }

            TResponse? result;
            if (decodeHandler is not null)
            {
                result = await decodeHandler(response);
            }
            else
            {
                result = await response.Content.ReadFromJsonAsync<TResponse>();
            }

            return response.StatusCode is not System.Net.HttpStatusCode.OK && failHandler is not null ? await failHandler(request.Request, result) : result;
        }

        public async Task<TResponse?> GetAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse
        {
            var client = httpClientFactory.Value.CreateHttpClient(request.ForceTls13);
            if (request.Timeout.HasValue)
            {
                client.Timeout = TimeSpan.FromMilliseconds(request.Timeout.Value);
            }

            if (string.IsNullOrEmpty(request.BaseAddress) is false)
            {
                client.BaseAddress = new Uri(request.BaseAddress);
            }

            if (request.HeaderParameters?.Count > 0)
            {
                for (int i = 0; i < request.HeaderParameters.Count; i++)
                {
                    client.DefaultRequestHeaders.Add(request.HeaderParameters[i].Key, request.HeaderParameters[i].Value);
                }
            }

            var response = await client.GetAsync(request.Uri);

            if (postCallHandler is not null)
            {
                await postCallHandler(response);
            }

            TResponse? result;
            if (decodeHandler is not null)
            {
                result = await decodeHandler(response);
            }
            else
            {
                result = await response.Content.ReadFromJsonAsync<TResponse>();
            }

            return response.StatusCode is not System.Net.HttpStatusCode.OK && failHandler is not null ? await failHandler(request.Request, result) : result;
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse
        {
            var client = httpClientFactory.Value.CreateHttpClient(request.ForceTls13);
            if (request.Timeout.HasValue)
            {
                client.Timeout = TimeSpan.FromMilliseconds(request.Timeout.Value);
            }

            if (string.IsNullOrEmpty(request.BaseAddress) is false)
            {
                client.BaseAddress = new Uri(request.BaseAddress);
            }

            if (request.HeaderParameters?.Count > 0)
            {
                for (var i = 0; i < request.HeaderParameters.Count; i++)
                {
                    client.DefaultRequestHeaders.Add(request.HeaderParameters[i].Key, request.HeaderParameters[i].Value);
                }
            }

            HttpResponseMessage response;
            if (request.Body is List<KeyValuePair<string, string?>> keyValuePairBody)
            {
                using var req = new HttpRequestMessage(HttpMethod.Post, request.Uri) { Content = new FormUrlEncodedContent(keyValuePairBody), };
                response = await client.SendAsync(req);
            }
            else if (request.Body is HttpContent httpContent)
            {
                response = await client.PostAsync(request.Uri, httpContent);
            }
            else
            {
                response = await client.PostAsJsonAsync(request.Uri, request.Body);
            }

            if (postCallHandler is not null)
            {
                await postCallHandler(response);
            }

            TResponse? result;
            if (decodeHandler is not null)
            {
                result = await decodeHandler(response);
            }
            else
            {
                result = await response.Content.ReadFromJsonAsync<TResponse>();
            }

            return response.StatusCode is not System.Net.HttpStatusCode.OK && failHandler is not null ? await failHandler(request.Request, result) : result;
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse, TBody>([NotNull] HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse
        {
            var client = httpClientFactory.Value.CreateHttpClient(request.ForceTls13);
            if (request.Timeout.HasValue)
            {
                client.Timeout = TimeSpan.FromMilliseconds(request.Timeout.Value);
            }

            if (string.IsNullOrEmpty(request.BaseAddress) is false)
            {
                client.BaseAddress = new Uri(request.BaseAddress);
            }

            if (request.HeaderParameters?.Count > 0)
            {
                for (var i = 0; i < request.HeaderParameters.Count; i++)
                {
                    client.DefaultRequestHeaders.Add(request.HeaderParameters[i].Key, request.HeaderParameters[i].Value);
                }
            }

            HttpResponseMessage response;

            if (request.Body is List<KeyValuePair<string, string?>> keyValuePairBody)
            {
                using var req = new HttpRequestMessage(HttpMethod.Put, request.Uri) { Content = new FormUrlEncodedContent(keyValuePairBody), };
                response = await client.SendAsync(req);
            }
            else if (request.Body is HttpContent httpContent)
            {
                response = await client.PutAsync(request.Uri, httpContent);
            }
            else
            {
                response = await client.PutAsJsonAsync(request.Uri, request.Body);
            }

            if (postCallHandler is not null)
            {
                await postCallHandler(response);
            }

            TResponse? result;
            if (decodeHandler is not null)
            {
                result = await decodeHandler(response);
            }
            else
            {
                result = await response.Content.ReadFromJsonAsync<TResponse>();
            }

            return response.StatusCode is not System.Net.HttpStatusCode.OK && failHandler is not null ? await failHandler(request.Request, result) : result;
        }
    }
}
