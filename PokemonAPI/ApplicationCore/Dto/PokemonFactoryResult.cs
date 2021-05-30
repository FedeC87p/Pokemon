using PokemonAPI.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.ApplicationCore.Dto
{
    public class PokemonFactoryResult
    {
        public bool PokemonNotFound { get; set; }
        public bool SpeciesNotFound { get; set; }
        public bool Result { get; set; }
        public IPokemon Pokemon { get; set; }
    }
}
