using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.ApplicationCore.Dto
{

    public class PokemonInfo
    {
        public string Name { get; set; }
        public Species Species { get; set; }
    }
    //https://pokeapi.co/api/v2/pokemon/mewtwo
    //https://pokeapi.co/api/v2/pokemon-species/150/
    //https://pokeapi.co/api/v2/pokemon-habitat/5/
}
