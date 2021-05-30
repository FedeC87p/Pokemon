using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.DomainEntity
{
    public class Pokemon : IPokemon
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Habitat { get; private set; }
        public bool IsLeggendary { get; private set; }

        private Pokemon()
        {

        }

        public static Pokemon Instance(PokemonDto pokemonDto)
        {
            return new Pokemon
            {
                Description = pokemonDto.Description,
                Habitat = pokemonDto.Habitat,
                IsLeggendary = pokemonDto.IsLeggendary,
                Name = pokemonDto.Name
            };
        }

    }
}
