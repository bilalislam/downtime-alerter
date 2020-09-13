using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ServiceWorkerCronJobDemo.Services
{
    public interface IUrlStatusChecker
    {
        Task<bool> CheckUrlAsync(string url);
    }

    public class UrlStatusChecker : IUrlStatusChecker
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<UrlStatusChecker> _logger;

        public UrlStatusChecker(IHttpClientFactory httpClientFactory, ILogger<UrlStatusChecker> logger){
            _clientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<bool> CheckUrlAsync(string url){
            try{
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e){
                _logger.LogError(e.ToString());
            }

            return await Task.FromResult(false);
        }
    }
}