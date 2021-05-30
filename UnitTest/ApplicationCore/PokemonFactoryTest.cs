using Microsoft.Extensions.Logging;
using Moq;
using PokemonAPI.ApplicationCore;
using PokemonAPI.ApplicationCore.Dto;
using PokemonAPI.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.ApplicationCore
{
    public class PokemonFactoryTest
    {
        readonly IPokemonFactory _pokemonFactory;
        readonly ILogger<PokemonFactory> _logger;
        readonly Mock<IPokemonService> _pokemonService;
        readonly Mock<IDescriptionFactory> _descriptionFactory;

        public PokemonFactoryTest()
        {
            _logger = new Mock<ILogger<PokemonFactory>>().Object;
            _pokemonService = new Mock<IPokemonService>();
            _descriptionFactory = new Mock<IDescriptionFactory>();

            _pokemonFactory = new PokemonFactory(_logger, _pokemonService.Object, _descriptionFactory.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldCreatePokemon_GetPokemonInfoAsync(bool translate)
        {
            string pokemonName = "mockedName";
            var species = new Species { Name = "SpecialName", Url = "http://secialUrl.it" };
            var pokemonInfo = new PokemonInfo { Name = pokemonName, Species = species };
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(pokemonInfo);


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, translate);


            _pokemonService.Verify(foo => foo.GetPokemonInfoAsync(pokemonName), Times.Once());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldCreatePokemon_GetPokemonSpeciesAsync_WhenPokemonInfoExist(bool translate)
        {
            string pokemonName = "mockedName";
            var species = new Species { Name = "SpecialName", Url = "http://secialUrl.it" };
            var pokemonInfo = new PokemonInfo { Name = pokemonName, Species = species };
            var habitat = new Habitat { Name = "HabitatName", Url = "http://habitatNameUrl.it" };
            var speciesDetail = new SpeciesDetail { Habitat = habitat };
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(pokemonInfo);
            _pokemonService.Setup(i => i.GetPokemonSpeciesAsync(pokemonInfo.Species).Result).Returns(speciesDetail);


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, translate);


            _pokemonService.Verify(i => i.GetPokemonInfoAsync(pokemonName), Times.Once());
            _pokemonService.Verify(i => i.GetPokemonSpeciesAsync(pokemonInfo.Species), Times.Once());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldCreatePokemon_NotCallGetPokemonSpeciesAsync_WhenSpeciesIsNull(bool translate)
        {
            string pokemonName = "mockedName";
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(new PokemonInfo());


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, translate);


            _pokemonService.Verify(foo => foo.GetPokemonSpeciesAsync(It.IsAny<Species>()), Times.Never);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldCreatePokemon_ReturnNotFoundResult_WhenSpeciesIsNull(bool translate)
        {
            string pokemonName = "mockedName";
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(new PokemonInfo());


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, translate);


            Assert.False(result.Result);
            Assert.True(result.SpeciesNotFound);
            Assert.Null(result.Pokemon);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldCreatePokemon_NotCallGetPokemonSpeciesAsync_WhenGetSpeciesReturnNull(bool translate)
        {
            string pokemonName = "mockedName";
            var species = new Species { Name = "SpecialName", Url = "http://secialUrl.it" };
            var pokemonInfo = new PokemonInfo { Name = pokemonName, Species = species };
            var habitat = new Habitat { Name = "HabitatName", Url = "http://habitatNameUrl.it" };
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(pokemonInfo);
            _pokemonService.Setup(i => i.GetPokemonSpeciesAsync(pokemonInfo.Species)).ReturnsAsync((SpeciesDetail)null);


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, translate);


            Assert.False(result.Result);
            Assert.True(result.SpeciesNotFound);
            Assert.Null(result.Pokemon);
        }

        [Fact]
        public async Task ShouldCreatePokemon_CallGetTranslatedDescriptionAsync_WhenTranslateIsTrueAndDescriptionNotEmpty()
        {
            string pokemonName = "mockedName";
            var species = new Species { Name = "SpecialName", Url = "http://secialUrl.it" };
            var pokemonInfo = new PokemonInfo { Name = pokemonName, Species = species };
            var habitat = new Habitat { Name = "HabitatName", Url = "http://habitatNameUrl.it" };
            var speciesDetail = new SpeciesDetail { Habitat = habitat, Flavor = new List<SpeciesDetail.FlavorTextEntrie> { new SpeciesDetail.FlavorTextEntrie { Language = new SpeciesDetail.Language { Name = "en", Url = "urlEn" }, Flavor_Text = "desc" } } };
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(pokemonInfo);
            _pokemonService.Setup(i => i.GetPokemonSpeciesAsync(pokemonInfo.Species)).ReturnsAsync(speciesDetail);


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, true);


            _descriptionFactory.Verify(i => i.GetTranslatedDescriptionAsync(It.IsAny<PokemonDto>()), Times.Exactly(1));
        }

        [Fact]
        public async Task ShouldCreatePokemon_CallNotGetTranslatedDescriptionAsync_WhenTranslateIsFalseAndDescriptionNotEmpty()
        {
            string pokemonName = "mockedName";
            var species = new Species { Name = "SpecialName", Url = "http://secialUrl.it" };
            var pokemonInfo = new PokemonInfo { Name = pokemonName, Species = species };
            var habitat = new Habitat { Name = "HabitatName", Url = "http://habitatNameUrl.it" };
            var speciesDetail = new SpeciesDetail { Habitat = habitat };
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(pokemonInfo);
            _pokemonService.Setup(i => i.GetPokemonSpeciesAsync(pokemonInfo.Species)).ReturnsAsync(speciesDetail);


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, false);


            _descriptionFactory.Verify(i => i.GetTranslatedDescriptionAsync(It.IsAny<PokemonDto>()), Times.Never);
        }

        [Fact]
        public async Task ShouldCreatePokemon_CallNotGetTranslatedDescriptionAsync_WhenTranslateIsTrueAndDescriptionEmpty()
        {
            string pokemonName = "mockedName";
            var species = new Species { Name = "SpecialName", Url = "http://secialUrl.it" };
            var pokemonInfo = new PokemonInfo { Name = pokemonName, Species = species };
            var habitat = new Habitat { Name = "HabitatName", Url = "http://habitatNameUrl.it" };
            var speciesDetail = new SpeciesDetail { Habitat = habitat, Flavor = new List<SpeciesDetail.FlavorTextEntrie> { new SpeciesDetail.FlavorTextEntrie { Language = new SpeciesDetail.Language { Name = "en", Url = "urlEn" } } } };
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(pokemonInfo);
            _pokemonService.Setup(i => i.GetPokemonSpeciesAsync(pokemonInfo.Species)).ReturnsAsync(speciesDetail);


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, true);


            _descriptionFactory.Verify(i => i.GetTranslatedDescriptionAsync(It.IsAny<PokemonDto>()), Times.Never);
        }

        [Fact]
        public async Task ShouldCreatePokemon_ReturnCorrectData_WhenTraslateIsFalse()
        {
            string pokemonName = "mockedName";
            var species = new Species { Name = "SpecialName", Url = "http://secialUrl.it" };
            var pokemonInfo = new PokemonInfo { Name = pokemonName, Species = species };
            var habitat = new Habitat { Name = "HabitatName", Url = "http://habitatNameUrl.it" };
            var speciesDetail = new SpeciesDetail { Habitat = habitat, IsLegendary = true, Flavor = new List<SpeciesDetail.FlavorTextEntrie> { new SpeciesDetail.FlavorTextEntrie { Language = new SpeciesDetail.Language { Name = "en", Url = "urlEn" }, Flavor_Text = "descNotTranslated" } } };
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(pokemonInfo);
            _pokemonService.Setup(i => i.GetPokemonSpeciesAsync(pokemonInfo.Species)).ReturnsAsync(speciesDetail);
            _descriptionFactory.Setup(i => i.GetTranslatedDescriptionAsync(It.IsAny<PokemonDto>())).ReturnsAsync("DescTraslated");


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, false);


            Assert.True(result.Result);
            Assert.NotNull(result.Pokemon);
            Assert.Equal("descNotTranslated", result.Pokemon.Description);
            Assert.Equal("HabitatName", result.Pokemon.Habitat);
            Assert.True(result.Pokemon.IsLeggendary);
            Assert.Equal(pokemonName, result.Pokemon.Name);
        }

        [Fact]
        public async Task ShouldCreatePokemon_ReturnCorrectData_WhenLeggendaryIsFalse()
        {
            string pokemonName = "mockedName";
            var species = new Species { Name = "SpecialName", Url = "http://secialUrl.it" };
            var pokemonInfo = new PokemonInfo { Name = pokemonName, Species = species };
            var habitat = new Habitat { Name = "HabitatName", Url = "http://habitatNameUrl.it" };
            var speciesDetail = new SpeciesDetail { Habitat = habitat, IsLegendary = false, Flavor = new List<SpeciesDetail.FlavorTextEntrie> { new SpeciesDetail.FlavorTextEntrie { Language = new SpeciesDetail.Language { Name = "en", Url = "urlEn" }, Flavor_Text = "desc" } } };
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(pokemonInfo);
            _pokemonService.Setup(i => i.GetPokemonSpeciesAsync(pokemonInfo.Species)).ReturnsAsync(speciesDetail);
            _descriptionFactory.Setup(i => i.GetTranslatedDescriptionAsync(It.IsAny<PokemonDto>())).ReturnsAsync("DescTraslated");


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, true);


            Assert.False(result.Pokemon.IsLeggendary);
        }

        [Fact]
        public async Task ShouldCreatePokemon_ReturnCorrectData_WhenTraslateIsTrue()
        {
            string pokemonName = "mockedName";
            var species = new Species { Name = "SpecialName", Url = "http://secialUrl.it" };
            var pokemonInfo = new PokemonInfo { Name = pokemonName, Species = species };
            var habitat = new Habitat { Name = "HabitatName", Url = "http://habitatNameUrl.it" };
            var speciesDetail = new SpeciesDetail { Habitat = habitat, IsLegendary = true, Flavor = new List<SpeciesDetail.FlavorTextEntrie> { new SpeciesDetail.FlavorTextEntrie { Language = new SpeciesDetail.Language { Name = "en", Url = "urlEn" }, Flavor_Text = "desc" } } };
            _pokemonService.Setup(i => i.GetPokemonInfoAsync(pokemonName).Result).Returns(pokemonInfo);
            _pokemonService.Setup(i => i.GetPokemonSpeciesAsync(pokemonInfo.Species)).ReturnsAsync(speciesDetail);
            _descriptionFactory.Setup(i => i.GetTranslatedDescriptionAsync(It.IsAny<PokemonDto>())).ReturnsAsync("DescTraslated");


            var result = await _pokemonFactory.CreatePokemonAsync(pokemonName, true);


            Assert.True(result.Result);
            Assert.NotNull(result.Pokemon);
            Assert.Equal("DescTraslated", result.Pokemon.Description);
            Assert.Equal("HabitatName", result.Pokemon.Habitat);
            Assert.True(result.Pokemon.IsLeggendary);
            Assert.Equal(pokemonName, result.Pokemon.Name);
        }
    }
}
