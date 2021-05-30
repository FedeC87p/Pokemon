using System.Threading.Tasks;
using PokemonAPI.DomainEntity;

namespace PokemonAPI.ApplicationCore
{
    public interface IPokemonManager
    {
        Task<PokemonDto> GetFunnyPokemonAsync(string pokemonName);
        Task<PokemonDto> GetPokemonAsync(string pokemonName);
    }
}