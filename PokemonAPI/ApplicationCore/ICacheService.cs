using PokemonAPI.ApplicationCore.CacheKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.ApplicationCore
{
    //TODO missiong implementation for time.... sorry 
    public interface ICacheService
    {
        Task<T> GetAsync<T>(ICacheKey<T> key, Func<Task<T>> objectRetriever, int expirationHours = 36)
            where T : class;

        Task InvalidateCacheKeyAsync<T>(ICacheKey<T> key)
            where T : class;
    }
}
