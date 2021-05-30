using PokemonAPI.ApplicationCore.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.ApplicationCore.CacheKeys
{
    public class PokemonCacheKey : ICacheKey<PokemonInfo>
    {
        public string _pokemonName;
        public PokemonCacheKey(string pokemonName)
        {
            _pokemonName = pokemonName;
        }

        public string CacheKey => $"PokemonInfo:{_pokemonName}";
    }
}
