using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonAPI.ApplicationCore.CacheKeys
{
    public interface ICacheKey<TItem>
    {
        string CacheKey { get; }
    }
}
