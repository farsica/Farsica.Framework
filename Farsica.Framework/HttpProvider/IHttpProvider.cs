namespace Farsica.Framework.HttpProvider
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Farsica.Framework.DataAnnotation;

    [Injectable]
    public interface IHttpProvider
    {
        Task<TResponse?> PostAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse;

        Task<TResponse?> PutAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse;

        Task<TResponse?> GetAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse;

        Task<TResponse?> DeleteAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse;
    }
}
