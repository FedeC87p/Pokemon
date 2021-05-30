using System.Threading.Tasks;
using PokemonAPI.ApplicationCore.Dto;
using PokemonAPI.DomainEntity;

namespace PokemonAPI.ApplicationCore
{
    public interface IPokemonFactory
    {
        Task<PokemonFactoryResult> CreatePokemonAsync(string pokemonName, bool translateDescription);
    }
}