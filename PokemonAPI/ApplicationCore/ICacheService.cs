using PokemonAPI.ApplicationCore.CacheKeys;

namespace PokemonAPI.ApplicationCore
{
    public interface ICacheService
    {
        T Get<T>(ICacheKey<T> key)
            where T : class;

        void Set<T>(ICacheKey<T> key, T value)
            where T : class;
    }
}
