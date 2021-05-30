using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonAPI.DomainEntity;

namespace PokemonAPI.ApplicationCore
{
    public class DescriptionFactory : IDescriptionFactory
    {
        private readonly ILogger<DescriptionFactory> _logger;
        private readonly ITransationService _transationService;

        public DescriptionFactory(
            ILogger<DescriptionFactory> logger,
            ITransationService transationService)
        {
            _logger = logger;
            _transationService = transationService;
        }
        
        public async Task<string> GetTranslatedDescriptionAsync(PokemonDto pokemonDto)
        {
            if (pokemonDto.IsLeggendary ||
                pokemonDto.Habitat.Equals("cave"))
            {
                var yodaResponse = await _transationService.GetYodaMessageAsync(pokemonDto.Description);
                if (yodaResponse?.Success != null &&
                    yodaResponse.Success.Total > 0 &&
                    string.IsNullOrWhiteSpace(yodaResponse.Contents.Translated))
                {
                    return yodaResponse.Contents.Translated;
                }
                else
                {
                    return pokemonDto.Description;
                }
            }
            var shakespeareResponse = await _transationService.GetShakespeareMessageAsync(pokemonDto.Description);
            if (shakespeareResponse?.Success != null &&
                shakespeareResponse.Success.Total > 0 &&
                string.IsNullOrWhiteSpace(shakespeareResponse.Contents.Translated))
            {
                return shakespeareResponse.Contents.Translated;
            }
            return pokemonDto.Description;
        }
    }
}
