using Microsoft.Extensions.Logging;
using Moq;
using PokemonAPI.ApplicationCore;
using PokemonAPI.ApplicationCore.Dto;
using PokemonAPI.DomainEntity;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.ApplicationCore
{
    public class PokemonManagerTest
    {
        IPokemonManager _pokemonManager;
        readonly  ILogger<PokemonManager> _logger;

        public PokemonManagerTest()
        {
            _logger = new Mock<ILogger<PokemonManager>>().Object;
        }

        [Fact]
        public async Task ShouldGetPokemon_CallFactory_WithNoTranslation()
        {
            string pokemonName = "mockedName";
            var mockFactory = new Mock<IPokemonFactory>();
            var mockEntity = new Mock<IPokemon>();
            mockEntity.Setup(i => i.Description).Returns("descMocked");
            mockEntity.Setup(i => i.Habitat).Returns("habitatMocked");
            mockEntity.Setup(i => i.IsLeggendary).Returns(true);
            mockEntity.Setup(i => i.Name).Returns(pokemonName);

            var factoryResult = new PokemonFactoryResult { Pokemon = mockEntity.Object, Result = true };
            mockFactory.Setup(i => i.CreatePokemonAsync(pokemonName, false).Result).Returns(factoryResult);
            _pokemonManager = new PokemonManager(_logger, mockFactory.Object);


            var result = await _pokemonManager.GetPokemonAsync(pokemonName);


            mockFactory.Verify(foo => foo.CreatePokemonAsync(pokemonName, false), Times.Once());
            Assert.NotNull(result);
            Assert.Equal("habitatMocked", result.Habitat);
            Assert.Equal("descMocked", result.Description);
            Assert.True(result.IsLeggendary);
            Assert.Equal(pokemonName, result.Name);
        }

        [Fact]
        public async Task ShouldGetFunnyPokemon_CallFactory_WithTranslation()
        {
            string pokemonName = "mockedName";
            var mockFactory = new Mock<IPokemonFactory>();
            var mockEntity = new Mock<IPokemon>();
            mockEntity.Setup(i => i.Description).Returns("descMocked");
            mockEntity.Setup(i => i.Habitat).Returns("habitatMocked");
            mockEntity.Setup(i => i.IsLeggendary).Returns(true);
            mockEntity.Setup(i => i.Name).Returns(pokemonName);

            var factoryResult = new PokemonFactoryResult { Pokemon = mockEntity.Object, Result = true };
            mockFactory.Setup(i => i.CreatePokemonAsync(pokemonName, true).Result).Returns(factoryResult);
            _pokemonManager = new PokemonManager(_logger, mockFactory.Object);


            var result = await _pokemonManager.GetFunnyPokemonAsync(pokemonName);


            mockFactory.Verify(foo => foo.CreatePokemonAsync(pokemonName, true), Times.Once());
            Assert.NotNull(result);
            Assert.Equal("habitatMocked", result.Habitat);
            Assert.Equal("descMocked", result.Description);
            Assert.True(result.IsLeggendary);
            Assert.Equal(pokemonName, result.Name);
        }

        [Fact]
        public async Task ShouldGetPokemon_ReturnNull_WhenFactoryResultFalse()
        {
            string pokemonName = "mockedName";
            var mockFactory = new Mock<IPokemonFactory>();
            var factoryResult = new PokemonFactoryResult { Pokemon = null, Result = false };
            mockFactory.Setup(i => i.CreatePokemonAsync(pokemonName, true).Result).Returns(factoryResult);
            _pokemonManager = new PokemonManager(_logger, mockFactory.Object);


            var result = await _pokemonManager.GetFunnyPokemonAsync(pokemonName);


            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldGetFunnyPokemon_ReturnNull_WhenFactoryResultFalse()
        {
            string pokemonName = "mockedName";
            var mockFactory = new Mock<IPokemonFactory>();
            var factoryResult = new PokemonFactoryResult { Pokemon = null, Result = false };
            mockFactory.Setup(i => i.CreatePokemonAsync(pokemonName, true).Result).Returns(factoryResult);
            _pokemonManager = new PokemonManager(_logger, mockFactory.Object);


            var result = await _pokemonManager.GetFunnyPokemonAsync(pokemonName);


            Assert.Null(result);
        }
    }
}
