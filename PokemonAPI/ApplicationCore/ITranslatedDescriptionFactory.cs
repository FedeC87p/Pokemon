using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonAPI.DomainEntity;

namespace PokemonAPI.ApplicationCore
{
    public interface ITranslatedDescriptionFactory
    {
        Task<string> GetTranslatedDescriptionAsync(PokemonDto pokemonDto);
    }
}
