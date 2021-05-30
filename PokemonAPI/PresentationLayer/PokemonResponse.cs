using System;

namespace PokemonAPI.PresentationLayer
{
    public class PokemonResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLeggendary { get; set; }
    }
}
