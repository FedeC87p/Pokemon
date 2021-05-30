using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using PokemonAPI.DomainEntity;

namespace PokemonAPI.ApplicationCore
{
    public class PokemonManager : IPokemonManager
    {
        private readonly ILogger<PokemonManager> _logger;
        private readonly IPokemonFactory _pokemonFactory;

        public PokemonManager(
            ILogger<PokemonManager> logger, 
            IPokemonFactory pokemonFactory)
        {
            _logger = logger;
            _pokemonFactory = pokemonFactory;
        }

        public async Task<PokemonDto> GetPokemonAsync(string pokemonName)
        {
            var factoryResult = await _pokemonFactory.CreatePokemonAsync(pokemonName, false);

            if (factoryResult.Result)
            {
                return PokemonDto.FromEntity(factoryResult.Pokemon);
            }
            return null;
        }

        public async Task<PokemonDto> GetFunnyPokemonAsync(string pokemonName)
        {
            var factoryResult = await _pokemonFactory.CreatePokemonAsync(pokemonName, true);

            if (factoryResult.Result)
            {
                return PokemonDto.FromEntity(factoryResult.Pokemon);
            }
            return null;
        }
    }
}
