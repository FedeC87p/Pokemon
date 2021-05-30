namespace PokemonAPI.DomainEntity
{
    public interface IPokemon
    {
        string Description { get;}
        string Habitat { get; }
        bool IsLeggendary { get; }
        string Name { get; }
    }
}