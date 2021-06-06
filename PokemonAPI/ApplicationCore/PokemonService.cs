using Microsoft.Extensions.Logging;
using PokemonAPI.ApplicationCore.CacheKeys;
using PokemonAPI.ApplicationCore.Dto;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonAPI.ApplicationCore
{
    public class PokemonService : IPokemonService
    {
        private readonly ILogger<PokemonService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cache;

        public PokemonService(
            ILogger<PokemonService> logger,
            HttpClient httpClient,
            ICacheService cache)
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
            var habitatDetail = _cache.Get(habitatCacheKey);
            if (habitatDetail != null)
            {
                return habitatDetail;
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
                _cache.Set(habitatCacheKey, result); 
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

            var pokemonCacheKey = new PokemonCacheKey(name);
            var pokemonInfo = _cache.Get(pokemonCacheKey);
            if (pokemonInfo != null)
            {
                return pokemonInfo;
            }

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
                
                var result = JsonSerializer.Deserialize<PokemonInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _cache.Set(pokemonCacheKey, result);
                return result;
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
                return JsonSerializer.Deserialize<SpeciesDetail>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
    }
}
