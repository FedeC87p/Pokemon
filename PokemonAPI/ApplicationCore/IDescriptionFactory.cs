using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonAPI.DomainEntity;

namespace PokemonAPI.ApplicationCore
{
    public interface IDescriptionFactory
    {
        Task<string> GetTranslatedDescriptionAsync(PokemonDto pokemonDto);
    }
}
