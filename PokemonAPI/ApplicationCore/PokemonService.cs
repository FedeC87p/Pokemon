using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PokemonAPI.ApplicationCore.CacheKeys;
using PokemonAPI.ApplicationCore.Dto;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PokemonAPI.ApplicationCore.Dto;

namespace PokemonAPI.ApplicationCore
{
    public class PokemonService : IPokemonService
    {
        private readonly ILogger<PokemonService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public PokemonService(
            ILogger<PokemonService> logger,
            HttpClient httpClient,
            IMemoryCache cache)
        {
            _logger = logger;
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<HabitatDetail> GetPokemonHabitatAsync(Habitat habitat)
        {
            _logger.LogDebug("Start GetPokemonHabitat");
            if (string.IsNullOrWhiteSpace(habitat?.Url ?? ""))
            {
                _logger.LogDebug("Url IsNullOrWhiteSpace");
                return null;
            }

            var habitatCacheKey = new HabitatCacheKey(habitat.Url);
            if (_cache != null)//TODO missing cache services (and relative unit test)... sorry
            {
                var habitatDetail = _cache.Get<HabitatDetail>(habitatCacheKey.CacheKey); 
                if (habitatDetail != null)
                {
                    return habitatDetail;
                }
            }

            var request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(habitat.Url) };
            using (var response = await _httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error: {response.StatusCode} \tHabitat {habitat.Name}\t{habitat.Url}\t{request.RequestUri}");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    var ex = new InvalidOperationException("UnExpected Error during Get Pokemon Habitat");
                    ex.Data.Add("Habitat", habitat.Name);
                    throw ex;
                }
                var json = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Json Recived");
                var result =  JsonSerializer.Deserialize<HabitatDetail>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (_cache != null)//TODO missing cache services (and relative unit test)... sorry
                {
                    _cache.Set(habitatCacheKey.CacheKey, result); //TODO missing cache services (and relative unit test)... sorry
                }
                return result;
            }
        }

        public async Task<PokemonInfo> GetPokemonInfoAsync(string name)
        {
            _logger.LogDebug("Start GetPokemonInfo");
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogDebug("Name IsNullOrWhiteSpace");
                return null;
            }

            //TODO missing cache call (and relative unit test)... sorry

            var request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri($"{Startup.PokemonBaseApiUrl}pokemon/{name}/") };
            using (var response = await _httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error: {response.StatusCode} \tPokemon {name}\t{request.RequestUri}");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    var ex = new InvalidOperationException("UnExpected Error during Get Pokemon Info");
                    ex.Data.Add("Pokemon", name);
                    throw ex;
                }
                var json = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Json Received");
                //TODO missing cache call (and relative unit test)... sorry
                return JsonSerializer.Deserialize<PokemonInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        public async Task<SpeciesDetail> GetPokemonSpeciesAsync(Species species)
        {
            _logger.LogDebug("Get PokemonSpecies");
            if (string.IsNullOrWhiteSpace(species?.Url??""))
            {
                _logger.LogDebug("Url IsNullOrWhiteSpace");
                return null;
            }

            //TODO missing cache call (and relative unit test)... sorry

            _logger.LogDebug("Start GetPokemonSpecies");
            var request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(species.Url) };
            using (var response = await _httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error: {response.StatusCode} \tSpecies {species.Name}\t{species.Url}\t{request.RequestUri}");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    var ex = new InvalidOperationException("UnExpected Error during Get Pokemon Info");
                    ex.Data.Add("Species", species.Name);
                    throw ex;
                }
                var json = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Json Received");
                //TODO missing cache call (and relative unit test)... sorry
                return JsonSerializer.Deserialize<SpeciesDetail>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
    }
}
