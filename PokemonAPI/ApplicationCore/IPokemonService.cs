using PokemonAPI.ApplicationCore.Dto;
using System.Threading.Tasks;

namespace PokemonAPI.ApplicationCore
{
    public interface IPokemonService
    {
        Task<PokemonInfo> GetPokemonInfoAsync(string name);
        Task<HabitatDetail> GetPokemonHabitatAsync(Habitat habitat);
        Task<SpeciesDetail> GetPokemonSpeciesAsync(Species species);
    }
}