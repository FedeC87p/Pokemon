using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PokemonAPI.ApplicationCore.Dto
{
    public class SpeciesDetail
    {
        public Habitat Habitat { get; set; }

        [JsonPropertyNameAttribute("flavor_text_entries")]
        public IEnumerable<FlavorTextEntrie> Flavor { get; set; }
        [JsonPropertyNameAttribute("is_legendary")]
        public bool IsLegendary { get; set; }


        public class FlavorTextEntrie
        {
            public string Flavor_Text { get; set; }
            public Language Language { get; set; }
            public Version Version { get; set; }
        }

        public class Language
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        public class Version
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
