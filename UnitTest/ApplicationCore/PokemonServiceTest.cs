using Microsoft.Extensions.Logging;
using Moq;
using PokemonAPI;
using PokemonAPI.ApplicationCore;
using PokemonAPI.ApplicationCore.Dto;
using RichardSzalay.MockHttp;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.ApplicationCore
{
    public class PokemonServiceTest
    {
        IPokemonService _pokemonService;
        ILogger<PokemonService> _logger;

        public PokemonServiceTest()
        {
            _logger = new Mock<ILogger<PokemonService>>().Object;
        }

        [Fact]
        public async Task ShouldDeserialize_PokemonInfo()
        {
            var pokemonName = "mewtwo";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{Startup.PokemonBaseApiUrl}pokemon/{pokemonName}/").Respond("application/json", System.IO.File.ReadAllText("MockedResponse/mewtwo.json"));
            var client = new HttpClient(mockHttp);
            _pokemonService = new PokemonService(_logger, client, null);


            var result = await _pokemonService.GetPokemonInfoAsync(pokemonName);


            Assert.Equal(pokemonName, result.Name);
            Assert.Equal("mewtwo", result.Species.Name);
            Assert.Equal("https://pokeapi.co/api/v2/pokemon-species/150/", result.Species.Url);
        }

        [Fact]
        public async Task ShouldDeserialize_SpeciesDetail()
        {
            var species = new Species { Name = "rare", Url = "https://pokeapi.co/api/v2/pokemon-species/150/" };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(species.Url).Respond("application/json", System.IO.File.ReadAllText("MockedResponse/Species150.json"));
            var client = new HttpClient(mockHttp);
            _pokemonService = new PokemonService(_logger, client, null);


            var result = await _pokemonService.GetPokemonSpeciesAsync(species);


            Assert.Equal("rare", result.Habitat.Name);
            Assert.Equal("https://pokeapi.co/api/v2/pokemon-habitat/5/", result.Habitat.Url);
            Assert.Equal(114, result.Flavor.Count());
            Assert.True(result.IsLegendary);
        }

        
        [Fact]
        public async Task ShouldDeserialize_HabitatDetail()
        {
            var habitat = new Habitat { Name = "rare", Url = "https://pokeapi.co/api/v2/pokemon-habitat/5/" };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(habitat.Url).Respond("application/json", System.IO.File.ReadAllText("MockedResponse/Habitat5.json"));
            var client = new HttpClient(mockHttp);
            _pokemonService = new PokemonService(_logger, client, null);


            var result = await _pokemonService.GetPokemonHabitatAsync(habitat);


            Assert.Equal(5, result.Id);
            Assert.Equal("rare", result.Name);
        }

    }
}
