namespace PokemonAPI.DomainEntity
{
    public class PokemonDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLeggendary { get; set; }

        public static PokemonDto FromEntity(IPokemon pokemon)
        {
            return new PokemonDto
            {
                Name = pokemon.Name,
                Description = pokemon.Description,
                Habitat = pokemon.Habitat,
                IsLeggendary = pokemon.IsLeggendary
            };
        }
    }
}
