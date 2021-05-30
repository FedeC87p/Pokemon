
using PokemonAPI.ApplicationCore.Dto;

namespace PokemonAPI.ApplicationCore.CacheKeys
{
    public class HabitatCacheKey : ICacheKey<HabitatDetail>
    {
        public string _habitatUrl;
        public HabitatCacheKey(string habitatUrl)
        {
            _habitatUrl = habitatUrl;
        }

        public string CacheKey => $"HabitatDetail:{_habitatUrl}";
    }
}
