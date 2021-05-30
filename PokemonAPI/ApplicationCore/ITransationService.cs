using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonAPI.ApplicationCore.Dto;

namespace PokemonAPI.ApplicationCore
{
    public interface ITransationService
    {
        Task<YodaResponse> GetYodaMessageAsync(string message);
        Task<ShakespeareResponse> GetShakespeareMessageAsync(string message);
    }
}
