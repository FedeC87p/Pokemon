using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PokemonAPI.ApplicationCore.Dto;

namespace PokemonAPI.ApplicationCore
{
    public class TransationService : ITransationService
    {
        private readonly ILogger<TransationService> _logger;
        private readonly HttpClient _httpClient;

        public TransationService(
            ILogger<TransationService> logger, 
            HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<ShakespeareResponse> GetShakespeareMessageAsync(string message)
        {
            _logger.LogDebug("Get ShakespeareMessage");
            if (string.IsNullOrWhiteSpace(message))
            {
                _logger.LogDebug("Message IsNullOrWhiteSpace");
                return null;
            }

            //TODO missing cache call (and relative unit test)... sorry

            var messageEncoded = WebUtility.UrlEncode(message);
            var request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri($"{Startup.ShakespeareBaseApiUrl}?text={messageEncoded}") };
            using (var response = await _httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error: {response.StatusCode} \tShakespeare: {message}\t{request.RequestUri}");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        return null;
                    }
                    var ex = new InvalidOperationException("UnExpected Error during Get Shakespeare");
                    ex.Data.Add("Message", message);
                    throw ex;
                }
                var json = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Json Received");
                //TODO missing cache call (and relative unit test)... sorry
                return JsonSerializer.Deserialize<ShakespeareResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        public async Task<YodaResponse> GetYodaMessageAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return null;
            }

            //TODO missing cache call (and relative unit test)... sorry

            var messageEncoded = WebUtility.UrlEncode(message);
            var request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri($"{Startup.YodaBaseApiUrl}?text={messageEncoded}") };
            using (var response = await _httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error: {response.StatusCode} \tYoda: {message}\t{request.RequestUri}");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        return null;
                    }
                    var ex = new InvalidOperationException("UnExpected Error during Get Yoda");
                    ex.Data.Add("Message", message);
                    throw ex;
                }
                var json = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Json Received");
                //TODO missing cache call (and relative unit test)... sorry
                return JsonSerializer.Deserialize<YodaResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
    }
}
