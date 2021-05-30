﻿using Microsoft.Extensions.Logging;
using PokemonAPI.ApplicationCore.Dto;
using PokemonAPI.DomainEntity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.ApplicationCore
{
    public class PokemonFactory : IPokemonFactory
    {
        private readonly ILogger<PokemonFactory> _logger;
        private readonly IPokemonService _pokemonService;
        private readonly IDescriptionFactory _descriptionFactory;

        public PokemonFactory(
            ILogger<PokemonFactory> logger, 
            IPokemonService pokemonService,
            IDescriptionFactory descriptionFactory)
        {
            _logger = logger;
            _pokemonService = pokemonService;
            _descriptionFactory = descriptionFactory;
        }

        public async Task<PokemonFactoryResult> CreatePokemonAsync(string pokemonName, bool translateDescription)
        {
            var pokemonFactoryResult = new PokemonFactoryResult();

            var pokemonInfo = await _pokemonService.GetPokemonInfoAsync(pokemonName);

            if (pokemonInfo == null)
            {
                pokemonFactoryResult.PokemonNotFound = true;
                return pokemonFactoryResult;
            }
            else if (pokemonInfo?.Species == null)
            {
                pokemonFactoryResult.SpeciesNotFound = true;
                return pokemonFactoryResult;
            }

            var species = await _pokemonService.GetPokemonSpeciesAsync(pokemonInfo.Species);

            if (species == null)
            {
                pokemonFactoryResult.SpeciesNotFound = true;
                return pokemonFactoryResult;
            }

            var pokemonDto = new PokemonDto
            {
                Name = pokemonInfo.Name,
                Habitat = species.Habitat.Name, //No need Rest Call
                IsLeggendary = species.IsLegendary,
                Description = species?.Flavor?.FirstOrDefault(i => i.Language?.Name != null &&
                                                                    !String.IsNullOrWhiteSpace(i.Flavor_Text) &&
                                                            i.Language.Name.Equals("en", StringComparison.InvariantCultureIgnoreCase))?
                                                            .Flavor_Text
                                                            ?? ""
            };

            if (translateDescription &&
                !string.IsNullOrWhiteSpace(pokemonDto.Description))
            {
                pokemonDto.Description = await _descriptionFactory.GetTranslatedDescriptionAsync(pokemonDto);
            }


            pokemonFactoryResult.Pokemon = Pokemon.Instance(pokemonDto);
            pokemonFactoryResult.Result = true;
            return pokemonFactoryResult;
        }

    }
}